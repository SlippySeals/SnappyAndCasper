using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnappyNCasper.PuzzlePieces.BlockActions;

namespace SnappyNCasper.PuzzlePieces.PressurePlate
{
    /// <summary>
    /// Listens to the inspector set pressure plate.
    /// passes the state to the BlockPuzzleAction function which handles
    /// the blocks behaviour.
    /// </summary>
    [RequireComponent(typeof(IBlockPuzzleAction))]
    public class PlateActionListener : MonoBehaviour
    {
        [SerializeField] private PressurePlate pressurePlateToListenTo;
        private IBlockPuzzleAction BlockBlockPuzzleAction => GetComponent<IBlockPuzzleAction>();
        private void OnEnable() => pressurePlateToListenTo.sendPlateState += HandleBehaviour;
        private void OnDisable() => pressurePlateToListenTo.sendPlateState -= HandleBehaviour;
        private void HandleBehaviour(PlateState state) => BlockBlockPuzzleAction.DoBehaviour(state);
    }
}
