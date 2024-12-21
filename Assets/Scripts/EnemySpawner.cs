using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs; // grabbing a list of our wave configs from our waves folder
    [SerializeField] int startingWave = 0; // our index in waveconfigslist
    [SerializeField] bool looping = true; // for our enemy wave loops
   IEnumerator Start() // enum in front will turn our start into a start coroutine 
    {
        do // start this series coroutine 
        {
            yield return StartCoroutine(SpawnAllWaves()); // yield and come back to start once this is completed
        }
        while (looping); // if true go back up to the do so we can repeat the process 
    }
    
    private IEnumerator SpawnAllWaves()
    {
        for(int WaveIndex = startingWave; WaveIndex < waveConfigs.Count; WaveIndex++) // as long as our wave index is less than the size of our waveconfigs list
        {
            var currentWave = waveConfigs[WaveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInwave(currentWave));
        }
    }
    private IEnumerator SpawnAllEnemiesInwave(WaveConfig waveConfig) // we are passing our current wave into the wave config
    {
        for (int EnemyCount = 0; EnemyCount < waveConfig.GetNumberOfEnemies(); EnemyCount++) // for sake of being informative change i to enemycount 
        {
            var newEnemy = Instantiate(
                waveConfig.GetenemyPrefab(),// object we are instantiating 
                waveConfig.Getwaypoints()[0].transform.position, // objects position
                Quaternion.identity); // objects rotation
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);// getting the wave config directly from enemy pathing and we are defining our own config in this class.
            yield return new WaitForSeconds(waveConfig.GettimeBetweenSpawns());
        }
    }
}
    
