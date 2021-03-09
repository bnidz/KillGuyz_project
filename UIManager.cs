using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button[] gunButtons;
    private PlayerManager pm;
    private RaycastShootcomplete shoot;

    //reset scene stuff
    public Button ResetButton;
    public GameObject BIG_Reset_Button;

    //Text Shower stuff
    public Text centerInfoText;

    // Start is called before the first frame update
    void Start()
    {
        shoot = GameObject.FindObjectOfType<RaycastShootcomplete>();
        pm = GameObject.FindObjectOfType<PlayerManager>();
        InitGunButtons();
    }

    private void InitGunButtons()
    {
        //to check available guns first!! TODO ----
        for (int i = 0; i < gunButtons.Length; i++)
        {
            gunButtons[i].gameObject.SetActive(true);
        }

        //Starting gun --- How to reference known objects integer???
        gunButtons[0].gameObject.GetComponentInChildren<Image>().enabled = true;

    }

    // Update is called once per frame
    public void GunButtonPress(Button btn)
    {
        int integer = Int16.Parse(btn.name);
        if (pm.selectedWeapon.name == pm.playerWeapons[integer].name)
        {
            Debug.Log("RELOADING!!!!");
            return;
            //RELOAD CODE
        }
        pm.selectedWeapon = pm.playerWeapons[integer];
        for (int i = 0; i < gunButtons.Length; i++)
        {
            gunButtons[i].transform.GetChild(0).gameObject.GetComponentInChildren<Image>().enabled = false;

        }

        gunButtons[integer].transform.GetChild(0).gameObject.GetComponentInChildren<Image>().enabled = true;
        //init the gun in shoot script
        shoot.InitWeapon();

    }

    public IEnumerator ShowInfoTextCenter(string text, float duration)
    {

        centerInfoText.text = text;
        yield return new WaitForSeconds(duration);
        string emptystring = "";
        centerInfoText.text = emptystring;

    }

    //weapon UI CODEX
    public Text ammoText;

    public void ShowWeaponAmmo(string text)
    { 

        ammoText.text = text;
        //yield return new WaitForSeconds(duration);
        //string emptystring = "";
        //centerInfoText.text = emptystring;

    }

    public void HIDE_gameplay_stuff()
    {

        for (int i = 0; i < gunButtons.Length; i++)
        {
            gunButtons[i].transform.gameObject.GetComponentInChildren<Image>().enabled = false;
        }
        ResetButton.gameObject.GetComponent<Image>().enabled = false;
        BIG_Reset_Button.SetActive(true);
    }

    //DO PAUSE
}