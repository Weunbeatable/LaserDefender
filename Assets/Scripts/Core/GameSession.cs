using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LD.Core;

namespace LD.Core
{
    public class GameSession : MonoBehaviour
    {
        int _score;
        int _highScore;
        // Start is called before the first frame update
        void Awake()
        {
            _setUpSingleton();
        }

        private void _setUpSingleton()
        {
            int _numberOfGameSessions = FindObjectsOfType<GameSession>().Length; //We want to account for the number of game sessions currently active, if the number is greater than one, destroy that session. otherwise load that session into the next scene
            if (_numberOfGameSessions > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public int getScore()
        {
            return _score;
        }
        public void Add_To_Score(int scoreValue)
        {
            _score += scoreValue;

        }
        public void SetHighScore()
        {
            if (_score > _highScore)
            {
                _highScore = _score;
            }
        }

        public void ResetScore()
        {
            _score = 0;
        }
        public int GetHighScore()
        {
            return _highScore;
        }
        public void ResetGame()
        {
            Destroy(gameObject);
        }


    }
}

