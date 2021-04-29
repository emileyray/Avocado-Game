using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightEye : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D LeftEye;
    public Transform Avocado;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.SetRotation(LeftEye.rotation);
        transform.position = new Vector3(Avocado.position.x + 0.156f, Avocado.position.y + 0.303f, 0f);
    }
}
