using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [HideInInspector]
    public GameObject player1, player2;
    public new Camera camera;
    Vector3 cameraPosition;
    private Vector3 currentVelocity = Vector3.zero;
    public float MovementSmoothingValue = .01f;
    public BoxCollider2D leftWall, rightWall;
    Vector3 leftSideOfScreenPosition, rightSideOfScreenPosition;
    // Start is called before the first frame update
    void Start() { 
    
        cameraPosition.y = 4.02f;
        cameraPosition.z = -19.7f;
    }

    // Update is called once per frame
    void Update()
    {
        if(player1.transform.position.x >= player2.transform.position.x)
        {
            cameraPosition.x = (player1.transform.position.x - ((player1.transform.position.x - player2.transform.position.x) / 2));
        }
        else
        {
            cameraPosition.x = (player2.transform.position.x - ((player2.transform.position.x - player1.transform.position.x) / 2));
        }

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, cameraPosition, ref currentVelocity, MovementSmoothingValue * Time.fixedDeltaTime); //* Time.fixedDeltaTime
        //leftWall.offset = camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
       // rightWall.offset = camera.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f));
        
    }
}
