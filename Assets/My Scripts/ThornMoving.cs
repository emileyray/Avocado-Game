using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornMoving : MonoBehaviour
{
    public Transform pos1, pos2;
    public Transform startPos;

    float totalTime = 0f;
    float step = 0f;
    public float speed;

    Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
    }

    // Update is called once per frame
    // The throrn cahnges its position from one point to another
    void Update()
    {
        float y1 = pos1.position.y;
        float y2 = pos2.position.y;
        float distance = System.Math.Abs(y1 - y2);
        float a = 2 * distance;

        totalTime += Time.deltaTime;

        if (step <= distance / 2)
        {
            step += distance / 5;
        }
        else
        {
            step -= distance / 5;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, step * speed);

        if (transform.position.y == pos1.position.y)
        {
            nextPos = pos2.position;
            step = 0f;
        }
        if (transform.position.y == pos2.position.y)
        {
            nextPos = pos1.position;
            step = 0f;
        }
    }
}
