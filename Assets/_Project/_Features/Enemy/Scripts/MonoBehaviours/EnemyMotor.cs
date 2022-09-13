using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnappyNCasper.EnemyLogic
{
    /// <summary>
    /// Class that handles the movement and rotation of the enemy entity.
    /// </summary>
    public class EnemyMotor : MonoBehaviour
    {
        // PRIVATE FIELDS ---
        
        [SerializeField] private float moveSpeed, turnSpeed; // how fast to turn and move forward
        private Transform _waypoint; // references for transforms to make code cleaner.

        private bool _facingWaypoint() // true when the enemy is rotated exactly to the 
        {
            var  dot = Vector3.Dot(((Component) this).transform.forward, (_waypoint.position - ((Component) this).transform.position).normalized);
            return dot == 0f;
        }
        
        // PUBLIC FIELDS ---
        
        public Transform CurrentWaypoint  // Exposed property to set the current active waypoint
        {
            set => _waypoint = value;
        }

        // METHODS
        
        /// <summary>
        /// Method to rotate the enemy so it is facing the waypoint before it starts moving.
        /// </summary>
        private void TurnTowardsWaypoint()
        {
            var lookDirection = _waypoint.position - transform.position; // direction to face when rotation completed
            
            // get the desired direction's rotation.
            var directionRotation = Quaternion.LookRotation(
                lookDirection, 
                Vector3.up);
            
            // how fast to turn per frame
            var step = turnSpeed * Time.deltaTime; 
            
            // perform the rotation over time.
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                directionRotation, 
                step);
        }

        /// <summary>
        /// Method that handles moving the enemy towards the waypoint target's position.
        /// </summary>
        private void MoveTowardsWaypoint()
        {
            // how fast to move per frame
            var step = moveSpeed * Time.deltaTime; 
            
            // perform the movement overtime.
            transform.position = Vector3.MoveTowards(
                transform.position, 
                _waypoint.position, 
                step);
        }
        
        // Function that is used to update the enemy motor in the Enemy controller class
        public void MotorUpdate()
        {
            if (!_facingWaypoint()) 
                TurnTowardsWaypoint();
            MoveTowardsWaypoint();
        }
    }
}
