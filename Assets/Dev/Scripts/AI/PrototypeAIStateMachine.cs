using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[Serializable]
public class NpcAIDataBoard
{
    public PrototypeAIStateMachine.State State;
    public NpcAIStateParam Param;

    public Sprite AttackSprite;
    public Sprite DefaultSprite;

    public float DetectFov;
    public float DetectDistance;

    public bool Flip;

    [Header("Patroll")] [Space(1)] 
    public float Patroll_MovemenetSpeed;
    public List<Transform> PatrollPoints;

    [Header("Trace")] [Space(1)] 
    public float Trace_MovementSpeed;

    
    [Header("LastSighting")] [Space(1)] 
    public float WaitSecAfterLastSighting;
    
    [Header("Attack")] [Space(1)]
    public float WaitSecAfterAttack;
}

public abstract class NpcAIStateParam
{
}

public static class PrototypeAIStateMachine
{
    public delegate void State(NpcAIController controller, NpcAIDataBoard board, NpcAIStateParam aParam);


    public class PatrollParam : NpcAIStateParam
    {
        public int CurrentPointIndex = -1;
    }

    public static void Patroll(NpcAIController controller, NpcAIDataBoard board, NpcAIStateParam aParam)
    {
        Debug.Assert(board.PatrollPoints != null, "patroll point 비엇음");
        Debug.Assert(board.PatrollPoints.Count > 0, "patroll point 비엇음");
        if (aParam is not PatrollParam param)
        {
            Debug.Assert(false);
            return;
        }

        if (param.CurrentPointIndex == -1)
        {
            param.CurrentPointIndex = 0;
        }

        controller.Renderer.sprite = board.DefaultSprite;

        var point = board.PatrollPoints[param.CurrentPointIndex];

        controller.Agent.speed = board.Patroll_MovemenetSpeed;
        controller.Agent.SetDestination(point.position);

        board.Flip = PrototypeAIUtil.IsFlip(controller.transform.position, point.position);

        // 현재 추적 포인트에 도달했는지 검사
        if (Vector2.Distance(point.position, controller.transform.position) <= 0.5f)
        {
            if (param.CurrentPointIndex + 1 >= board.PatrollPoints.Count)
            {
                param.CurrentPointIndex = 0;
            }
            else
            {
                param.CurrentPointIndex++;
            }
        }

        // 플레이어 발견 검사
        var playerController =
                PrototypeAIUtil.FindPlayerControllerWithFov(
                    controller.transform.position,
                    controller.Agent.desiredVelocity,
                    board.DetectFov, board.DetectDistance)
            ;
        
        if (playerController)
        {
            board.Param = new TraceParam()
            {
                Target = playerController
            };
            board.State = Trace;
        }
    }

    public class TraceParam : NpcAIStateParam
    {
        public PlayerController Target;

        public Vector2? LastSightingPoint;
    }

    public static void Trace(NpcAIController controller, NpcAIDataBoard board, NpcAIStateParam aParam)
    {
        if (aParam is not TraceParam param)
        {
            Debug.Assert(false);
            return;
        }

        Debug.Assert(param.Target);

        controller.Renderer.sprite = board.AttackSprite;
        param.LastSightingPoint = param.Target.transform.position;

        var playerController =
                PrototypeAIUtil.FindPlayerControllerWithFov(
                    controller.transform.position,
                    controller.Agent.desiredVelocity,
                    board.DetectFov, board.DetectDistance)
            ;
        
        if (playerController == false)
        {
            if (param.LastSightingPoint.HasValue)
            {
                board.State = LastSighting;
                board.Param = new LastSightingPram()
                {   
                    Point = (Vector2) param.LastSightingPoint
                };
            }
            else
            {
                board.State = Patroll;

                var point = PrototypeAIUtil.CloserPoint(controller.transform.position, board.PatrollPoints);
            
                board.Param = new PatrollParam()
                {   
                    CurrentPointIndex = board.PatrollPoints.FindIndex(x=>x == point)
                };
            }
        }
        else if (Vector2.Distance(param.Target.transform.position, controller.transform.position) < 1f)
        {
            param.LastSightingPoint = param.Target.transform.position;
            board.State = Attack;
            board.Param = new AttackParam()
            {
                Target = param.Target
            };
        }
        else
        {
            controller.Agent.speed = board.Trace_MovementSpeed;
            param.LastSightingPoint = param.Target.transform.position;
            controller.Agent.SetDestination(param.Target.transform.position);
            
            board.Flip = PrototypeAIUtil.IsFlip(controller.transform.position, param.Target.transform.position);
        }
    }

    public class LastSightingPram : NpcAIStateParam
    {
        public Vector2 Point;
    }

    public static void LastSighting(NpcAIController controller, NpcAIDataBoard board, NpcAIStateParam aParam)
    {
        if (aParam is not LastSightingPram param)
        {
            Debug.Assert(false);
            return;
        }

        controller.Renderer.sprite = board.AttackSprite;
        var playerController =
                PrototypeAIUtil.FindPlayerControllerWithFov(
                    controller.transform.position,
                    controller.Agent.desiredVelocity,
                    board.DetectFov, board.DetectDistance)
            ;
        
        if (playerController)
        {
            board.State = Trace;
            board.Param = new TraceParam()
            {   
                Target = playerController
            };
        }
        else if (Vector2.Distance(param.Point, controller.transform.position) <= 0.5f)
        {
            board.State = WaitForSec;

            var point = PrototypeAIUtil.CloserPoint(controller.transform.position, board.PatrollPoints);

            board.Param = new WaitForSecParam()
            {
                NextState = Patroll,
                NextParam = new PatrollParam()
                {   
                    CurrentPointIndex = board.PatrollPoints.FindIndex(x=>x == point)
                },
                Internval = board.WaitSecAfterLastSighting
            };


        }
        else
        {
            
            board.Flip = PrototypeAIUtil.IsFlip(controller.transform.position, param.Point);
            controller.Agent.SetDestination(param.Point);
        }
    }
    
