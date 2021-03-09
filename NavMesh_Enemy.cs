using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh_Enemy : MonoBehaviour
{

    // Start is called before the first frame update
    private GameObject player;
    public NavMeshAgent thisAgent;

    //enemy stats

    private int enemyHealth;
    private int attackRange;
    private int damage;
    private float attackSpeed;
    public float shookDelay = .7f;
    private float enemySpeed;

    private Material normal_mat;
    public Material attack_mat;
    public Material shookMaterial;

    //Scriptable object
    public Enemy thisEnemy;

    public LayerMask enemies;

    private PlayerManager playerManager;
    private GameObject spawnobu;

    void Start()
    {

        spawnobu = GameObject.Find("Spawner");
        spawner = spawnobu.GetComponent<Spawner>();
        InitEnemy();
        //set shooktimer
        shookTimer = shookDelay;

        thisAgent.speed = enemySpeed;
        mInput = GameObject.FindObjectOfType<MoveToClickInput>();

        //set enemy depended material
        materialObu = gameObject.transform.GetChild(0).gameObject;
        materialObu.GetComponent<MeshRenderer>().material = normal_mat;

        player = GameObject.Find("Player");
        playerManager = player.GetComponent<PlayerManager>();
        thisAgent.SetDestination(player.transform.position);

    }

    private void InitEnemy()
    {
        
        enemyHealth = thisEnemy.health;
        attackRange = thisEnemy.attRange;
        damage = thisEnemy.damage;
        attackSpeed = thisEnemy.attSpeed;
        normal_mat = thisEnemy.basemat;
        enemySpeed = thisEnemy.enemySpeed;

    }

    // Update is called once per frame
    //active target stuff
    public bool isActiveTarget = false;

    void FixedUpdate()
    {
        thisAgent.SetDestination(player.transform.position);

        if (thisAgent.remainingDistance <= attackRange && thisAgent.remainingDistance >= .1f)
        {
            //Attack player
            if (attackDelay == false && !isShoked)
            {
                StartCoroutine(AttackPlayer());
            }
        }

        if(isShoked)
        {
            ShookDelay();
        }
    }

    public Spawner spawner;
    public void TakeDamage(int damage)
    {

        //maybe reference to the gun to know if shookdamage or not
        //Debug.Log("ENEMY TOOK DAMAGE!");
        enemyHealth -= damage;
        isShoked = true;
        if(isShoked)
        {
            newShot = true;
        }

        if(enemyHealth <= 0)
        {
            //Destroy(this.gameObject);
            

            //test for waveman
            spawner.activeEnemies.Remove(spawner.activeEnemies[spawner.activeEnemies.Count -1]);
            Debug.Log("Active enemies: " + spawner.activeEnemies.Count);

            this.gameObject.SetActive(false);
            AssignNewTarget(transform.position, 5f);
            if(isActiveTarget)
            {
                //hmm
            }
        }
    }

    private bool isShoked = false;
    private float shookTimer;
    private bool newShot = false;

    public void ShookDelay()
    {

        if(newShot && isShoked)
        {
            shookTimer = shookDelay;
            newShot = false;
        }

        shookTimer -= Time.deltaTime;

        if(shookTimer <= 0)
        {
            isShoked = false;
            shookTimer = shookDelay;
        }   
    }

    private MoveToClickInput mInput;
    private float this_dist;
    private float attack_range;
    private Transform newTarget;


    void AssignNewTarget(Vector3 center, float radius)
    {
        newTarget = null;

        attack_range = playerManager.attackRange;
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, enemies);
        for (int i = 0; i < hitColliders.Length; i++)
        {
           this_dist = Vector3.Distance(hitColliders[i].transform.position, player.transform.position);
            if (this_dist < attack_range)
            {
                attack_range = this_dist;
                if(hitColliders[i].transform != gameObject.transform && hitColliders[i].transform != player.transform)
                {
                    newTarget = hitColliders[i].transform;
                }
            }
        }
        if(newTarget != null)
        {
            mInput.NewTargetInRange(newTarget);
            Debug.Log("New TArget!!!!!!");
        }
    }

    private GameObject materialObu; 
    public bool attackDelay = false;

    //Attack player Code here
    public IEnumerator AttackPlayer()
    {
        attackDelay = true;
        materialObu.GetComponent<MeshRenderer>().material = attack_mat;

        yield return new WaitForSeconds(attackSpeed);

        //Debug.Log("ENEMY ATTACK");
        playerManager.TakeDamage(damage);
        attackDelay = false;

        yield return new WaitForSeconds(.1f);
        materialObu.GetComponent<MeshRenderer>().material = normal_mat;
        //think about suppresing fire logix here
    }
}