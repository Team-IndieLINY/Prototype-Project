using System;
using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Extensions;
using UnityEngine;
using UnityEngine.AI;


public class NpcAIController : MonoBehaviour
{
    [SerializeField]
    private NpcAIDataBoard _board;

    [SerializeField]
    private AgentOverride2d _agent;

    [SerializeField] 
    private ViewVisualizer _visualizer;
    
    public NavMeshAgent Agent => _agent.Agent;

    private Vector2 _lastVelocity;

    [SerializeField] public SpriteRenderer Renderer;

    private void Awake()
    {
        _board.State = PrototypeAIStateMachine.Patroll;
        _board.Param = new PrototypeAIStateMachine.PatrollParam()
        {
        };
    }

    private void Update()
    {
        _visualizer.Renderer.material.SetFloat("_radius", _board.DetectDistance);
        
        if (_board.State == null)
        {
            _board.State = PrototypeAIStateMachine.Patroll;
        }
        
        _board.State?.Invoke(this, _board, _board.Param);
        
        var ql= Quaternion.Euler(0f, 0f, _board.DetectFov * -0.5f);
        
        if(Mathf.Approximately(Agent.desiredVelocity.magnitude, 0f) == false)
            _lastVelocity = Agent.desiredVelocity;
        
        _visualizer.UpdateView(ql * (Vector2)_lastVelocity.normalized, _board.DetectFov, _board.DetectDistance);

        Renderer.flipX = _board.Flip;
    }
}