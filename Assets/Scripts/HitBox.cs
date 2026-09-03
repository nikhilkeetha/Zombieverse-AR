using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitBox : MonoBehaviour
{
    public ZombieHealth health;

    public void OnRaycastHit(RaycastWeapon weapon, Vector3 direction) {
        health.TakeDamage(weapon.damage, direction); //here enemy will take damage
    }
}
