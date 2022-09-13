using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SnappyNCasper.EnemyLogic
{
    /// <summary>
    /// Attached to an empty game object, it makes all empty game object children waypoints.
    /// It will create a list of these objects and draw gizmos to show the path network they make.
    /// </summary>
    
    public class WaypointManager : MonoBehaviour
    {
        // Lets me float the gizmos above their transform.
        [Range(0f, 1f)] [SerializeField] private float yAxisOffset = 0.25f;
        
        // List of waypoints for the enemy to move between
        public List<Transform> waypoints;
        
        /// <summary>
        /// Initialize fields.
        /// </summary>
        private void Awake()
        {
            foreach (Transform child in transform)
            {
                waypoints.Add(child);
            }
        }
        
        /// <summary>
        /// Following a tutorial here - // https://www.youtube.com/watch?v=EwHiMQ3jdHw
        /// Lets me have empty game objects and then see them only in the inspector.
        /// </summary>
        private void OnDrawGizmos()
        {
            var hoverDist = Vector3.up * yAxisOffset;
            var childCount = transform.childCount;
            var firstWaypoint = transform.GetChild(0);
            var lastWaypoint = transform.GetChild(childCount - 1);
            
            foreach (Transform child in transform)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(child.position + hoverDist, child.localScale);
            }

            Gizmos.color = Color.cyan; // makes the line between waypoints cyan

            for (int i = 0; i < childCount - 1; i++)
            {
                Gizmos.DrawLine(
                    transform.GetChild(i).position + hoverDist, // the origin
                    transform.GetChild(i + 1).position + hoverDist // the destination
                );
            }

            Gizmos.DrawLine(lastWaypoint.position + hoverDist, firstWaypoint.position + hoverDist);
        }

        // Provide enemy with the next waypoint they need to head towards
        public Transform GetNextWaypoint(Transform waypoint)
        {
            var waypointIndex = waypoints.IndexOf(waypoint);
            
            Debug.Log((waypointIndex >= waypoints.Count - 1 ? waypoints[0] : waypoints[waypointIndex + 1]).ToString());
            
            return waypointIndex >= waypoints.Count - 1 ? waypoints[0] : waypoints[waypointIndex + 1];
            
        }
    }
}
