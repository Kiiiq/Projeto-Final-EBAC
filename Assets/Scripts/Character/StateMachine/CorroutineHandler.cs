using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CorroutineHandler : MonoBehaviour
{
    public PlayerStateManager _playerStateManager;

    public CorroutineHandler(PlayerStateManager playerStateManager)
    {
        _playerStateManager = playerStateManager;
    }



    public IEnumerator AttackCorroutine(AttackingState attackingState, WeaponSO weapon)
    {
        if (_playerStateManager.stamina.UseStamina(weapon.staminaCost))
        {
            Debug.Log("Attack Started");
            _playerStateManager.Attacking = true;
            _playerStateManager.CanAttack = false;
            _playerStateManager.animator.Play(weapon.animationName);
            weapon.hitbox.enabled = true;


            yield return new WaitForSeconds(weapon.attackDuration);
            weapon.hitbox.enabled = false;
        }
        attackingState.SwitchToIdle();


        yield return new WaitForSeconds(weapon.cooldown);
        _playerStateManager.CanAttack = true;
    }

    public IEnumerator Dash(DashingState dashingState)
    {
        if (_playerStateManager.stamina.UseStamina(_playerStateManager.DashStaminaCost) && _playerStateManager.CanDash)
        {
            _playerStateManager.CanDash = false;
            _playerStateManager.IsDashing = true;
            _playerStateManager.animator.Play("Dash");
            
            _playerStateManager.player.transform.DOMove(
                _playerStateManager.player.transform.position + (_playerStateManager.playerSprite.transform.forward * _playerStateManager.DashMultiplier*_playerStateManager.DefaultSpeed)/3,
                _playerStateManager.DashTime
            ).SetEase(Ease.InCubic);

            Debug.Log("Dashing Started");
            yield return new WaitForSeconds(_playerStateManager.DashTime);
        }
        
            
        
        _playerStateManager.IsDashing = false;
        dashingState.SwitchToIdle();
        _playerStateManager.animator.SetBool("Dash", false);
        Debug.Log("Dashing Ended");

        yield return new WaitForSeconds(_playerStateManager.DashCooldown);
        _playerStateManager.CanDash = true;
        Debug.Log("Dashing Cooldown Ended");
        
    }

    public IEnumerator JumpDuration()
    {
        _playerStateManager.IsJumping = true;
        yield return new WaitForSeconds(_playerStateManager.jumpTime);
        _playerStateManager.IsJumping = false;
    }
}
