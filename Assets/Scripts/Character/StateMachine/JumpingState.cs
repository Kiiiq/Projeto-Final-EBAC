using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using DG.Tweening;
public class JumpingState : StateBase
{
    Transform playerTransform;
    Tween jumpTween;

    public override void OnStateEnter()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnStateStay()
    {

        if(playerTransform.position.y<=0) jumpTween=playerTransform.DOMoveY(3, 1f).SetEase(Ease.Linear);
        if (playerTransform.position.y >= 3) jumpTween = playerTransform.DOMoveY(0, 1f).SetEase(Ease.Linear);


    }

    public override void OnStateExit() { 
        jumpTween.Kill();
        jumpTween = playerTransform.DOMoveY(0, 0.5f).SetEase(Ease.Linear);
    }
}
