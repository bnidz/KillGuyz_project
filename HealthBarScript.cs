using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public Slider slider;
    public PlayerManager pmanager;

    // Start is called before the first frame update
    void Start()
    {
        pmanager = GameObject.FindObjectOfType<PlayerManager>();
        slider.maxValue = pmanager.playerHealth;
    }

    // Update is called once per frame
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}