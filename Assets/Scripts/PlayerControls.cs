using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Threading;
using System;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    public float playerSpeed;
    public float playerWidth;
    public Transform playerCenterOfMass;

    private Camera camera;

    private Touch theTouch;

    //test
    public Text directionText;
    private string directionString;
    //test

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerWidth = GetComponent<SpriteRenderer>().bounds.extents.x;

        rigidbody2D.centerOfMass = new Vector2(playerCenterOfMass.localPosition.x, playerCenterOfMass.localPosition.y);

        camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Stationary)
            {
                if (camera.ScreenToWorldPoint(theTouch.position).x > transform.position.x + playerWidth)
                {
                    MovePlayerRight();
                }
                else
                {
                    if (camera.ScreenToWorldPoint(theTouch.position).x < transform.position.x - playerWidth)
                    {
                        MovePlayerLeft();
                    }
                }
            }
        }
    }

    private void MovePlayerLeft()
    {
        transform.Translate(-Time.deltaTime * playerSpeed, 0, 0);
    }

    private void MovePlayerRight()
    {
        transform.Translate(Time.deltaTime * playerSpeed, 0, 0);
    }

    private void JumpPlayer()
    {
        rigidbody2D.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
    }
}
