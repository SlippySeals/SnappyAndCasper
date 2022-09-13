using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnappyNCasper.PuzzlePieces.PressurePlate;

namespace SnappyNCasper.PuzzlePieces.PressurePlate
{
    public enum PlateState { Inactive, Active }
        // active means pressure plate is depressed.
        // inactive means pressure plate is raised.
    
    /// <summary>
    /// Component attached to the PressurePlate prefab. Handles the 'Pressed" animation by moving the graphic object.
    /// The component will send the plate state to listening game-objects with the PlateListener Script
    /// letting them change their behaviours.
    /// </summary>
    public class PressurePlate : MonoBehaviour
    {
        // SERIALIZED FIELDS --- 

        [SerializeField] private bool doDebug = true;
            // Lets me toggle my debug logs.

        [SerializeField] protected float bumpHeight = 0.2f;
            // use this to make the cube pop out of the ground when inactive. 

        [SerializeField] protected List<GameObject> entitiesThatCanTriggerPlate;
            // set this up in inspector for the object to check that can activate this plate.

        [SerializeField] protected GameObject plateGraphic;
            // the object that has the mesh of the plate, doing it this way due to how
            // character movement needs the cubes to be at specific transform positions.
            
        // PRIVATE FIELDS ---
        private Vector3 HeightOffset => Vector3.up * bumpHeight;
        
        protected Vector3 SetPlateHeight(PlateState state)
        {
            if (state == PlateState.Inactive) return plateGraphic.transform.position + HeightOffset; 
                // if the pressure plate is not being stepped on move graphic into the ground.
            
            return plateGraphic.transform.position - HeightOffset;
                // otherwise it sticks up out of the ground slightly till we step on it. 
        }

        protected bool CanEntityTriggerPlate(GameObject entity) => entitiesThatCanTriggerPlate.Contains(entity);
            // Check if the colliding entity is within the list it can react to.
        
        // PUBLIC FIELDS ---
            
        public Action<PlateState> sendPlateState; 
            // Send State event to listening objects.
            
        // METHODS ---
        
        protected void Start() => plateGraphic.transform.position = SetPlateHeight(PlateState.Inactive);
        //      // set the cube I use as a plate animation to state inactive (so popped out of the ground)
        
        protected virtual void OnTriggerEnter(Collider other) => HandlePressurePlate(other.gameObject, PlateState.Active);
        //      // When entering trigger collider activate the pressure plate and notify listening entities
        protected virtual void OnTriggerExit(Collider other) => HandlePressurePlate(other.gameObject, PlateState.Inactive);
        //      // When leaving the trigger collider deactivate the pressure plate and notify listening entities
        
        // Check if the collided entity is part of the list, if update the plate animation and trigger the state event
        protected virtual void HandlePressurePlate(GameObject entity, PlateState state)
        {
            if (!CanEntityTriggerPlate(entity)) return;
                // if entity can't interact do nothing
            plateGraphic.transform.position = SetPlateHeight(state);
                // otherwise modify the plateGraphic height
            sendPlateState?.Invoke(state);
                // send the plate state to listening puzzle pieces
                if (doDebug) Debug.Log($"The state is: {state.ToString()}");
        }
    }
}
