using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using DG.Tweening;

public class WalkingState : StateBase
{
    Transform playerTransform;

    public override void OnStateEnter()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnStateStay()
    {
        float speed = 50f;
        if (playerTransform.position.x < 10)
        {
            playerTransform.DOMoveX(playerTransform.position.x + speed * Time.deltaTime, 0.1f).SetEase(Ease.Linear);
        } else
        {
            playerTransform.position = new Vector3(-10, playerTransform.position.y, playerTransform.position.z);
        }
    }
}
