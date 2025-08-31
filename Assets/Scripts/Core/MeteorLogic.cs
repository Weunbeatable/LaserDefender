using LD.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD.Core
{

    public class MeteorLogic : MonoBehaviour, IImpact
    {
        public static event Action<bool, Vector3> OnMeteorsDestruction;

        [SerializeField] Sprite[] _meteorSprites;
        [SerializeField] AudioSource _meteor_audio_Source;
        [SerializeField] ParticleSystem _meteorParticles;

        int _spawnSize;
        int _meteorLevel;
        SpriteRenderer _meteorRenderer;
        Health _Health;
        int _startValue = 0;
        bool is_Survival_Type;
        float _spinValue;
        Vector3 lastKnownPos;

        // Start is called before the first frame update
        void Start()
        {
            _meteorRenderer = GetComponent<SpriteRenderer>();
            _meteor_audio_Source = GetComponent<AudioSource>();
            _meteorParticles = GetComponent<ParticleSystem>();
            _Health = GetComponent<Health>();

            is_Survival_Type = MeteorSpawner.Instance.Get_Meteor_Type();
            initialSpawnSize(MeteorSpawner.Instance.GetSpawnSize() - 1); // decrement one so it doesn't risk going out of array bounds
            Set_Meteor_level(MeteorSpawner.Instance.GetMeteorLevel());
            Determine_MeteorToSpawn();
            _meteorSprites = new Sprite[MeteorSpawner.Instance.GetMeteorLevel()]; // setup size of sprite array to use
            int _countDown = _spawnSize;
            _countDown = _SetupMeteors(_countDown);
            _UpdateHealth();
            _spinValue = UnityEngine.Random.Range(-45f, -120f);
            // once initial image has been set, need to determine how many more meteors to make and adjust the circle collider. 


        }

        private void Update()
        {
            gameObject.transform.Rotate(transform.forward * _spinValue * Time.deltaTime); // lil bit of visual flair
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_Health.GetNormalizedHealth() <= 0 && _startValue + 1 < _meteorSprites.Length)
            {
                _startValue++;
                _meteorRenderer.sprite = _meteorSprites[_startValue];
                _Update_Smaller_Meteor_Health();
                //TODO: update collider size
            }
            else if (_Health.GetNormalizedHealth() <= 0 && _startValue + 1 >= _meteorSprites.Length)
            {
                // add drop logic. 
                lastKnownPos = transform.position;
                OnMeteorsDestruction?.Invoke(is_Survival_Type, lastKnownPos);
                // drops can either be random or determined by the starting size of the meteor.
                Destroy(gameObject);
            }
            else
            {
                if (collision.TryGetComponent<DamageDealer>(out DamageDealer damageDealer))
                {
                    _Health.DealDamage(damageDealer.GetDamage());
                    damageDealer.Hit();
                    PlayImpactEffect();
                }
            }
        }

        private void Determine_MeteorToSpawn()
        {
            if (MeteorSpawner.Instance.Get_Meteor_Type() == true)
            {
                _meteorRenderer.sprite = MeteorSpawner.Instance.Get_Survival_Meteors()[_spawnSize]; // set the inital image of the sprite. 
            }
            else
            {
                _meteorRenderer.sprite = MeteorSpawner.Instance.Get_Utility_Meteors()[_spawnSize]; // set the inital image of the sprite. 
            }
        }

        private int _SetupMeteors(int _countDown)
        {
            if (MeteorSpawner.Instance.Get_Meteor_Type() == true) // setup survival meteors
            {
                for (int i = 0; i < _meteorSprites.Length; i++)// constrct sprite array 
                {
                    _meteorSprites[i] = MeteorSpawner.Instance.Get_Survival_Meteors()[_countDown];
                    _countDown--;
                }
            }
            else
            {
                for (int i = 0; i < _meteorSprites.Length; i++)// constrct sprite array 
                {
                    _meteorSprites[i] = MeteorSpawner.Instance.Get_Utility_Meteors()[_countDown];
                    _countDown--;
                }
            }

            return _countDown;
        }

        private void _UpdateHealth()
        {
            int setHealth;
            if (MeteorSpawner.Instance.Get_Meteor_Dictionary_Values().TryGetValue(_meteorLevel, out setHealth))
            {
                _Health.SetMaxHealth(setHealth);
            }
        }

        private void _Update_Smaller_Meteor_Health()
        {
            int setHealth;
            if (MeteorSpawner.Instance.Get_Meteor_Dictionary_Values().TryGetValue(_meteorLevel, out setHealth))
            {
                _Health.SetMaxHealth((int)(_Health.getMaxHealth() * 0.75f));
                _Health.SetHealth(_Health.getMaxHealth());
            }
        }

        public void initialSpawnSize(int _setSize)
        {
            _spawnSize = _setSize;
            // Logic to setup meteor and circle collider when spawned in. 
        }

        public void Set_Meteor_level(int _Meteor_level)
        {
            _meteorLevel = _Meteor_level;
        }

        public void PlayImpactEffect()
        {
            float _randomPitch = UnityEngine.Random.Range(0.75f, 1.25f);
            _meteor_audio_Source.pitch = _randomPitch;
            _meteor_audio_Source.Play();
            _meteorParticles.Play();
        }
    }
}