    public class AttackParam : NpcAIStateParam
    {
        public PlayerController Target;
    }

    public static void Attack(NpcAIController controller, NpcAIDataBoard board, NpcAIStateParam aParam)
    {
        if (aParam is not AttackParam param)
        {
            Debug.Assert(false);
            return;
        }

        Debug.Assert(param.Target);
        controller.Renderer.sprite = board.AttackSprite;

        if (param.Target.Interaction.TryGetContractInfo(out ActorContractInfo info) &&
            info.TryGetBehaviour(out IBActorStemina stemina))
        {
            stemina.Properties.SetValue(EStatCode.Health, 0);
        }
        
        board.Flip = PrototypeAIUtil.IsFlip(controller.transform.position, param.Target.transform.position);

        board.State = WaitForSec;
        board.Param = new WaitForSecParam
        {
            NextState = Trace,
            NextParam = new TraceParam()
            {
                Target = param.Target,
            },
            Internval = board.WaitSecAfterAttack
        };
    }

    public class WaitForSecParam : NpcAIStateParam
    {
        public State NextState;
        public NpcAIStateParam NextParam;
        public float Internval;

        public float Timer;
    }

    public static void WaitForSec(NpcAIController controller, NpcAIDataBoard board, NpcAIStateParam aParam)
    {
        if (aParam is not WaitForSecParam param)
        {
            Debug.Assert(false);
            return;
        }

        Debug.Assert(param.NextState != null);
        
        controller.Renderer.sprite = board.DefaultSprite;
        
        var playerController =
                PrototypeAIUtil.FindPlayerControllerWithFov(
                    controller.transform.position,
                    controller.Agent.desiredVelocity,
                    board.DetectFov, board.DetectDistance)
            ;
        
        if (playerController)
        {
            board.State = Trace;
            board.Param = new TraceParam()
            {   
                Target = playerController
            };
        }

        if (param.Timer >= param.Internval)
        {
            board.State = param.NextState;
            board.Param = param.NextParam;
        }
        else
        {
            param.Timer += Time.deltaTime;
        }
    }
}

public static class PrototypeAIUtil
{
    public static bool IsFlip(Vector2 myPos, Vector2 destination)
    {
        var dir = destination - myPos;

        return dir.x > 0;
    }
    public static Transform CloserPoint(Vector2 point, List<Transform> list)
    {
        Transform closerPoint = null;
        float minDis = Mathf.Infinity;
        foreach (var result in list)
        {
            float dis = Vector2.Distance(result.position, point);
            if (minDis >= dis)
            {
                minDis = dis;
                closerPoint = result;
            }
        }

        return closerPoint;

    }
    public static List<Transform> FindWithFov(Vector2 position, Vector2 dir, float fov, float maxDistance, int laskMask)
    {
        var ql= Quaternion.Euler(0f, 0f, fov * -0.5f);
        var qr= Quaternion.Euler(0f, 0f, fov * 0.5f);

        dir = dir.normalized;

        var leftRay = ql * dir * maxDistance;
        var rightRay = qr * dir * maxDistance;
        
        Debug.DrawRay(position, leftRay);
        Debug.DrawRay(position, rightRay);
        Debug.DrawRay(position, dir * maxDistance);

        var overlaps = Physics2D.OverlapCircleAll(position, maxDistance, laskMask);
        List<Transform> list = new(overlaps.Length);
        foreach (var col in overlaps)
        {
            Vector2 n2o = (Vector2)col.transform.position - position;
            n2o = n2o.normalized;

            float dot = Vector2.Dot(n2o, dir);
            dot = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (dot <= fov * 0.5f)
            {
                list.Add(col.transform);
            }
        }
        return list;
    }

    public static PlayerController FindPlayerControllerWithFov(Vector2 position, Vector2 dir,float fov, float maxDistance)
    {
        var list = FindWithFov(position, dir, fov, maxDistance, LayerMask.GetMask("Player"));

        if (list == null) return null;

        var results = list
                .Where(x => x.GetComponent<PlayerController>() != null)
                .Select(x => x.GetComponent<PlayerController>())
                .ToList()
            ;

        if (results.Count < 1)
            return null;
        

        bool isObstacleCloser = false;
        float minDis = Mathf.Infinity;
        var raycastResults = Physics2D.RaycastAll(
            position, 
            (Vector2)results[0].transform.position - position,
            maxDistance,
            LayerMask.GetMask("Obstacle", "Player"));
        
        foreach (var result in raycastResults)
        {
            float dis = Vector2.Distance(result.point, position);
            if (minDis >= dis)
            {
                minDis = dis;
                isObstacleCloser = result.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle");
            }
        }

        if (isObstacleCloser)
            return null;


        return results[0];
    }
}