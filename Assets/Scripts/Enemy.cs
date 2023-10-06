using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy related stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;


     float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;

    [SerializeField] GameObject EnemyLaser;
    [SerializeField] float EnemyProjectileSpeed = -12f;
    [SerializeField] GameObject ExplosionVFX;
    [SerializeField] float deathTimer = 1f;

    [Header("Audio")]
    [SerializeField] public AudioClip enemyLaserSFX;
    [SerializeField] public AudioClip enemyDeathSFX;
    [SerializeField] [Range(0, 1)] public float enemyLaserSFXVolume = 0.25f;
    [SerializeField] [Range(0, 1)] public float enemyExplosionSFXVolume = 0.75f;

    Coroutine DestroyedShip;
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }
    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;// framerate independent, every frame counter goes down
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject Laser = Instantiate(EnemyLaser, transform.position, Quaternion.identity);  // quaternion.identity means just use rotation you have don't change anything.
        Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, EnemyProjectileSpeed); // apply some velocity to this projectile
        AudioSource.PlayClipAtPoint(enemyLaserSFX, Camera.main.transform.position, enemyLaserSFXVolume);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>(); // calling the damage dealer class when it damages the other object.
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }
 
    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage(); //subtract some health
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        FindObjectOfType<GameSession>().addToScore(scoreValue);
        Instantiate(ExplosionVFX, transform.position, Quaternion.identity); // Instantiate the explosion game object at the enemy ship prefab instead of near the origin or where it was first made
        AudioSource.PlayClipAtPoint(enemyDeathSFX, Camera.main.transform.position, enemyExplosionSFXVolume);
        Destroy(gameObject, deathTimer);
    }
}
