using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
/// <summary>
/// Different shield types can be added on later but for now they will be 
/// broken into three basic sections
///Shields that don't replenish
///Shields that auto replenish
///Shields that deflect damage
///Damage taken will affect shields and cause a shield visual to display.
///once shields are depleted they will be destroyed.
/// </summary>
/// 

namespace LD.Core
{
    public class Shields : MonoBehaviour
    {
        public event Action<bool> onShieldsBroken;

        [SerializeField] private int _maxShields = 50;
        [SerializeField] private int _shields;
        [SerializeField] private bool _canRecharge;
        [SerializeField] private float _rechargeCooldown; // will expose this to allow for debuffs that increase recharge delay
        [SerializeField] private float _rechargeRate = 0.5f;
        [SerializeField] private int _rechargeValue = 15;


        bool _shieldsEmpty;
        float _Material_X_Offset;
        float _Material_X_Modifier;
        private Vector2 _offsetValues = new Vector2(0f, 0f);
        private Material _m_Material;
        ParticleSystemRenderer _m_ParticleSystem;
        //ParticleSystem _sheildParticles;
        //private void Awake()
        //{
        //    _m_Material = GetComponent<Material>();
        //}
        void Start()
        {
            _m_ParticleSystem = GetComponent<ParticleSystemRenderer>();
            _shieldsEmpty = false;
            _Material_X_Offset = _offsetValues.x;
            //_rechargeCooldown = 5f;
            _shields = _maxShields;
            _m_Material = _m_ParticleSystem.material;
            _m_Material.SetVector("_ShieldOffset", _offsetValues);
            //_sheildParticles = GetComponent<ParticleSystem>();
        }


        void Update()
        {
            _shieldsEmpty = _shields <= 0 ? _shieldsEmpty = true : _shieldsEmpty = false; // check if there are any remaining shields

            _Material_X_Modifier = (float)_shields / _maxShields;

            _rechargeCooldown -= Time.deltaTime;

            _rechargeRate -= Time.deltaTime;
            if (Allowed_To_Recharge())
            {
                RechargeShields(_rechargeValue);
            }
        }

        public void RechargeShields(int _rechargeAmount)
        {
            if (_shields == _maxShields) { return; }
            _rechargeAmount = _rechargeValue;

            if (_rechargeRate <= 0)
            {
                _shields += _rechargeAmount;
                _Recharge_Shield_Visual_Update();
                _rechargeRate = 0.5f;
            }

        }

        public void HealShields(int healAmount)
        {
            if (_shields == _maxShields) { return; }
            Debug.Log("shield heal amount value " + _shields);
            float _percentage = healAmount / 100f;
            Debug.Log("shield percentage value " + _percentage);
            float _shieldsRestored = _maxShields * _percentage;
            float _maxValue = _maxShields;          
            _shields += (int)_shieldsRestored; // Mathf.Min((int)_shieldsRestored, (int)_maxValue);
            if (_shields >= _maxValue) { _shields = (int)_maxShields; }
            _Heal_Shield_Visual_Update(healAmount);

        }
        public void DealShieldDamage(int damageDealt)
        {
            if (_shields == 0) { return; }

            {
                _rechargeCooldown = 5f;
                _shields = Mathf.Max(_shields - damageDealt, 0); // making sure our health doesn't go negative, if it does set it to 0 otherwise whatever your damage value wass
                _Decharge_Shield_Visual_Update();
                Debug.Log("Shield damage is " + _shields);
                /*            if (_shields <= 0)
                            {
                                onShieldsBroken?.Invoke(_shieldsEmpty);
                            }*/
                // Debug.Log(health);
            }

        }
        private void _Recharge_Shield_Visual_Update()
        {
            if (_offsetValues.x <= 0) { return; }
            _offsetValues.x -= (float)_rechargeValue / _maxShields;
            _m_Material.SetVector("_ShieldOffset", _offsetValues);
            //Debug.Log("vector offset value is currently " + _offsetValues);
        }
        private void _Heal_Shield_Visual_Update(int _healAmount)
        {
            if (_offsetValues.x <= 0) { return; }
            _offsetValues.x -= (float)_healAmount / _maxShields;
            _m_Material.SetVector("_ShieldOffset", _offsetValues);
            Debug.Log("vector offset value is currently " + _offsetValues);
        }

        private void _Decharge_Shield_Visual_Update()
        {
            if (_offsetValues.x >= 1) { return; }
            _offsetValues.x = (float)(_maxShields - _shields) / _maxShields;
            _m_Material.SetVector("_ShieldOffset", _offsetValues);
            Debug.Log("lost " + _offsetValues.x);
        }
        private bool Allowed_To_Recharge()
        {
            return _canRecharge && _rechargeCooldown <= 0;
        }
        public int GetShields() => _shields;
        public int GetMaxShields() { return _maxShields; }

        public float GetNormalizedShields() => (float)_shields / _maxShields;
        public float GetCooldown() => _rechargeCooldown;
        public float Get_Recharge_Rate() => _rechargeRate;

        public bool Get_Shield_Status() => _shieldsEmpty;
    }
}