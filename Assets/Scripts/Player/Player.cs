using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LD.Core;

public class Player : MonoBehaviour, IImpact

{
    //config
    [Header("Player Stats")]
    [SerializeField] Health _playerHealth;
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _padding = 1f; // adding some boundary padding for our scene. 
    [SerializeField] float _impact_flash_duration = 0.3f;


    [Header("Visual objects")]
    [SerializeField] GameObject _ExplosionVFX;
    [SerializeField] Material _impactMat;
    [SerializeField] Sprite[] _shipDamage;
    [SerializeField] SpriteRenderer _damageRenderer;

    [Header("Projectile")]
    [SerializeField] public GameObject laser;
    [SerializeField] float _ProjectileSpeed = 10f;
    [SerializeField] float _ProjectileFiringPeriod = .4f;

    [Header("Audio")]
    [SerializeField] [Range(0, 1)] public float laserSound = 0.25f;
    [SerializeField] public AudioClip playerLaser;
    [SerializeField] [Range(0, 1)] public float deathSound = 1f;
    [SerializeField] public AudioClip player_Death_Sound;

    Coroutine Firingcoroutine;

    Material _originalMat;

    float _xMin;
    float _xMax;
    float _yMin;
    float _yMax; // camera variable initalization

   
    void Start()
    {
        _playerHealth = GetComponent<Health>();
        // Cache the originial material for the ship 
        CacheOriginalMaterial();
        _Set_Up_Move_Boundaries();
        StartCoroutine(_TestCoroutine());
        _playerHealth.onDie += _playerHealth_onDie; // subscribe to death event from health script
    }



    private void OnDestroy()
    {
        _playerHealth.onDie -= _playerHealth_onDie;
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
    #region Powerups
    /* 
     * Storing this method for a future powerup
     *  GameObject Laser = Instantiate(laser, transform.position, Quaternion.identity);  // quaternion.identity means just use rotation you have don't change anything.
            yield return new WaitForSeconds(1);
            Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ProjectileSpeed); // apply some velocity to this projectile
     */
    #endregion



    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer _damageDealer = other.gameObject.GetComponent<DamageDealer>(); // calling the damage dealer class when it damages the other object.
        if (!_damageDealer) { return; } // if there is no damage dealer, return null, just end the proecss further down the chain and leave the method.
        ProcessHit(_damageDealer);
    }

    private void ProcessHit(DamageDealer _damageDealer)
    {
        CinemachineShake.Instance.ShakeCamera(7, 0.1f);
        _playerHealth.DealDamage(_damageDealer.GetDamage()); //subtract some health
        PlayImpactEffect();
        _damageDealer.Hit();
    }
    private void _playerHealth_onDie()
    {
      
        Die();
    }
    private void Die()
    {
        StartCoroutine(_finalExplosion());
        Instantiate(_ExplosionVFX, transform.position, Quaternion.identity); // Instantiate the explosion game object at the enemy ship prefab instead of near the origin or where it was first made
        CinemachineShake.Instance.ShakeCamera(14f, 0.1f);
        AudioSource.PlayClipAtPoint(player_Death_Sound, Camera.main.transform.position, deathSound);
        Destroy(gameObject);
        FindObjectOfType<Level>().Load_Game_Over();
        
    }

    public void PlayImpactEffect()
    {
        playDamageSprite();
        StartCoroutine(_playFlash());
       // _damageRenderer.sprite = null; // remove the sprite that was previously there.
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

    private IEnumerator _finalExplosion()
    {
        int _counter = 0;
       
        while (_counter < _shipDamage.Length)
        {
            _damageRenderer.sprite = _shipDamage[_counter];
            Debug.Log("changing ship image to counter value " + _counter);
            _counter++;
            yield return new WaitForSeconds(1f);
        }
        
            Debug.Log("changing ship image to counter value " + _counter);
        yield return new WaitForSeconds(1f);
    }
    private void playDamageSprite()
    {
        int _damage_sprites_Index_Value;
        int[] _damage_sprites_Array = new int [_shipDamage.Length];

        if(_playerHealth.GetNormalizedHealth() == 1f)
        {
            _damageRenderer.sprite = null;
        }
        else if (_playerHealth.GetNormalizedHealth() >= 0.7 && _playerHealth.GetNormalizedHealth() <1f)
        {
             _damage_sprites_Index_Value = UnityEngine.Random.Range(0, _damage_sprites_Array[2]);
            _damageRenderer.sprite = _shipDamage[_damage_sprites_Index_Value];
        }
        else if (_playerHealth.GetNormalizedHealth() >= 0.5f && _playerHealth.GetNormalizedHealth() < 0.7f)
        {
            _damage_sprites_Index_Value = UnityEngine.Random.Range(_damage_sprites_Array[3], _damage_sprites_Array[5]);
            _damageRenderer.sprite = _shipDamage[_damage_sprites_Index_Value];
        }
        else if (_playerHealth.getCurrentHealth() > 0f && _playerHealth.GetNormalizedHealth() < 0.5f)
        {
            _damage_sprites_Index_Value = UnityEngine.Random.Range(_damage_sprites_Array[6], _shipDamage.Length - 1);
            _damageRenderer.sprite = _shipDamage[_damage_sprites_Index_Value];
        }
        else
        {
            Debug.Log("Start death routine.");
            StartCoroutine(_finalExplosion());
        }
    }
    private void CacheOriginalMaterial()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            _originalMat = spriteRenderer.material;
        }
    }

    public Health _Get_Player_Health() => _playerHealth;
}
