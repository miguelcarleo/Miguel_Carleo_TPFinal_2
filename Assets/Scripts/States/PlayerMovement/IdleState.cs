using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    public override void EnterState(Player_Movement movement)
    {

    }

    public override void UpdateState(Player_Movement movement)
    {
        if(movement.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.running);
            else movement.SwitchState(movement.walking);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.previousState = this;
            movement.SwitchState(movement.jump);
        }
    }
}
