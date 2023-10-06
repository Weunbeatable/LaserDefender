using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 8;
    [SerializeField] float moveSpeed = 2f;

    public GameObject GetenemyPrefab() {  return enemyPrefab;  }
    public List<Transform> Getwaypoints() 
    {
        var wavWaypoints = new List<Transform>(); // returning a list of transforms so we need a variable that makes a new list of transforms
        foreach(Transform child in pathPrefab.transform) // we are getting the transform of the object attactched to this game object
        {
            wavWaypoints.Add(child); // for each added transform child lets add them to our wavwaypoints list
        }

        return wavWaypoints;
    }
    public float GettimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetspwanRandomFactor() { return spawnRandomFactor; }
    public int GetNumberOfEnemies() { return numberOfEnemies; }
    public float GetMoveSpeed() { return moveSpeed; }
}
// we want to return the waypoints not just the prefab