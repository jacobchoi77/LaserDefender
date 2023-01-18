using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour{
    private EnemySpawner _enemySpawner;
    private WaveConfigSo _waveConfig;
    private List<Transform> _waypoints;
    private int _waypointIndex;

    private void Awake(){
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void Start(){
        _waveConfig = _enemySpawner.GetCurrentWave();
        _waypoints = _waveConfig.GetWaypoints();
        transform.position = _waypoints[_waypointIndex].position;
    }

    private void Update(){
        FollowPath();
    }

    private void FollowPath(){
        if (_waypointIndex < _waypoints.Count){
            var targetPosition = _waypoints[_waypointIndex].position;
            var delta = _waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
            if (transform.position == targetPosition){
                _waypointIndex++;
            }
        }
        else{
            Destroy(gameObject);
        }
    }
}