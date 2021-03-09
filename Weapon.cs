using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{

    public new string name;

    public int damage;
    public int clipSize;
    public int Ammo;
    public int bulletsInClip;
    public float attackRange;

    public int projectileAmount;

    public float fireRate;
    public float reloadTime;
    public float speedXplier;
    public float enemyKnockBack;

    public string fireSFX;

    //vois olla vaikka meshi ja animaatiot :o :o
}