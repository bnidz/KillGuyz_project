using UnityEngine;

public class MouseAimCamera : MonoBehaviour
{ 
    public GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;

    //MODIFY THIS TO IS POINTER OVER UI OBJECT
    //TURN PLAYER IF NOT ACTIVE TARGET
    //AND JSTICK CONTRLS TO PLAYER TO CAMERA LOCAL SPACE

    public FixedTouchField ftouchField;
    private RaycastShootcomplete rayshoot;

    void Start()
    {
        rayshoot = GameObject.FindObjectOfType<RaycastShootcomplete>();
        offset = target.transform.position - transform.position;
    }

    public GameObject player;
    Quaternion rotation;


    void LateUpdate()
    {

        if(ftouchField.Pressed && rayshoot.isShooting)
        {
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            target.transform.Rotate(0, horizontal, 0);

            float desiredAngle = target.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
            transform.position = target.transform.position - (rotation * offset);
            transform.LookAt(target.transform);
        }

        if(!rayshoot.isShooting)
        {
            //smootthaus playerin taakse
            //turni screen pressistä

            target.transform.rotation = player.transform.rotation;
            //transform.position = offset;
            transform.LookAt(target.transform);
            if (ftouchField.Pressed)
            {
                float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
                player.transform.Rotate(0, horizontal, 0);

                //float desiredAngle = target.transform.eulerAngles.y;
                //Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
                //transform.position = target.transform.position - (rotation * offset);
                //transform.LookAt(target.transform);
            }
        }
    }
}