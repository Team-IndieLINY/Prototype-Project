using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRProject.Utils.Log;

public class CameraFollower : MonoBehaviour
{
    public float FollowSpeed;
    public float FixedZPosition = -10f;
    
    private PlayerController _playerController;

    private void Start()
    {
        var coms = FindObjectsOfType<PlayerController>();
        var legnth = coms.Length;
        if (legnth > 1)
        {
            XLog.LogError($"camera follower에서 {legnth}개의 player controller를 찾음. player controller는 단 하나만 존재해야함.", "default");
        }
        else if (legnth == 0)
        {
            XLog.LogError("camera follower에서 player controller를 찾을 수 없음.", "default");
        }

        _playerController = coms[0];
    }

    private void Update()
    {
        if (_playerController == false) return;
        
        Follow(_playerController.transform);
    }

    private void Follow(Transform target)
    {
        var myPos = (Vector2)transform.position;
        var targetPos = (Vector2)target.position;
        
        if (Vector2.Distance(myPos, targetPos) < 0.0001f) return;

        Vector2 movedPos = Vector2.Lerp(myPos, targetPos, FollowSpeed * Time.deltaTime);

        transform.position = new Vector3(movedPos.x, movedPos.y, FixedZPosition);
    }
}
