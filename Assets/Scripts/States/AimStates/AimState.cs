using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimState : AimBaseState
{
    public override void EnterState(Player_Camera aim)
    {
        aim.animator.SetBool("Aiming", true);
        aim.currentFov = aim.adsFov;
    }
    public override void UpdtateState(Player_Camera aim)
    {
        if (Input.GetKeyUp(KeyCode.Mouse1)) aim.SwitchState(aim.noAim);
    }
}
