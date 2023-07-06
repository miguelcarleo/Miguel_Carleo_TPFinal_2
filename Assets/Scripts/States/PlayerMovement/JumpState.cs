using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MovementBaseState
{
    public override void EnterState(Player_Movement movement)
    {
        if (movement.previousState == movement.idle) movement.animator.SetTrigger("Jump");
        else if (movement.previousState == movement.walking || movement.previousState == movement.running) movement.animator.SetTrigger("RunJump");
    }

    public override void UpdateState(Player_Movement movement)
    {
        if(movement.jumped && movement.IsGrounded())
        {
            movement.jumped = false;
            if (movement.hzInput == 0 && movement.vInput == 0) movement.SwitchState(movement.idle);
            else if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.running);
            else movement.SwitchState(movement.walking);
        }
    }
}
