using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript_protoenemy : MonoBehaviour
{
    public float speed = 2f;
    private GameObject player;
    [SerializeField] private LayerMask whatIsEnemy;
    public int attackRange = 2;
    private int rotDir = 1;
    // Start is called before the first frame update
    void Start()
    {
        individualAttackRange = attackRange + Random.Range(0, 3);
        player =  GameObject.Find("Player");

        if(Random.Range(0,2) == 1)
        {
            rotDir *= -1;
        }
    }
    private int individualAttackRange;
    private bool onAttPos = false;
    // Update is called once per frame
    void FixedUpdate()
    {

        float distance = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log("Distance to player: " + distance);
        if(distance >= individualAttackRange + 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
            onAttPos = false;
        }
        if (distance <= individualAttackRange && CheckSurroundings() == true)
        {
            onAttPos = true;
           
        }else if (distance <= individualAttackRange && CheckSurroundings() == false)
        {
            // ROTATE 5S to random dir and check again surroundings
            if (!isRotating)
            {
            
                transform.RotateAround(player.transform.position, Vector3.up, rotDir * speed * 4 * Time.fixedDeltaTime);
                StartCoroutine(RotatePosition(3));
            }
        }
    }

    private bool CheckSurroundings()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2, whatIsEnemy);
        if (colliders.Length > 0)
        {
            isRotating = false;
            return false;
        }
        else
        {
            isRotating = true;
            return true;
        }
    }

    public int avoidRotateSpeed = 8;

    private bool isRotating = false;
    public IEnumerator RotatePosition(float time)
    {
     
        yield return new WaitForSeconds(time);
        isRotating = true;

    }


    public bool friendlyCollision = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            friendlyCollision = true;
           // other.gameObject.GetComponent<EnemyScript_protoenemy>().friendlyCollision = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {

            friendlyCollision = false;
    }
}
