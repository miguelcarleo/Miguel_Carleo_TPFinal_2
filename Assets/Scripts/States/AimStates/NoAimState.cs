using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAimState : AimBaseState
{
    public override void EnterState(Player_Camera aim)
    {
        aim.animator.SetBool("Aiming", false);
        aim.currentFov = aim.noAimFov;
    }
    public override void UpdtateState(Player_Camera aim)
    {
        if (Input.GetKey(KeyCode.Mouse1)) aim.SwitchState(aim.aim);
    }
}
