using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class RotateWorld : MonoBehaviour
{
    [SerializeField] private AudioSource turnSound;
    private Rigidbody2D body;
    private int targetAngle;
    private float degreePerSecond = 300;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private readonly Dictionary<int, Vector2> gravityValues = new Dictionary<int, Vector2>()
    {
        {0, Vector2.down * 9.81f}, {90, Vector2.right * 9.81f}, {180, Vector2.up * 9.81f}, {270, Vector2.right * -9.81f}
    };

    public void Reset()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        Physics2D.gravity = new Vector2(0, -9.81f);
        targetAngle = 0;
    }

    private void Update()
    {

        if(Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if(touch.position.x < Screen.width / 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    targetAngle -= 90;
                }
            }
            else if(touch.position.x > Screen.width / 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    targetAngle += 90;
                }
            }
        }

        if (Input.GetKeyDown("1"))
            targetAngle -= 90;

        if (Input.GetKeyDown("2"))
            targetAngle += 90;

        if (targetAngle >= 360)
            targetAngle -= 360;
        if (targetAngle < 0)
            targetAngle += 360;

        int reverseTargetAngle = targetAngle;
        if (reverseTargetAngle >= 180)
            reverseTargetAngle -= 360;

        float reverseCurrentAngle = transform.rotation.eulerAngles.z;
        if (reverseCurrentAngle >= 180)
            reverseCurrentAngle -= 360;


        float cwAngleDiff = Mathf.Abs(targetAngle - transform.rotation.eulerAngles.z);
        float ccwAngleDiff = Mathf.Abs(reverseTargetAngle - reverseCurrentAngle);

        float angleDiff = Mathf.Min(cwAngleDiff, ccwAngleDiff);
        if (!Mathf.Approximately(angleDiff, 0) )
        {
            body.velocity = Vector2.zero;
            Physics2D.gravity = Vector2.zero;
            if (Mathf.Abs(angleDiff) < Time.deltaTime * degreePerSecond)
            {
                Physics2D.gravity = gravityValues[targetAngle];
                transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                Camera.main.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                if (!turnSound.isPlaying)
                    turnSound.Play();
            }
            else
            {
                int direction;
                if (cwAngleDiff < ccwAngleDiff)
                    direction = targetAngle > transform.rotation.eulerAngles.z ? 1 : -1;
                else
                    direction = reverseTargetAngle > reverseCurrentAngle ? 1 : -1;
                Camera.main.transform.Rotate(0, 0, Time.deltaTime * degreePerSecond * direction);
                transform.Rotate(0, 0, Time.deltaTime * degreePerSecond * direction);
            }
        }
        else if (Physics2D.gravity == Vector2.zero)
        {
            Physics2D.gravity = gravityValues[targetAngle];
            if (!turnSound.isPlaying)
                turnSound.Play();
        }
    }
}
