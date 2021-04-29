using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI text;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;

    }

    // Updating the score value
    public void ChangeScore(int crystalValue)
    {
        score += crystalValue;
        text.text = "x" + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
