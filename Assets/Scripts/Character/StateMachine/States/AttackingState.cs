using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class AttackingState : PlayerStateBase
{
    public AttackingState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    : base(playerStateManager, playerStateFactory) {
        weapon = _playerStateManager.currentWeapon;
    }

    public WeaponScript weaponScript => weapon.weapon.GetComponent<WeaponScript>();
    public WeaponSO weapon;

    public override void OnStateEnter()
    {
        if (!_playerStateManager.Attacking)
        {
            weapon.hitbox = weapon.weapon.GetComponent<Collider>();
            weaponScript.ClearColliders();
            _playerStateManager.corroutineHandler.StartCoroutine(_playerStateManager.corroutineHandler.AttackCorroutine(this, weapon));
            
        }


    }
    public override void OnStateExit()
    {
        _playerStateManager.Attacking = false;
        _playerStateManager.animator.SetBool("Attack", false);
        _playerStateManager.stamina.AbleRegen();
        
    }
    public override void OnStateUpdate()
    {
        
    }
    public override void CheckSwitchState()
    {
        // Logic to check if we should switch to another state
    }
    public override void InitializeSubState()
    {
        // Initialize any sub-states if needed
    }

    public void SwitchToIdle()
    {
        SwitchState(_playerStateFactory.IdleState());
    }
   
}

