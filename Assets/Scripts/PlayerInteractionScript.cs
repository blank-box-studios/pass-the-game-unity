using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float dragDistance, draggedDistance;
    [SerializeField] bool isDragging, initialPositionSet;
    Vector2 lastPosition;

    [SerializeField] Transform pullIndicatorObject, gfx;
    [SerializeField] float angleForForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pullBack(); //Function that pulls back the object to determine strength of push through mouse drag
        rotateIndicator();

    }

    void addForce()
    {
        Vector3 dir = Quaternion.AngleAxis(angleForForce, Vector3.forward) * Vector3.right;
        rb.AddForce(-dir * draggedDistance / 10);


    }

    void rotateIndicator()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        angleForForce = angle;

        pullIndicatorObject.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

        gfx.localScale = new Vector3(0.1f, dragDistance / 100.0f, 0);
    }

    void pullBack()
    {


        if (Input.GetButtonDown("Fire1"))
        {
            isDragging = true;
            if (!initialPositionSet)
            {
                lastPosition = Input.mousePosition;
                initialPositionSet = true;
            }

        }

        if (Input.GetButtonUp("Fire1"))
        {
            isDragging = false;
            Debug.Log("Mouse moved " + dragDistance + " while button was down.");
            draggedDistance = dragDistance;

            dragDistance = 0;

            initialPositionSet = false;

            addForce();
        }

        if (isDragging)
        {
            var newPosition = Input.mousePosition;
            dragDistance = Vector2.Distance(lastPosition, newPosition);
        }
    }
}
