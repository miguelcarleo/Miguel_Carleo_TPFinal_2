using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MovementBaseState
{
    public override void EnterState(Player_Movement movement)
    {
        movement.animator.SetBool("Walking", true);
    }

    public override void UpdateState(Player_Movement movement)
    {
        if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.running);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.idle);

        if (movement.vInput < 0) movement.currentMoveSpeed = movement.backwardsWalkingSpeed;
        else movement.currentMoveSpeed = movement.walkingMoveSpeed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.previousState = this;
            ExitState(movement, movement.jump);
        }
    }

    void ExitState(Player_Movement movement, MovementBaseState state)
    {
        movement.animator.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}
