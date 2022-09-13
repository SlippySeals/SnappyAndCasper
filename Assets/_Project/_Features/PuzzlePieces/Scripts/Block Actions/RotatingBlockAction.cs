using System;
using System.Linq;
using SnappyNCasper.PuzzlePieces.PressurePlate;
using UnityEngine;

namespace SnappyNCasper.PuzzlePieces.BlockActions
{
    public enum Rotate
    {
        Clockwise = 1,
        AntiClockwise = 0
    }

    public class RotatingBlockAction : MonoBehaviour, IBlockPuzzleAction
    {
        [SerializeField] private Rotate[] rotationMap = new Rotate[4];
        // stores a rotation direction at each of the 4 cardinal directions

        private int _currentRotationIndex = 0;

        // what rotation are we currently at?
        private static Vector3 ClockwiseRotationVector => Vector3.up * 90f;

        // I save the rotation degrees into a vector3
        private static Vector3 AntiClockwiseRotationVector => ClockwiseRotationVector * -1;
        // I invert the property to get anticlockwise rotation.

        private void Awake() => _currentRotationIndex = 0;

        private void IncrementIndex()
        {
            var nextRotationIndex = _currentRotationIndex + 1;
            if (nextRotationIndex >= rotationMap.Length) _currentRotationIndex = 0;
            else _currentRotationIndex++;
        }
        
        /// <summary>
        /// Interface required behaviour that is accessed by the plate listener component.
        /// </summary>
        /// <param name="activate"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void DoBehaviour(PlateState plateState)
        {
            if(plateState == PlateState.Inactive) return;
            var currentRotation = rotationMap[_currentRotationIndex];
            switch (currentRotation)
            {
                case Rotate.Clockwise:
                    transform.Rotate(ClockwiseRotationVector);
                    IncrementIndex();
                    break;
                case Rotate.AntiClockwise:
                    transform.Rotate(AntiClockwiseRotationVector);
                    IncrementIndex();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
