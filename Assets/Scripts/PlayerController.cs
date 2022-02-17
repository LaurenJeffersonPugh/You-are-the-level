using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private LayerMask walls;
    [SerializeField] private AudioSource bumpWall;
    [SerializeField] private AudioSource dieSpike;
    [SerializeField] private AudioSource dieElectric;


    private float distanceToWall = 0.1f;
    private float distanceToGround = 0.1f;
    private static int deaths = 0;
    private float playerSpeed = 4;

    private Rigidbody2D body;
    private float width;
    private float height;
    private bool grounded;
    private RotateWorld world;
    private Vector2 startingLocation;
    void Start()
    {
        world = GetComponent<RotateWorld>();
        startingLocation = transform.position;
        body = GetComponent<Rigidbody2D>();
        width = GetComponent<BoxCollider2D>().size.x/2;
        height = GetComponent<BoxCollider2D>().size.y/2 - 0.001f;

        //bumpWall = GetComponent<AudioSource>();
        /*if (startPos.x != 0 && startPos.y != 0)
        {
            transform.position = startPos;
        }
        */
    }
    
    void Update()
    {
        bool old_grounded = grounded;
        grounded = CheckGrounded();
        if (old_grounded && !grounded)
            body.velocity = Vector2.zero;
        CheckWallHit();
        if (grounded)
        {
            body.velocity = (Vector3)Vector3Int.RoundToInt(transform.right) * playerSpeed * transform.localScale.x;
        }
        
    }

    private void CheckWallHit()
    {
        
        bool hitWall = (Physics2D.Raycast(transform.position + transform.right * transform.localScale.x * width, transform.right * transform.localScale.x, distanceToWall, walls).rigidbody != null);


        Debug.DrawRay(transform.position, transform.right * transform.localScale.x * width * (1 + distanceToWall), hitWall ? Color.green : Color.red);
        if (hitWall)
        {
            bumpWall.Play();
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            body.velocity = Mathf.Abs(transform.right.x) > 0 ? new Vector2(0, body.velocity.y) : new Vector2(body.velocity.x, 0);
        }
    }

    private bool CheckGrounded()
    {
        Vector2[] characterLowestPoints = { transform.position + height * -transform.up + width * transform.right, transform.position + height * -transform.up - width * transform.right };
        Debug.DrawRay(characterLowestPoints[0], transform.up * -distanceToGround, Physics2D.Raycast(characterLowestPoints[0], -transform.up, distanceToGround, walls).rigidbody != null ? Color.green : Color.red);
        Debug.DrawRay(characterLowestPoints[1], transform.up * -distanceToGround, Physics2D.Raycast(characterLowestPoints[1], -transform.up, distanceToGround, walls).rigidbody != null ? Color.green : Color.red);
        return Physics2D.Raycast(characterLowestPoints[0], -transform.up, distanceToGround, walls).rigidbody != null ||
                   Physics2D.Raycast(characterLowestPoints[1], -transform.up, distanceToGround, walls).rigidbody != null;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            dieSpike.Play();
            Death();
        }

        else if (other.tag == "Electric")
        {
            dieElectric.Play();
            Death();
        }
    }

    private void Death()
    {
        deaths++;
        world.Reset();
        transform.position = startingLocation;
    }
}
