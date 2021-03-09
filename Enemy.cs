using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public GameObject baseEnemy;

    public new string name;

    public int health;
    public int attRange;
    public int damage;
    public float attSpeed;

    public Material basemat;
    //new values

    public float enemySpeed;
    //projectile enemy or not
}