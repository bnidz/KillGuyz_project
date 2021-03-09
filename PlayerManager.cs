using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int playerHealth = 40;
    public HealthBarScript healthbar;
    public float playerSpeed;

    //Shooting related stuff
    //public int bullets = 30;
    //public float attackRange = 30f;
    //public float fireRate = 1f;
    //public int gunDamage = 2;

    public float damage;
    public int clipSize;
    public int Ammo;
    public int bulletsInClip;
    public float attackRange;

    public float fireRate;
    public float reloadTime;
    public float speedXplier;
    public float enemyKnockBack;


    public Weapon[] playerWeapons;
    public Weapon selectedWeapon;
    public bool isDead = false;


    //TODO:
    //UI placeholders for showing clip / ammo
    //UI placeholder button for gun

    void Start()
    {
        uiman = GameObject.FindObjectOfType<UIManager>();
        //TODO weapon selection code:
        selectedWeapon = playerWeapons[0];
        // InitGUN();

        healthbar.slider.maxValue = playerHealth;
        healthbar.slider.value = playerHealth;
    }


    //take damage and update healthbar
    //also death


    private UIManager uiman;

    public void TakeDamage(int dmg)
    {
        playerHealth -= dmg;
        healthbar.SetHealth(playerHealth);

        if(playerHealth <= 0)
        {
            //DEATH
            isDead = true;
            uiman.HIDE_gameplay_stuff();

        }
    }
}