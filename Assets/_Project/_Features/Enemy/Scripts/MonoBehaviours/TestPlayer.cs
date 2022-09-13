using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnappyNCasper.EnemyLogic
{
    public class TestPlayer : MonoBehaviour
    {
        [Range(1, 5)] [SerializeField] private int playerHealth = 1;

        public void ChangeHealth(int val)
        {
            playerHealth -= val;
            Debug.Log($"Change Health Called. Health is now {playerHealth}");
            
            if (playerHealth <= 0)
                Die();
        }

        // when health is below 0 trigger the player death
        void Die()
        {
            // Destroy(gameObject);

            // do respawn here
            FindObjectOfType<SceneController>().Respawn();

        }
    }
}
