using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LD.Core;

namespace LD.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        TextMeshProUGUI _ScoreText;
        GameSession _gameSession;
        // Start is called before the first frame update
        void Start()
        {
            _ScoreText = GetComponent<TextMeshProUGUI>();
            _gameSession = FindObjectOfType<GameSession>();
            _gameSession.ResetScore();

        }

        // Update is called once per frame
        void Update()
        {
            _ScoreText.text = _gameSession.getScore().ToString();

        }
    }
}
