using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100; // damage value 
    GameSession SessionHandler;

    private void Start()
    {
        SessionHandler = FindObjectOfType<GameSession>();
    }
    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
