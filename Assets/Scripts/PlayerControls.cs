using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Threading;
using System;

public class PlayerControls : MonoBehaviour
{
    public Animator animator;

    private Rigidbody2D rigidbody2D;
    public float playerSpeed;
    public float playerJumpForce;
    private float playerHalfWidth;
    private float playerHalfHeight;
    public Transform playerCenterOfMass;

    private Camera camera;

    private Touch theTouch;

    private LineRenderer lineRenderer;

    public GameObject playerAimPrefab;
    private GameObject playerAim;

    private bool isShooting;

    private bool isLanded;
    public LayerMask isPlatform;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        rigidbody2D.centerOfMass = new Vector2(playerCenterOfMass.localPosition.x, playerCenterOfMass.localPosition.y);

        camera = Camera.main;
    }

    void FixedUpdate()
    {
        isLanded = Physics2D.OverlapCircle(playerCenterOfMass.position, 0.5f, isPlatform);
        animator.SetBool("IsLanded", isLanded);
        animator.SetFloat("PlayerVelocity", Mathf.Abs(rigidbody2D.velocity.x));

        MovePlayer();
        MovePlayerPCTest();
    }

    private void MovePlayer()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Stationary)
            {
                if (camera.ScreenToWorldPoint(theTouch.position).x > transform.position.x + playerHalfWidth && !isShooting)
                {
                    MovePlayerRight();
                }

                if (camera.ScreenToWorldPoint(theTouch.position).x < transform.position.x - playerHalfWidth && !isShooting)
                {
                    MovePlayerLeft();
                }

                if (camera.ScreenToWorldPoint(theTouch.position).x <= transform.position.x + playerHalfWidth &&
                    camera.ScreenToWorldPoint(theTouch.position).x >= transform.position.x - playerHalfWidth &&
                    camera.ScreenToWorldPoint(theTouch.position).y <= transform.position.y + playerHalfHeight &&
                    camera.ScreenToWorldPoint(theTouch.position).y >= transform.position.y - playerHalfHeight && !playerAim)
                {
                    CreatePlayerAim();
                }
            }

            if (theTouch.phase == TouchPhase.Moved && isShooting)
            {
                playerAim.transform.Translate(Time.fixedDeltaTime * 3, 0, 0);
            }

            if (theTouch.phase == TouchPhase.Ended && isShooting)
            {
                Destroy(playerAim);
                isShooting = false;
            }
        }

    }

    private void MovePlayerPCTest()
    {
        if (Input.GetKey(KeyCode.A))
        {
            MovePlayerLeft();
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                MovePlayerRight();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) && isLanded)
                {
                    JumpPlayer();
                }
            }
        }

    }

    private void MovePlayerLeft()
    {
        transform.rotation = new Quaternion(0, 180f, 0, 0);
        rigidbody2D.velocity = new Vector2(-playerSpeed, 0);
    }

    private void MovePlayerRight()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        rigidbody2D.velocity = new Vector2(playerSpeed, 0);
    }

    private void JumpPlayer()
    {
        rigidbody2D.AddForce(Vector2.up * playerJumpForce, ForceMode2D.Impulse);
    }

    private void CreatePlayerAim()
    {
        playerAim = Instantiate(playerAimPrefab, transform.position, Quaternion.identity);
        isShooting = true;
    }
}
