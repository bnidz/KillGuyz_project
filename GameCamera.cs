using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    private Transform playerobject;

    public bool isAttacking = false;


    private void Awake()
    {
        playerobject = GameObject.Find("Player").transform;
    }
    //  public Quaternion toRotationTarget;

    void LateUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

        if(isAttacking)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            Vector3 targetPosition_ = new Vector3(target.position.x, transform.position.y, playerobject.position.z -15);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition_, ref velocity, smoothTime);
        }

        // Smoothly move the camera towards that target position
        // transform.rotation = Quaternion.Lerp(transform.rotation, toRotationTarget, 2 * Time.deltaTime);
        transform.LookAt(playerobject);
    }
}