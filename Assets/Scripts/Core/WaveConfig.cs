using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LD.Core
{
    [CreateAssetMenu(menuName = "Enemy Wave Config")]
    public class WaveConfig : ScriptableObject
    {
        [SerializeField] GameObject enemyPrefab;
        [SerializeField] GameObject pathPrefab;
        [SerializeField] float timeBetweenSpawns = 0.5f;
        [SerializeField] float spawnRandomFactor = 0.3f;
        [SerializeField] int numberOfEnemies = 8;
        [SerializeField] float moveSpeed = 2f;

        public GameObject Get_Enemy_Prefab() { return enemyPrefab; }
        public List<Transform> GetWaypoints()
        {
            var _wavWaypoints = new List<Transform>(); // returning a list of transforms so we need a variable that makes a new list of transforms
            foreach (Transform child in pathPrefab.transform) // we are getting the transform of the object attactched to this game object
            {
                _wavWaypoints.Add(child); // for each added transform child lets add them to our wavwaypoints list
            }

            return _wavWaypoints;
        }
        public float Get_Time_Between_Spawns() { return timeBetweenSpawns; }
        public float Get_Spwan_Random_Factor() { return spawnRandomFactor; }
        public int Get_Number_Of_Enemies() { return numberOfEnemies; }
        public float Get_Move_Speed() { return moveSpeed; }
    }
    // we want to return the waypoints not just the prefab
}