using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig; 
    List<Transform> Waypoint;
    int waypointIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        Waypoint = waveConfig.Getwaypoints();     
        transform.position = Waypoint[waypointIndex].transform.position;  //current position is whatever position of our current waypoint position
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig; // saying this waveconfig we refrenced from the class is equal to the wave config we recievwe when we call this method somewhere else.
    }
    private void move()
    {
        if (waypointIndex <= Waypoint.Count - 1)
        {
            var targetPosition = Waypoint[waypointIndex].transform.position;
            var movementThisframe = waveConfig.GetMoveSpeed() * Time.deltaTime; // delta time allows it to be framerate independent.
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisframe);


            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
