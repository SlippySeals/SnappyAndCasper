using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnappyNCasper.EnemyLogic
{
    public class EnemyAttack : MonoBehaviour
    {

        // PRIVATE FIELDS ---

        [SerializeField] private GameObject activePlayerRef; // what is the player game object
        [Range(1, 4)] [SerializeField] private int damage; // how much damage to deal to the player
        [Range(1f, 5f)] [SerializeField] private float attackRange; // distance to start attacking.
        
        private TestPlayer _playerHealth;
        private Transform _playerTransform;
        
        // METHODS --- 
        
        private void Awake()
        {
            //activePlayerRef = FindObjectOfType<PlayerManager>().activePlayerRef;
            _playerHealth = activePlayerRef.GetComponent<TestPlayer>();
            _playerTransform = activePlayerRef.transform;
        }

        /// <summary>
        /// Returns distance (float) between the player and enemy.
        /// </summary>
        private float CalculateDistance()
        {
            var distance = Vector3.Distance(transform.position, _playerTransform.position);
            //Debug.Log($"The distance is {distance}");
            return distance;
        }

        /// <summary>
        /// determines if the enemy is close enough to the player to start attacking
        /// </summary>
        /// <returns> True allows attacking, false prevents it</returns>
        private bool CheckDistance()
        {
            var threshold = CalculateDistance() <= attackRange;
            Debug.Log($"Can Attack is {threshold}");
            return threshold;
        }

        // reduce the player's health by one.
        private void Attack()
        {
            _playerHealth.ChangeHealth(damage);
            // Debug.Log("Attack is Called");
        }

        // Update is called once per frame
        private void Update()
        {
            //activePlayerRef = FindObjectOfType<PlayerManager>().activePlayerRef;

            if (activePlayerRef == null) return; // stops null reference exception warning
            if (!CheckDistance()) return;
            Attack();
            Debug.Log("Attack Triggered");
        }
    }
}
