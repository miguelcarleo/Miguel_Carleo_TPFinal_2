using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponClassManager : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint leftHandIK;
    public Transform recoilFollowPos;

    public WeaponManager[] weapons;
    int currentWeaponIndex;

    private bool weaponAvailable = false;

    private void Awake()
    {
        currentWeaponIndex = 0;
        for(int i = 0; i < weapons.Length; i++)
        {
            if (i == 0) weapons[i].gameObject.SetActive(true);
            else weapons[i].gameObject.SetActive(false);
        }
    }

    public void SetCurrentWeapon(WeaponManager weapon)
    {
        leftHandIK.data.target = weapon.leftHandTarget;
        leftHandIK.data.hint = weapon.leftHandHint;
    }

    public void ChangeWeapon(float direction)
    {
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        if (direction < 0)
        {
            if (currentWeaponIndex == 0) currentWeaponIndex = weapons.Length - 1;
            else currentWeaponIndex--;
        }
        else
        {
            if (currentWeaponIndex == weapons.Length - 1) currentWeaponIndex = 0;
            else currentWeaponIndex++;
        }
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0 && weaponAvailable)
        {
            ChangeWeapon(Input.mouseScrollDelta.y);
        }
    }

    public void WeaponAvailable() => weaponAvailable = true;
}
