using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camRotater : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public Quaternion toRotationTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localRotation = Quaternion.Lerp(transform.rotation, toRotationTarget, 10 * Time.deltaTime);
    }
}
