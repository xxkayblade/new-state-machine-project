using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float speed = 5f;
    public float distance = 5f;
    private Vector3 startPos;

    public MoveStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case MoveStates.UP:
                transform.position = new Vector3(transform.position.x, startPos.y + Mathf.PingPong(Time.time * speed, distance), transform.position.z);
                break;
            case MoveStates.DOWN:
                transform.position = new Vector3(transform.position.x, startPos.y - Mathf.PingPong(Time.time * speed, distance), transform.position.z);
                break;
            case MoveStates.LEFT:
                transform.position = new Vector3(startPos.x - Mathf.PingPong(Time.time * speed, distance), transform.position.y, transform.position.z);
                break;
            case MoveStates.RIGHT:
                transform.position = new Vector3(startPos.x + Mathf.PingPong(Time.time * speed, distance), transform.position.y, transform.position.x);
                break;
        }
    }

    public enum MoveStates
    {
        UP, 
        DOWN,
        LEFT,
        RIGHT
    }
}
