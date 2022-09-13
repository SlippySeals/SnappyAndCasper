using System;
using SnappyNCasper.PuzzlePieces.PressurePlate;
//using UnityEditor.VersionControl;
using UnityEngine;

public enum RaisingBehaviour
{
    Once,
    Toggle,
    Alternating // make sure this is set for Timed pressure plates. 
}

public enum MoveDirection
{
    Up,
    Down,
    Left,
    Right,
    Forward,
    Backward,
}

namespace SnappyNCasper.PuzzlePieces.BlockActions
{
    /// <summary>
    /// This test component lets people tell the object what to do when a pressure plate signal has been received.
    /// I allow people to activate / deactivate, raise and reset, and rotate.
    /// </summary>
    public class RaisingBlockAction : MonoBehaviour, IBlockPuzzleAction
    {
        [SerializeField] private RaisingBehaviour raisingBehaviour;
        [SerializeField] private float amountToRaise = 1f;
        [SerializeField] private MoveDirection[] moveDirectionsArray;
        
        private bool _hasToggledOnce = false;
        private int _currentMovementIndex = 0;

        private void Awake() => _currentMovementIndex = 0;
        
        private Vector3 CurrentPosition
        {
            get => gameObject.transform.position;
            set => gameObject.transform.position = value;
        }

        private void ChangePosition(MoveDirection direction)
        {
            CurrentPosition = direction switch
            {
                MoveDirection.Up => CurrentPosition + Vector3.up * amountToRaise,
                MoveDirection.Down => CurrentPosition + Vector3.down * amountToRaise,
                MoveDirection.Left => CurrentPosition + Vector3.left * amountToRaise,
                MoveDirection.Right => CurrentPosition + Vector3.right * amountToRaise,
                MoveDirection.Forward => CurrentPosition + Vector3.forward * amountToRaise,
                MoveDirection.Backward => CurrentPosition + Vector3.back * amountToRaise,
                _ => throw new Exception()
            };
            IncrementIndex();
        }

        private void IncrementIndex()
        {
            var nextIndex = _currentMovementIndex + 1;
            if (nextIndex >= moveDirectionsArray.Length) _currentMovementIndex = 0;
            else _currentMovementIndex++;
        }
        
        // METHODS ---

        private void NextPosition() => ChangePosition(moveDirectionsArray[_currentMovementIndex]);

        public void DoBehaviour(PlateState state)
        {
            switch (raisingBehaviour)
            {
                case RaisingBehaviour.Once:
                    if (_hasToggledOnce) return;
                    NextPosition();
                    _hasToggledOnce = true;
                    break;
                case RaisingBehaviour.Toggle:
                    if (state == PlateState.Inactive) return;
                    NextPosition();
                    break;
                case RaisingBehaviour.Alternating:
                    NextPosition();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
