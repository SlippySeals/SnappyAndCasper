using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SnappyNCasper.PuzzlePieces.PressurePlate
{
    /// <summary>
    /// This class script modifies the base pressure plate by adding a timer.
    /// Timer starts when we step off the plate. Timer restarts when stepping onto the plate.
    /// The coroutine dispatches the sendPlateState Action to objects with a PlateListener Component 
    /// </summary>
    public class TimedPressurePlate : PressurePlate
    {
        // SERIALIZED FIELDS ---
        
        [SerializeField] private float timerDuration = 7f; 
            // how long does the timer stay active for.
        
        // PRIVATE FIELDS ---
        
        private void StartTimer() => StartCoroutine(PlateTimer());
            // function for better readability
        private void ResetTimer() => StopCoroutine(PlateTimer());
            // function for better readability
            
        // METHODS ---
        
        /// <summary>
        /// Timer for controlling the sendPlateState Action to objects with PlateListener Component
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlateTimer()
        {
            sendPlateState(PlateState.Active); 
                // stepped off plate, activate puzzle piece
                Debug.Log("Timer Started Activating Piece");
            yield return new WaitForSeconds(timerDuration); 
                // run timer
            sendPlateState?.Invoke(PlateState.Inactive); 
                // timer finished deactivate puzzle piece
                Debug.Log("Timer Finished Deactivating Piece");
        }
        
        /// <summary>
        /// Takes the entity from the OnCollision functions to determine if plate should react.
        /// Takes a plate state and gives it to the timer to determine pressure plate behaviour  
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        protected override void HandlePressurePlate(GameObject entity, PlateState state)
        {
            if (!CanEntityTriggerPlate(entity)) return;
                // return if object can't trigger pressure plate.
            
            plateGraphic.transform.position = SetPlateHeight(state);
                // otherwise modify the plateGraphic height
            
            HandleTimer(state);
                // action event is delayed through the timer.
        }

        /// <summary>
        /// Function that runs a switch on the current plateState.
        /// Stepping onto the plate resets the timer, Stepping of the plate starts it 
        /// </summary>
        /// <param name="state"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void HandleTimer(PlateState state)
        {
            switch (state)
            {
                case PlateState.Active : ResetTimer(); break; // since entity stepped on it
                case PlateState.Inactive : StartTimer(); break; // since entity stepped off it
                default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}