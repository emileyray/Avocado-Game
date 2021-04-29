using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    Vector3 direction = Vector3.zero;
    public Transform Avocado;
    public AvocadoController Controller;

    float deltaY = 0;
    float deltaX = 0;

    float X;
    float Y;

    // Start is called before the first frame update
    void Start()
    {
        X = -Avocado.position.x + transform.position.x;
        Y = -Avocado.position.y + transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject nextCrystal;
        float distance = 1000000f;

        foreach (GameObject currentCrystal in GameObject.FindGameObjectsWithTag("Crystal"))
        {
            float deltaX0 = currentCrystal.transform.position.x - transform.position.x;
            float deltaY0 = currentCrystal.transform.position.y - transform.position.y;

            if ((float)System.Math.Pow(deltaX0*deltaX0 + deltaY0*deltaY0, 0.5) < distance)
            {
                distance = (float)System.Math.Pow(deltaX0 * deltaX0 + deltaY0 * deltaY0, 0.5);
                deltaX = currentCrystal.transform.position.x - transform.position.x;
                deltaY = currentCrystal.transform.position.y - transform.position.y;

            }
        }


        float tan = deltaY / deltaX;
        float degree;
        if (deltaX >= 0) degree = (float)System.Math.Atan(tan) * 180f / 3.14159f;
        else degree = 180f + (float)System.Math.Atan(tan) * 180f / 3.14159f;

        if (distance > 5f)
        {
            if (Controller.isFacingRight()) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else
        {
            if (!Controller.isFacingRight()) degree -= 180f;
            transform.rotation = Quaternion.Euler(0f, 0f, degree);
        }

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            if (Controller.isFacingRight()) transform.position = Avocado.position + new Vector3(X - 0.017f, Y, 0);
            else transform.position = Avocado.position + new Vector3(X - 0.15f, Y, 0);
        }
        else
        {
            if (Controller.isFacingRight()) transform.position = Avocado.position + new Vector3(X - 0.017f, Y, 0);
            else transform.position = Avocado.position + new Vector3(X - 0.16f, Y, 0);
        }
    }
}
