using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionScript : MonoBehaviour
{

    //To be removed.
    public TextMeshProUGUI testDragText;

    [SerializeField] TextMeshProUGUI fuelText;


    [SerializeField] float walkingSpeed;
    [SerializeField] Rigidbody2D playerRb;
    
    [SerializeField] float dragDistance;
    [SerializeField] bool isDragging;
    Vector2 lastPosition;

    [SerializeField] Transform pullIndicatorObject, gfx;
    [SerializeField] float angleForForce;

    [SerializeField] Transform goal;
    [SerializeField] float distanceToGoal;
    [SerializeField] float FuelMax = 100;
    [SerializeField] float FuelCurrent;

    float horizontalmovement;
    bool onGround = false;


    // Start is called before the first frame update
    void Start()
    {
        
        FuelCurrent= FuelMax;
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
    }


    // Update is called once per frame
    // CONSIDERING: Getting all the movement queued up in a list, and then processing it in fixed update
    void Update()
    {
        if (onGround)
        {
            horizontalmovement = Input.GetAxis("Horizontal");
            onGround = false;
        }
        
        calculateDragDistance(); //Function that pulls back the object to determine strength of push through mouse drag
        rotateIndicator();
        updateCameraZoom();
    }

    //TODO: STUB Use Fixed Update for physics
    void FixedUpdate()
    {

        transform.Translate(new Vector3(horizontalmovement * 2 * Time.deltaTime, 0,0));
    }
    
    void updateCameraZoom()
    {
        distanceToGoal = Vector2.Distance(transform.position, goal.position) /2;
        Camera.main.orthographicSize = Mathf.Clamp(distanceToGoal, 6, 10);
    }

    
    void addForce(float draggedDistance)
    {

        float forceToAdd = Mathf.Clamp( Mathf.RoundToInt(draggedDistance/30), 1, 75);

        if (forceToAdd < FuelCurrent)
        {
            forceToAdd = FuelCurrent;
        }

        Vector3 dir = Quaternion.AngleAxis(angleForForce, Vector3.forward) * Vector3.right;
        testDragText.text = forceToAdd.ToString();
        playerRb.AddForce(-dir * forceToAdd, ForceMode2D.Impulse);

        FuelCurrent -= forceToAdd;
        fuelText.text = $"{FuelCurrent} / {FuelMax}";

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
        // If the mouse is clicked, start dragging
        if (Input.GetButtonDown("Fire1"))
        {
            isDragging = true;
            lastPosition = Input.mousePosition;
        }
        // If the mouse is released, stop dragging, add force to the player
        if (Input.GetButtonUp("Fire1"))
        {
            isDragging = false;
            Debug.Log("Mouse moved " + dragDistance + " while button was down.");
            addForce(dragDistance); // Add force to the object based on the distance dragged
            dragDistance = 0;
        }
        // If the mouse is still down, calculate the distance for indicator
        if (isDragging)
        {   
            // TODO: check FuelCurrent
            var newPosition = Input.mousePosition;
            dragDistance = Vector2.Distance(lastPosition, newPosition);
        }
    }
}
