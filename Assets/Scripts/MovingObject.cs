using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Vector2[] nodes;
    [SerializeField] private float speed;

    private Vector2 direction;
    private int targetIndex;
    private Rigidbody2D body;
    void Start()
    {
        targetIndex = 0;
        setDirection();
        body = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        body.velocity = direction * speed;
        if (PastTargetNode())
        {
            targetIndex += 1;
            targetIndex %= nodes.Length;
            setDirection();
        }
    }

    void setDirection()
    {
        direction = Mathf.Abs(transform.position.x - nodes[targetIndex].x)  > Mathf.Abs(transform.position.y - nodes[targetIndex].y) ? 
            new Vector2(nodes[targetIndex].x > transform.position.x ? 1 : -1, 0) :
            new Vector2(0, nodes[targetIndex].y > transform.position.y ? 1 : -1);
    }

    bool PastTargetNode()
    {
        if (Mathf.Abs(direction.x) > 0)
        {
            if (direction.x > 0)
                return transform.position.x > nodes[targetIndex].x;
            return transform.position.x < nodes[targetIndex].x;
        }
        if (direction.y > 0)
            return transform.position.y > nodes[targetIndex].y;
        return transform.position.y < nodes[targetIndex].y;
    }
}
