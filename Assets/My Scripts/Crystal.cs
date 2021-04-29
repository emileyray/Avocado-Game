using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public int crystalValue = 1;
    public ScoreManager SM;
    bool hasGiven = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player touches the coinf, it gets removed
        if (other.gameObject.CompareTag("Player"))
        {
            if (!hasGiven)
            {
                ScoreManager.instance.ChangeScore(crystalValue);
                hasGiven = true;
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
