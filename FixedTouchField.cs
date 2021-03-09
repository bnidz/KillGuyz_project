using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;
    
    public bool Pressed;

    // Use this for initialization
    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Pressed)
        //{
        //    if (PointerId >= 0 && PointerId < Input.touches.Length)
        //    {
        //        TouchDist = Input.touches[PointerId].position - PointerOld;
        //        PointerOld = Input.touches[PointerId].position;
        //    }
        //    else
        //    {
        //        TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
        //        PointerOld = Input.mousePosition;
        //    }
        //}
        //else
        //{
        //  //  TouchDist = new Vector2();
        //}
    }

    float rotX;
    float rotY;

    float xVelocity;
    float yVelocity;

    public GameObject camrotator;

    public float snappiness = 1f;

    public GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}