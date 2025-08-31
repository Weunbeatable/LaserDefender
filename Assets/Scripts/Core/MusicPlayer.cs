using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD.Core
{
    public class MusicPlayer : MonoBehaviour
    {
        private int _Length;

        void Awake()
        {
            Set_Up_Singleton();
        }

        private void Set_Up_Singleton()
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
}
