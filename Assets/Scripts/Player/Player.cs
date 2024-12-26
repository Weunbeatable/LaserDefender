using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LD.Core;

public class Player : MonoBehaviour

{
    //config
    [Header("Player")]
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _padding = 1f; // adding some boundary padding for our scene. 
    [SerializeField] int _health = 200;
    [SerializeField] GameObject _ExplosionVFX;

    [Header("Projectile")]
    [SerializeField] public GameObject laser;
    [SerializeField] float _ProjectileSpeed = 10f;
    [SerializeField] float _ProjectileFiringPeriod = 1f;

    [Header("Audio")]
    [SerializeField] [Range(0, 1)] public float laserSound = 0.25f;
    [SerializeField] public AudioClip playerLaser;
    [SerializeField] [Range(0, 1)] public float deathSound = 1f;
    [SerializeField] public AudioClip player_Death_Sound;

    Coroutine Firingcoroutine;

    float _xMin;
    float _xMax;
    float _yMin;
    float _yMax; // camera variable initalization

   
    void Start()
    {
        _Set_Up_Move_Boundaries();
        StartCoroutine(_TestCoroutine());
    }

   
 
    void Update()
    {
        _Move();
        _fire();
    }

    

    private void _fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Firingcoroutine = StartCoroutine(_FireContinuously());
           
        }
            if (Input.GetButtonUp("Fire1"))
            {
            // StopAllCoroutines() good for stopping all the routines in a given script. there is a cleaner way however.
            StopCoroutine(Firingcoroutine);
            }
        }
    

    private IEnumerator _FireContinuously()
    {
        while (true)
        {
            GameObject Laser = Instantiate(laser, transform.position, Quaternion.identity);  // quaternion.identity means just use rotation you have don't change anything.
            Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, _ProjectileSpeed); // apply some velocity to this projectile
            AudioSource.PlayClipAtPoint(playerLaser, Camera.main.transform.position, laserSound); // use camera.main for displaying sound for better 3d audio management.
            yield return new WaitForSeconds(_ProjectileFiringPeriod);
        }
    }

    private void _Move()
    {
        var deltaX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * _moveSpeed; // another way to get our keybindings
        var newXPos = Mathf.Clamp( transform.position.x + deltaX, _xMin, _xMax)   ; // position is current position + change

        var deltaY = Input.GetAxisRaw("Vertical") * Time.deltaTime * _moveSpeed;
        var newYPos = Mathf.Clamp( transform.position.y + deltaY, _yMin, _yMax);
        // camera boundary constraints 

        transform.position = new Vector2(newXPos, newYPos);
        

        // Time.deltatime will allow for our game to be framerate independent meaning it will behave the same on fast and slow pc's

    }

    private void _Set_Up_Move_Boundaries()
    {
        Camera gameCamera = Camera.main;
        _xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + _padding;
        _xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - _padding;
        _yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + _padding;
        _yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - _padding;
        //camera boundary setup
    }

    private IEnumerator _TestCoroutine()
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
        DamageDealer _damageDealer = other.gameObject.GetComponent<DamageDealer>(); // calling the damage dealer class when it damages the other object.
        if (!_damageDealer) { return; } // if there is no damage dealer, return null, just end the proecss further down the chain and leave the method.
        ProcessHit(_damageDealer);
    }

    private void ProcessHit(DamageDealer _damageDealer)
    {
        _health -= _damageDealer.GetDamage(); //subtract some health
        _damageDealer.Hit();
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(_ExplosionVFX, transform.position, Quaternion.identity); // Instantiate the explosion game object at the enemy ship prefab instead of near the origin or where it was first made
        AudioSource.PlayClipAtPoint(player_Death_Sound, Camera.main.transform.position, deathSound);
        Destroy(gameObject);
        FindObjectOfType<Level>().Load_Game_Over();
        
    }

    public int GetHealth()
    {
        return (_health);
    }
}
