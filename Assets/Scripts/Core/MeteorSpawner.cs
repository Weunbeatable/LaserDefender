using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD.Core
{
    public class MeteorSpawner : MonoBehaviour
    {
        [SerializeField] Sprite[] _utilityMeteors;
        [SerializeField] Sprite[] _survivalMeteors;
        [SerializeField] GameObject _utilityPrefab;
        [SerializeField] GameObject _survivalPrefab;
        [SerializeField] BoxCollider2D _meteor_Spawner_Collider;

        public int _spawnSize; // spawning size of meteors 

        public int _meteorLevels;

        private int[] _randomizerValue = new int[15];
        bool _is_Surivival_Meteor;

        [SerializeField] float _next_Meteor_Timer;
        // Cache bounds of collider for efficiency and speed
        Vector3 _lowerBound;
        Vector3 _upperBound;

        // Meteors won't be destroyed depending on hit count but on a set health for each stage of the meteor sprite, because
        // some lasers can live past one hit, so to prevent having to code this logic later or having unintended issues having a 
        // set health for the meteors seems easier. Also it allows me to implement the damage interface with ease. So meteors will 
        // instead have randomized size values. collider value for each stage will be collider size * 0.75
        // there will be a dictionary of set health for each stage of the meteors. 


        Dictionary<int, int> _Meteor_Damage_Lookup = new Dictionary<int, int>();

        // serialized Dictionary 
        [SerializeField]
        MeteorDamageValues _Meteor_Damage;

        public static MeteorSpawner Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null) // singleton since I only need one spawner. 
            {
                Debug.Log("Can only have one meteor spawner at a time!");
                Destroy(gameObject);
                return;
            }
            Instance = this;

        }

        void Start()
        {
            _next_Meteor_Timer = 2f;
            // add values to randomizer 
            for (int i = 0; i < _randomizerValue.Length; i++)
            {
                int _randomNumber = UnityEngine.Random.Range(30, 75);
                _randomizerValue[i] = _randomNumber;
            }

            _lowerBound = _meteor_Spawner_Collider.bounds.min;
            _upperBound = _meteor_Spawner_Collider.bounds.max;
            _Meteor_Damage_Lookup = _Meteor_Damage.Convert_To_Dictionary();
        }

        void Update()
        {
            _next_Meteor_Timer -= Time.deltaTime;
            if (_next_Meteor_Timer <= 0)
            {
                _Create_Meteor_Logic();
            }
        }

        public void _Create_Meteor_Logic()
        {
            // first create logic to choose wich meteor type as the type assigns the enums it will have. 
            // then choose the logic for randomizing the enums 
            // finally choose
            float _chosenValue = UnityEngine.Random.Range(0, _randomizerValue.Length);
            _chosenValue = _chosenValue % 2;

            float _randomX = UnityEngine.Random.Range(_lowerBound.x, _upperBound.x);
            float _randomY = _meteor_Spawner_Collider.transform.position.y;
            float _randomZ = _meteor_Spawner_Collider.transform.position.z;
            Vector3 _spawnValue = new Vector3(_randomX, _randomY, _randomZ);

            // spawn level 7-10
            //spawnSize
            // meteor levels 1-6
            //meteorLevels

            _spawnSize = UnityEngine.Random.Range(7, 10);
            _meteorLevels = UnityEngine.Random.Range(1, 6); // when passed must be n-1

            if (_chosenValue == 0)
            {
                _is_Surivival_Meteor = true;
                Instantiate(_survivalPrefab, _spawnValue, Quaternion.identity);

                // implement logic to spawn health related meteor
            }
            else
            {
                _is_Surivival_Meteor = false;
                Instantiate(_utilityPrefab, _spawnValue, Quaternion.identity);
                // shields
            }

            _next_Meteor_Timer = 5; //UnityEngine.Random.Range(5f, 20f);

        }

        // meteor info 
        public int GetSpawnSize() => _spawnSize;
        public int GetMeteorLevel() => _meteorLevels;

        public Sprite[] Get_Survival_Meteors() => _survivalMeteors;
        public Sprite[] Get_Utility_Meteors() => _utilityMeteors;

        public bool Get_Meteor_Type() => _is_Surivival_Meteor;

        public Dictionary<int, int> Get_Meteor_Dictionary_Values() => _Meteor_Damage_Lookup;

    }

}

[Serializable]
public class MeteorDamageValues
{
    [SerializeField]
    MeteorDictionaryFields[] _Meteor_Dictionary_Items;

    public Dictionary<int, int> Convert_To_Dictionary()
    {
        Dictionary<int, int> new_Meteor_Dictionary = new Dictionary<int, int>();

        foreach (var item in _Meteor_Dictionary_Items)
        {
            new_Meteor_Dictionary.Add(item.GetSize(), item.GetAssignedHealth());
        }
        return new_Meteor_Dictionary;
    }
}
[Serializable]
public class MeteorDictionaryFields
{
    [SerializeField]
    int _arraySize;
    [SerializeField]
    int _assignedHealth;

    public int GetSize() => _arraySize;
    public int GetAssignedHealth() => _assignedHealth;

}
