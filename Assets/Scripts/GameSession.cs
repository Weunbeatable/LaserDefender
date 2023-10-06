using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score;
    // Start is called before the first frame update
     void Awake()
    {
        setUpSingleton();
    }

    private void setUpSingleton()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length; //We want to account for the number of game sessions currently active, if the number is greater than one, destroy that session. otherwise load that session into the next scene
        if(numberOfGameSessions > 1)
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
        return score;
    }
    public void addToScore(int scoreValue)
    {
        score += scoreValue;
    
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }


}
