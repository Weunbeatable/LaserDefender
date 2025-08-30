using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD.Core
{
    public class LaserShredder : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(collision.gameObject);
        }
    }
}