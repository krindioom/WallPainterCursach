using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlot : MonoBehaviour
{
    public GunParameters Weapon { get; set; }

    public WallMap Map;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /*var instatiatedShot = Instantiate(Weapon.Shoot, transform);
            instatiatedShot.trigger.AddCollider(Map);
            Map.Shot = instatiatedShot;
            Debug.Log(Map.Shot);*/

            Debug.Log(Weapon.name);
            Weapon.MakeShoot(transform);
        }
    }
}
