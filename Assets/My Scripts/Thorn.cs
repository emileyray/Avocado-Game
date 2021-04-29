using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{

    public AvocadoMovement movement;
    public AvocadoController controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            movement.SetOnThorn(true);
            StartCoroutine(ApplyForce(other));
        }
    }

    IEnumerator ApplyForce(Collision2D other)
    {
        other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 5f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
    }
}
