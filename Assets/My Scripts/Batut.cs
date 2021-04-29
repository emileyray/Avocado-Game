using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batut : MonoBehaviour
{
    public AvocadoMovement movement;
    public AvocadoController controller;
    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            movement.SetOnThorn(true);
            StartCoroutine(ApplyForce(other));
        }
    }

    IEnumerator ApplyForce(Collision2D other)
    {
        velocity = other.gameObject.GetComponent<Rigidbody2D>().velocity;
        velocity.y *= -2.75f;
        other.gameObject.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
    }
}
