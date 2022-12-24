using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    [SerializeField] float lagAmount;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 

        // Follow the player TODO: (Considering to add a lag effect to this, so that the camera is dragged behind the player and snaps)
        transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(player.position.x, player.position.y, transform.position.z), 
            lagAmount * Time.deltaTime);

        //Edit lerps the camera position onto the player, which is controlled by the variable: lagAmount
    }
}
