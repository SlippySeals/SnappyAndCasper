using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnappyNCasper.EnemyLogic
{
    /// <summary>
    /// The central enemy component class. This controls the enemies health, movement, and attacking behaviours.  
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        // the collection of waypoint managers used for navigation.
        // this allows for enemies to switch between different waypoint systems.
        [SerializeField] private List<WaypointManager> waypointManagers; // assign in inspector
        [SerializeField] private bool startAtWaypoint;
        [Range(0.01f, 0.1f)] [SerializeField] private float waypointStoppingRange;
        
        private EnemyMotor _motor; // motor component for turning and moving the enemy
        private WaypointManager _currentWaypointManager; // reference for the current waypoints being used.
        private Transform _currentWaypoint; // what is my current waypoint target.
        
        /// <summary>
        /// Used for GameObject initialisation.
        /// </summary>
        private void Start()
        {
            ControllerSetup();
            if (!startAtWaypoint) return;
            transform.position = _currentWaypointManager.waypoints[0].position;
            _currentWaypoint = _currentWaypointManager.GetNextWaypoint(_currentWaypoint);
        }

        private void ControllerSetup()
        {
            _motor = gameObject.GetComponent<EnemyMotor>(); // store reference to enemy's motor component
            _currentWaypointManager = waypointManagers[0]; // initialise to first object in list.
        }
        
        private bool _atWaypoint() // true when the enemy is within waypoints minimum distance.  
        {
            var distanceToWaypoint = Vector3.Distance(_currentWaypoint.position, transform.position);
            return distanceToWaypoint <= waypointStoppingRange;
        }
        
        /// <summary>
        /// This function runs the enemy behaviours.
        ///     1. The enemy will first turn to look at the waypoint
        ///     2. The enemy will move to the waypoint
        ///     3. Once at the waypoint the enemy requests it next waypoint.
        /// </summary>
        private void UpdateEnemy()
        {
            if (!_atWaypoint())
            {
                _motor.MotorUpdate();
            }
            else
            {
                _currentWaypoint =_currentWaypointManager.GetNextWaypoint(_currentWaypoint);
                _motor.CurrentWaypoint = _currentWaypoint;
            } ;
        }

        private void Update()
        {
            UpdateEnemy();
        }
    }
}