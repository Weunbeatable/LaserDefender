using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{
    //config
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f; // adding some boundary padding for our scene. 
    [SerializeField] int health = 200;
    [SerializeField] GameObject ExplosionVFX;

    [Header("Projectile")]
    [SerializeField] public GameObject laser;
    [SerializeField] float ProjectileSpeed = 10f;
    [SerializeField] float ProjectileFiringPeriod = 1f;

    [Header("Audio")]
    [SerializeField] [Range(0, 1)] public float laserSound = 0.25f;
    [SerializeField] public AudioClip playerLaser;
    [SerializeField] [Range(0, 1)] public float deathSound = 1f;
    [SerializeField] public AudioClip playerDeathSound;

    Coroutine Firingcoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax; // camera variable initalization

   
    void Start()
    {
        SetUpMoveBoundaries();
        StartCoroutine(TestCoroutine());
    }

   
 
    void Update()
    {
        Move();
        fire();
    }

    

    private void fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Firingcoroutine = StartCoroutine(FireContinuously());
           
        }
            if (Input.GetButtonUp("Fire1"))
            {
            // StopAllCoroutines() good for stopping all the routines in a given script. there is a cleaner way however.
            StopCoroutine(Firingcoroutine);
            }
        }
    

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject Laser = Instantiate(laser, transform.position, Quaternion.identity);  // quaternion.identity means just use rotation you have don't change anything.
            Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ProjectileSpeed); // apply some velocity to this projectile
            AudioSource.PlayClipAtPoint(playerLaser, Camera.main.transform.position, laserSound); // use camera.main for displaying sound for better 3d audio management.
            yield return new WaitForSeconds(ProjectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed; // another way to get our keybindings
        var newXPos = Mathf.Clamp( transform.position.x + deltaX, xMin, xMax)   ; // position is current position + change

        var deltaY = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;
        var newYPos = Mathf.Clamp( transform.position.y + deltaY, yMin, yMax);
        // camera boundary constraints 

        transform.position = new Vector2(newXPos, newYPos);
        

        // Time.deltatime will allow for our game to be framerate independent meaning it will behave the same on fast and slow pc's

    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
        //camera boundary setup
    }

    private IEnumerator TestCoroutine()
    {
        Debug.Log("This is my test routine");
        yield return new WaitForSeconds(3); // an example of how to add a wait
        Debug.Log("I'm so surprised this turned out so well");

    }
    /* 
     * Storing this method for a future powerup
     *  GameObject Laser = Instantiate(laser, transform.position, Quaternion.identity);  // quaternion.identity means just use rotation you have don't change anything.
            yield return new WaitForSeconds(1);
            Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ProjectileSpeed); // apply some velocity to this projectile
     */



    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>(); // calling the damage dealer class when it damages the other object.
        if (!damageDealer) { return; } // if there is no damage dealer, return null, just end the proecss further down the chain and leave the method.
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
        Instantiate(ExplosionVFX, transform.position, Quaternion.identity); // Instantiate the explosion game object at the enemy ship prefab instead of near the origin or where it was first made
        AudioSource.PlayClipAtPoint(playerDeathSound, Camera.main.transform.position, deathSound);
        Destroy(gameObject);
        FindObjectOfType<Level>().LoadGameOver();
        
    }

    public int GetHealth()
    {
        return (health);
    }
}
