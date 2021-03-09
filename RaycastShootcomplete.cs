using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(LineRenderer))]
public class RaycastShootcomplete : MonoBehaviour
{

    private int gunDamage;                                            // Set the number of hitpoints that this gun will take away from shot objects with a health script
    private float fireRate;                                           // Number in seconds which controls how often the player can fire
    private float weaponRange;
    public int projectileCount;

    public string firesound;
    // Distance in Unity units over which the player can fire

    private float hitForce;                                         // Amount of force which will be added to objects with a rigidbody shot by the player
    public Transform gunEnd;                                               // Holds a reference to the gun end object, marking the muzzle location of the gun

    //private Camera fpsCam;
    // Holds a reference to the first person camera

    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);       // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
    //private AudioSource gunAudio;                                        // Reference to the audio source which will play our shooting sound effect
    private LineRenderer laserLine;                                        // Reference to the LineRenderer component which will display our laserline
    private float nextFire;                                                // Float to store the time the player will be allowed to fire again, after firing
    private float gunReloadTime;

    //playermanager
    private PlayerManager pm;

    //test shooting gamera stuff
    //maybe face closest enemies
    private GameCamera gamcam;

    void Start()
    {
        uiman = GameObject.FindObjectOfType<UIManager>();

        gamcam = GameObject.FindObjectOfType<GameCamera>();
        pm = GameObject.FindObjectOfType<PlayerManager>();
        //set atributes for the gun
        InitWeapon();
        laserLine = gameObject.GetComponent<LineRenderer>();
        // Get and store a reference to our LineRenderer component
        // Get and store a reference to our AudioSource component
        //gunAudio = GetComponent<AudioSource>();
        // Get and store a reference to our Camera by searching this GameObject and its parents
        //  fpsCam = GetComponentInParent<Camera>();
    }

    //weapon UI CODEX
    public UIManager uiman;

    private bool changingWeapon = false;
    public void InitWeapon()
    {
        gunDamage = pm.selectedWeapon.damage;
        fireRate = pm.selectedWeapon.fireRate;
        weaponRange = pm.selectedWeapon.attackRange;
        hitForce = pm.selectedWeapon.enemyKnockBack;
        projectileCount = pm.selectedWeapon.projectileAmount;
        firesound = pm.selectedWeapon.fireSFX;

        gunReloadTime = pm.selectedWeapon.reloadTime;

        // Debug.Log("Changing Weapon!!!");
        changingWeapon = true;
        StartCoroutine(ChangingWeapon());
        //SCRIPT TO UPDATE Bullets & stuff to the objects values

        pm.selectedWeapon.bulletsInClip = pm.selectedWeapon.clipSize;
        pm.selectedWeapon.Ammo -= pm.selectedWeapon.clipSize;

        uiman.ShowWeaponAmmo(pm.selectedWeapon.ToString() + " CLIP: " + pm.selectedWeapon.bulletsInClip.ToString() +" / "+ pm.selectedWeapon.Ammo.ToString());
        

    }
    public float ChangeTime = .8f;
    public bool isShooting = false;
    private IEnumerator ChangingWeapon()
    {
        
        yield return new WaitForSeconds(ChangeTime);
        changingWeapon = false;
       // Debug.Log("Changing DONEEE!!!");

    }

    void Update()
    {
        if(isShooting && Time.time > nextFire && !changingWeapon && pm.selectedWeapon.bulletsInClip > 0 &&!isLoading && !pm.isDead)
        {
            for (int i = 0; i < projectileCount; i++)
            {
                GunshotRay();
            }
            pm.selectedWeapon.bulletsInClip -= 1;
            AudioManager.Instance.PlaySound(firesound);
            uiman.ShowWeaponAmmo(pm.selectedWeapon.ToString() + " CLIP: " + pm.selectedWeapon.bulletsInClip.ToString() + " / " + pm.selectedWeapon.Ammo.ToString());
        }
        if(pm.selectedWeapon.bulletsInClip == 0 && pm.selectedWeapon.Ammo >=0 && !isLoading)
        {
           isLoading = true;
           StartCoroutine( ReloadWeapon());
        }
    }

    private Vector3 spread;
    private float dur = 0.05f;
    private void GunshotRay()
    {

        Vector3 direction = gunEnd.transform.forward; // your initial aim.
        //spread += gunEnd.transform.up * UnityEngine.Random.Range(-1f, 1f); // add random up or down (because random can get negative too)
        spread += gunEnd.transform.right * UnityEngine.Random.Range(-1f, 1f); // add random left or right

        // Using random up and right values will lead to a square spray pattern. If we normalize this vector, we'll get the spread direction, but as a circle.
        // Since the radius is always 1 then (after normalization), we need another random call. 
        direction += spread.normalized * UnityEngine.Random.Range(0f, 0.2f);




        //// Update the time when our player can fire next
        nextFire = Time.time + fireRate;
        //pm.selectedWeapon.Ammo -= 1;
        //// Start our ShotEffect coroutine to turn our laser line on and off
       

        //// Create a vector at the center of our camera's viewport
        Vector3 rayOrigin = gunEnd.transform.forward;

        //// Declare a raycast hit to store information about what our raycast has hit
        RaycastHit hit;

        //// Set the start position for our visual effect for our laser to the position of gunEnd
        laserLine.SetPosition(0, gunEnd.position);

        // Check if our raycast has hit anything
        if (Physics.Raycast(gunEnd.position, direction, out hit, weaponRange))
        {
            // Set the end position for our laser line 
            laserLine.SetPosition(1, hit.point);
           // Debug.DrawLine(gunEnd.transform.position, hit.point, Color.green, dur);
            // Get a reference to a health script attached to the collider we hit
            NavMesh_Enemy health = hit.collider.GetComponent<NavMesh_Enemy>();

            // If there was a health script attached
            if (health != null)
            {
                // Call the damage function of that script, passing in our gunDamage variable
                health.TakeDamage(gunDamage);
            }

            // Check if the object we hit has a rigidbody attached
            if (hit.rigidbody != null)
            {
                // Add force to the rigidbody we hit, in the direction from which it was hit
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
        else
        {
            // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
            laserLine.SetPosition(1, gunEnd.transform.position + (direction * weaponRange));
           // Debug.DrawRay(gunEnd.transform.position, direction, Color.red, dur);
        }
        StartCoroutine(ShotEffect());

    }

    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        // gunAudio.Play();

        // Turn on our line renderer
        laserLine.enabled = true;
     
        //Wait for .07 seconds
        yield return dur;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }

    public bool isLoading = false;

    private IEnumerator ReloadWeapon()
    { 

        //RELOAD EFFEXTS
        AudioManager.Instance.PlaySound("pump");

        yield return new WaitForSeconds(gunReloadTime);
        if(pm.selectedWeapon.Ammo >= pm.selectedWeapon.clipSize)
        {
            pm.selectedWeapon.bulletsInClip = pm.selectedWeapon.clipSize;
            pm.selectedWeapon.Ammo -= pm.selectedWeapon.clipSize;

        }
        if(pm.selectedWeapon.Ammo < pm.selectedWeapon.clipSize)
        {
            pm.selectedWeapon.bulletsInClip = pm.selectedWeapon.Ammo;
            pm.selectedWeapon.Ammo -= pm.selectedWeapon.bulletsInClip;
        }

        uiman.ShowWeaponAmmo(pm.selectedWeapon.ToString() + " CLIP: " + pm.selectedWeapon.bulletsInClip.ToString() + " / " + pm.selectedWeapon.Ammo.ToString());

        isLoading = false;
    }
}
