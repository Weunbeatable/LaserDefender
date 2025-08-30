using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;


        [SerializeField] private int _health;
        private bool _isInVulnerable = false;
        private string generate;

        public event Action onTakeDamage, onDie;
        public event Action<bool> onFinishable;

        public bool isDead => _health == 0; // if isdead return the health value as 0. 
        void Start()
        {

            _health = _maxHealth;
        }

        public void setInVulnerable(bool isInvulnerable)
        {
            this._isInVulnerable = isInvulnerable;
        }
        public void DealDamage(int damageDealt)
        {
            //if (!isAlive) { return; }
            if (_health == 0)
            { return; }

            if (_isInVulnerable) { return; }

            {
                //onTakeDamage?.Invoke();

                _health = Mathf.Max(_health - damageDealt, 0); // making sure our health doesn't go negative, if it does set it to 0 otherwise whatever your damage value wass

                //  check for health state, if at any point health becomes severly low invoke a finishable method where it is possible for the enemy to be  finished. 
                // additionally this opens the opportunity for a "damaged" state where player or enemy will move in a hurt fashion. 
                // if awkward beheavior occurs where you can do finishers in air just switch logic to add another bool where finishers are only done grounded (Unless air finishers are introduced...).
                onFinishable?.Invoke(CriticalHealthPercentage());


                if (_health <= 0)
                {
                    onDie?.Invoke();
                }
                // Debug.Log(health);
            }


        }

        public void GenerateString(string generate)
        {
            this.generate = generate;
        }
        public bool CriticalHealthPercentage()
        {
            float remaining_Health_percentage = (float)_health / _maxHealth * 100; // Dont forget to cast to float otherwise returning float value will default to 0 since health and max health are ints (learned that hte hard way)
            bool bro_Your_Health_Is_LOW;
            // Debug.Log("remaining health % is " + remaining_Health_percentage);
            if (remaining_Health_percentage <= 20f)
            {
                bro_Your_Health_Is_LOW = true;
            }
            else
            {
                bro_Your_Health_Is_LOW = false;
            }

            return bro_Your_Health_Is_LOW;
        }
        public int getCurrentHealth() => _health;
        public void HealHealth(int healAmount)
        {
            if (_health == _maxHealth) { return; }
            Debug.Log("health heal amount value " + healAmount);
            float _percentage = healAmount / 100f;
            Debug.Log("health percentage " + _percentage);
            float _shieldsRestored = _maxHealth * _percentage;
            float _maxValue = _maxHealth;
            _health += (int)_shieldsRestored; //Mathf.Min((int)_shieldsRestored, (int)_maxValue);
            if (_health >= _maxValue) { _health = (int) _maxHealth; }
        }
        public int getMaxHealth() => _maxHealth;
        public float GetNormalizedHealth() => (float)_health / _maxHealth;

        public void SetMaxHealth(int _maxHealth)
        {
            this._maxHealth = _maxHealth;
        }

        public void SetHealth(int updatedHealthValue)
        {
            _health = updatedHealthValue;
        }

        public bool GetVulnerabilityStatus() => _isInVulnerable;
    }
}