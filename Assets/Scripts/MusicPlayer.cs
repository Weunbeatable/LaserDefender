using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private int Length;

   void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {

        if (FindObjectsOfType(GetType()).Length > 1) 
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // basis of the singleton pattern.
}
