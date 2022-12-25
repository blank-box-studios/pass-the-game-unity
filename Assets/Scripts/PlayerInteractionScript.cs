using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionScript : MonoBehaviour
{

    //To be removed.
    public TextMeshProUGUI testDragText;
    



    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] float dragDistance;
    [SerializeField] bool isDragging;
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
        calculateDragDistance(); //Function that pulls back the object to determine strength of push through mouse drag
        rotateIndicator();
    }

    void addForce(float draggedDistance)
    {

        float forceToAdd = Mathf.Clamp( Mathf.RoundToInt(draggedDistance), 200, 1000);

        Vector3 dir = Quaternion.AngleAxis(angleForForce, Vector3.forward) * Vector3.right;
        testDragText.text = forceToAdd.ToString();
        playerRb.AddForce(-dir * forceToAdd);

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

        gfx.localScale = new Vector3(1f, dragDistance / 1000.0f, 0);
    }

    void calculateDragDistance()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isDragging = true;
            lastPosition = Input.mousePosition;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isDragging = false;
            Debug.Log("Mouse moved " + dragDistance + " while button was down.");
            addForce(dragDistance); // Add force to the object based on the distance dragged
            dragDistance = 0;
        }

        if (isDragging)
        {
            var newPosition = Input.mousePosition;
            dragDistance = Vector2.Distance(lastPosition, newPosition);
        }
    }
}
