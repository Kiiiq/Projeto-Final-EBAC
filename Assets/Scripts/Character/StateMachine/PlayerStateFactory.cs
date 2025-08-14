

public class PlayerStateFactory {
    PlayerStateManager playerStateManager;

    public PlayerStateFactory(PlayerStateManager playerStateManager)
    {
        this.playerStateManager = playerStateManager;
    }

    public PlayerStateBase GetRootState()
    {
        if (playerStateManager.groundCheck.IsGrounded())
        {
            return InGroungState();
        }
        else
        {
            return NotInGroundState();
        }
    }

    public WalkingNullState WalkingNullState()
    {
        return new WalkingNullState(playerStateManager, this);
    }

    public PlayerStateBase WalkingState()
    {
        return new WalkingState(playerStateManager,this);
    }

    public PlayerStateBase JumpingState()
    {
        return new JumpingState(playerStateManager, this);
    }

    public PlayerStateBase DashingState()
    {
        return new DashingState(playerStateManager, this);
    }

    public PlayerStateBase RunningState()
    {
        return new RunningState(playerStateManager, this);
    }

    public PlayerStateBase AttackingState()
    {
        return new AttackingState(playerStateManager, this);
    }

    public PlayerStateBase DefendingState()
    {
        return new DefendingState(playerStateManager, this);
    }

    public PlayerStateBase InGroungState()
    {
        return new InGroundState(playerStateManager, this);
    }

    public PlayerStateBase NotInGroundState()
    {
        return new NotInGroundState(playerStateManager, this);
    }

    public PlayerStateBase IdleState()
    {
        return new IdleState(playerStateManager, this);
    }

    public PlayerStateBase DeadState()
    {
        return new DeadState(playerStateManager, this);
    }
}
