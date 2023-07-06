using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : MovementBaseState
{
    public override void EnterState(Player_Movement movement)
    {
        movement.animator.SetBool("Running", true);
    }

    public override void UpdateState(Player_Movement movement)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.walking);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.idle);

        if (movement.vInput < 0) movement.currentMoveSpeed = movement.backwardsRunningSpeed;
        else movement.currentMoveSpeed = movement.runningSpeed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.previousState = this;
            ExitState(movement, movement.jump);
        }
    }
    void ExitState(Player_Movement movement, MovementBaseState state)
    {
        movement.animator.SetBool("Running", false);
        movement.SwitchState(state);
    }
}
