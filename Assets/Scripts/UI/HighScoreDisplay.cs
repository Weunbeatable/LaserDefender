using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LD.Core;
public class HighScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI _HighScoreText;
    void Start()
    {
        _HighScoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
       _HighScoreText.text = FindObjectOfType<GameSession>().GetHighScore().ToString();
    }
}
