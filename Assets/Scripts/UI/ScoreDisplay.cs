using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI ScoreText;
    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        ScoreText = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = gameSession.getScore().ToString();
        Debug.Log(ScoreText);
    }
}
