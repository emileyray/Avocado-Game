using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteEye : MonoBehaviour {
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        GameObject nextCrystal;
        float distance = 1000000f;
        Vector3 direction = Vector3.zero;

        foreach(GameObject currentCrystal in GameObject.FindGameObjectsWithTag("Crystal"))
        {
            float currentDistance = Vector3.Distance(currentCrystal.transform.position, transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                direction = currentCrystal.transform.position;
            }

            rb.AddForce(direction*0.0001f, ForceMode2D.Impulse);
        }
    }
}
