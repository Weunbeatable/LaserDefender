using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LD.Core;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig _waveConfig; 
    List<Transform> _Waypoint;
    int _waypointIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _Waypoint = _waveConfig.GetWaypoints();     
        transform.position = _Waypoint[_waypointIndex].transform.position;  //current position is whatever position of our current waypoint position
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    public void SetWaveConfig(WaveConfig _waveConfig)
    {
        this._waveConfig = _waveConfig; // saying this waveconfig we refrenced from the class is equal to the wave config we recievwe when we call this method somewhere else.
    }
    private void move()
    {
        if (_Is_At_Final_Waypoint())
        {
            var _targetPosition = _Waypoint[_waypointIndex].transform.position;
            var _movementThisframe = _waveConfig.Get_Move_Speed() * Time.deltaTime; // delta time allows it to be framerate independent.
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _movementThisframe);

            if (transform.position == _targetPosition)
            {
                _waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private bool _Is_At_Final_Waypoint()
    {
        return _waypointIndex <= _Waypoint.Count - 1;
    }
}
