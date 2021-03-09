using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class MoveToClickInput : MonoBehaviour
{
    public Transform moveThis;
    public LayerMask hitLayers;
    float speed;
    Vector3 targetPos;

    //turn to shoot stuff
    public bool shooting = false;
    //private LayerMask Enemy = LayerMask.GetMask("Enemy");
    public LayerMask Enemy;

    private PlayerManager pm;
    public Animator anim;


    //onscreen controls
    public Joystick jstick;

    Vector3 moveDir;

    private bool IsPointerOverUIObject()
    {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (results.Count > 0) return results[0].gameObject.tag == "excludeUiTouch"; else return false;
    }
    //private camRotater camrot;

    private void Awake()
    {
        rayshoot = GameObject.FindObjectOfType<RaycastShootcomplete>();
        pm = gameObject.GetComponent<PlayerManager>();
        speed = pm.playerSpeed;
       // camrot = GameObject.FindObjectOfType<camRotater>();
    }

    private void Start()
    {
        StartCoroutine(CalcVelocity());

    }

    private Vector3 _targetPos;
    private bool enemyAtRange = false;
    private Transform shootTarget;
    private RaycastShootcomplete rayshoot;

    void FixedUpdate()
    {
        moveDir.x = jstick.Horizontal;
        moveDir.y = jstick.Vertical;
        // moveDir.y = currVel.z;

       // moveThis.transform.Translate(new Vector3 (moveDir.x, 0f, moveDir.y) * 10 * Time.deltaTime, Space.Self);
        anim.SetFloat("dirX", moveDir.x);
        anim.SetFloat("dirY", moveDir.y);



                    //stuf for rotating camera with touch

                  //  Vector3 touchpos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);

//        Debug.Log("X " + moveDir.x + " Y " + moveDir.y);


        if (!pm.isDead)
        {

            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);

            if(shootTarget != null )
            {
                if(enemyAtRange || shootTarget.gameObject.activeSelf == true)
                {
                    FaceToShoot(shootTarget);
                    float dist = Vector3.Distance(shootTarget.transform.position, moveThis.transform.position);
                    if (dist >= pm.attackRange || shootTarget.gameObject.activeSelf == false)
                    {
                        enemyAtRange = false;
                        rayshoot.isShooting = false;
                    }
                }
            }
            if (Input.GetMouseButtonDown(0) && IsPointerOverUIObject() == false)
            {

                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, Enemy))
                {
                    float dist = Vector3.Distance(hit.transform.gameObject.transform.position, moveThis.transform.position); //dangerous!! input mousepos
                    if (pm.attackRange >= dist)
                    {
                        shootTarget = hit.transform.gameObject.transform;
                        enemyAtRange = true;
                        //Debug.Log("Face to Shoot!");
                    }
                    else
                    {
                        enemyAtRange = false;
                    }
                    return;
                }

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, hitLayers) && IsPointerOverUIObject() == false)
                {
                    targetPos = new Vector3 (hit.point.x, moveThis.transform.position.y, hit.point.z);
                   // Debug.Log("New Move target!");
                }
            }

        Vector3 relativePos = targetPos - transform.position;
        //not to look to destination if shooting at something
        //disabled for joystick
          //  moveThis.transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            if(relativePos.x != 0)
            {
                if(!enemyAtRange)
                {
                   // Quaternion toRotation = Quaternion.LookRotation(relativePos);
                  //  transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
                    //camrot.toRotationTarget = toRotation;
                }
            }
        }
    }

    void FaceToShoot(Transform shootTarget)
    {
        // IF GROUND ENEMY KEEP BARREL HORIZONTAL
        // IF FLYING ENEMY THEN  USE CODE BELOW


        Vector3 relativePos = shootTarget.transform.position - transform.position;

        //mark active target
        rayshoot.isShooting = true;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
        //Debug.Log("Out of Range");

    }
    public void NewTargetInRange(Transform target)
    {
        FaceToShoot(target);
        shootTarget = target;
        enemyAtRange = true;
    }
    Vector3 prevPos;
    Vector3 currVel;

    private IEnumerator CalcVelocity()
    {
        while (Application.isPlaying)
        {
            // Position at frame start
            prevPos = transform.position;
            // Wait till it the end of the frame
            yield return new WaitForEndOfFrame();
            // Calculate velocity: Velocity = DeltaPosition / DeltaTime
            currVel = (prevPos - transform.position) / Time.deltaTime;
//            Debug.Log(currVel);
        }

    }
}

// click to move
// clikc enemy to aim there
// clikc enemy to attack if melee weapon
// click same weapon to reload