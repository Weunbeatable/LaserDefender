using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LD.Core;

public class Enemy : MonoBehaviour, IImpact
{
    [Header("Enemy related stats")]
    [SerializeField] Health _enemyHealth;

    float _shotCounter;
    float _impact_Effect_Timer;
    [SerializeField] int scoreValue = 150;
    [SerializeField] float _min_Time_Between_Shots = 0.2f;
    [SerializeField] float _max_Time_Between_Shots = 3f;
    [SerializeField] float _deathTimer = 1f;
    [SerializeField] float _EnemyProjectileSpeed = -12f;
    [SerializeField] float _impact_flash_duration = 0.3f;

    [Header("Aduio and Visual objects")]
    [SerializeField] GameObject _EnemyLaser;
    [SerializeField] GameObject _ExplosionVFX;
    [SerializeField] Material _impactMat;
    Material _originalMat;


    [Header("Audio")]
    [SerializeField] public AudioClip enemy_Laser_SFX;
    [SerializeField] public AudioClip enemy_Death_SFX;
    [SerializeField] [Range(0, 1)] public float enemy_Laser_SFX_Volume = 0.25f;
    [SerializeField] [Range(0, 1)] public float enemy_Explosion_SFX_Volume = 0.75f;

    Coroutine _DestroyedShip;
    void Start()
    {
        _enemyHealth = GetComponent<Health>();
        // Cache the originial material for the ship 
        if(TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            _originalMat = spriteRenderer.material;
        }
        _shotCounter = UnityEngine.Random.Range(_min_Time_Between_Shots, _max_Time_Between_Shots);
        _enemyHealth.onDie += _enemyHealth_onDie;
    }

    private void OnDestroy()
    {
        _enemyHealth.onDie -= _enemyHealth_onDie;
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }
    private void CountDownAndShoot()
    {
        _shotCounter -= Time.deltaTime;// framerate independent, every frame counter goes down
        if(_shotCounter <= 0f)
        {
            Fire();
            _shotCounter = UnityEngine.Random.Range(_min_Time_Between_Shots, _max_Time_Between_Shots);
        }
    }

    private void Fire()
    {
        GameObject _Laser = Instantiate(_EnemyLaser, transform.position, Quaternion.identity);  // quaternion.identity means just use rotation you have don't change anything.
        _Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, _EnemyProjectileSpeed); // apply some velocity to this projectile
        AudioSource.PlayClipAtPoint(enemy_Laser_SFX, Camera.main.transform.position, enemy_Laser_SFX_Volume);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer _damageDealer = other.gameObject.GetComponent<DamageDealer>(); // calling the damage dealer class when it damages the other object.
        if (!_damageDealer) { return; }
        ProcessHit(_damageDealer);
    }
 
    private void ProcessHit(DamageDealer _damageDealer)
    {
        HandleDamgeProcess(_damageDealer);
    }
    private void _enemyHealth_onDie()
    {
        Die();
    }
    private void HandleDamgeProcess(DamageDealer _damageDealer)
    {
        _enemyHealth.DealDamage(_damageDealer.GetDamage()); //subtract some health
        PlayImpactEffect();
        CinemachineShake.Instance.ShakeCamera(2, 0.1f);
        _damageDealer.Hit();
        
    }

    private void Die()
    {
        if(TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider))
        {
            // turn of the collider to destroyed ships to avoid cheesing. 
            circleCollider.enabled = false;
        }
        CinemachineShake.Instance.ShakeCamera(6, 0.1f);
        //TODO add death effects
        Destroy(gameObject, _deathTimer);
    }

    private void _Play_Damage_Effects()
    {
        Instantiate(_ExplosionVFX, transform.position, Quaternion.identity); // Instantiate the explosion game object at the enemy ship prefab instead of near the origin or where it was first made
        AudioSource.PlayClipAtPoint(enemy_Death_SFX, Camera.main.transform.position, enemy_Explosion_SFX_Volume);
    }

    public void PlayImpactEffect()
    {
        FindObjectOfType<GameSession>().Add_To_Score(scoreValue);
        StartCoroutine(_playFlash());
        _Play_Damage_Effects();
        this.gameObject.GetComponent<SpriteRenderer>().material = _impactMat;
    }

    private IEnumerator _playFlash()
    {
        while (true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().material = _originalMat;

            yield return new WaitForSeconds(_impact_flash_duration);
        }
    }
    //TODO 
    // Add some way to handle visuals so that when health is below a certain value, play smoke damage effect. 
    // Grab health value from health script. 
}
