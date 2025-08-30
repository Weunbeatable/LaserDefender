using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD.Core
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] int _damage = 100; // damage value 
        GameSession _SessionHandler;

        private void Start()
        {
            _SessionHandler = FindObjectOfType<GameSession>();
        }
        public int GetDamage()
        {
            return _damage;
        }

        public void Hit()
        {
            Destroy(gameObject);
        }
    }
}

