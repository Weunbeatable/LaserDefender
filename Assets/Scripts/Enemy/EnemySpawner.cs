using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LD.Core;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> _waveConfigs; // grabbing a list of our wave configs from our waves folder
    [SerializeField] int _startingWave = 0; // our index in waveconfigslist
    [SerializeField] bool _looping = true; // for our enemy wave loops
   IEnumerator Start() // enum in front will turn our start into a start coroutine 
    {
        do // start this series coroutine 
        {
            yield return StartCoroutine(_Spawn_All_Waves()); // yield and come back to start once this is completed
        }
        while (_looping); // if true go back up to the do so we can repeat the process 
    }
    
    private IEnumerator _Spawn_All_Waves()
    {
        for(int _WaveIndex = _startingWave; _WaveIndex < _waveConfigs.Count; _WaveIndex++) // as long as our wave index is less than the size of our waveconfigs list
        {
            var _currentWave = _waveConfigs[_WaveIndex];
            yield return StartCoroutine(_Spawn_All_Enemies_In_wave(_currentWave));
        }
    }
    private IEnumerator _Spawn_All_Enemies_In_wave(WaveConfig _waveConfig) // we are passing our current wave into the wave config
    {
        for (int _EnemyCount = 0; _EnemyCount < _waveConfig.Get_Number_Of_Enemies(); _EnemyCount++) // for sake of being informative change i to enemycount 
        {
            var _newEnemy = Instantiate(
                _waveConfig.Get_Enemy_Prefab(),// object we are instantiating 
                _waveConfig.GetWaypoints()[0].transform.position, // objects position
                Quaternion.identity); // objects rotation
            _newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(_waveConfig);// getting the wave config directly from enemy pathing and we are defining our own config in this class.
            yield return new WaitForSeconds(_waveConfig.Get_Time_Between_Spawns());
        }
    }
}
    
