using System;
using SnappyNCasper.PuzzlePieces.PressurePlate;
using UnityEngine;

namespace SnappyNCasper.PuzzlePieces.BlockActions
{
    public enum AppearingType
    {
        Once,
        Toggle,
        Analog
    }

    public class AppearingBlockAction : MonoBehaviour, IBlockPuzzleAction
    {
        [SerializeField] private bool startsActive = true;
        [SerializeField] private AppearingType appearingType;
        
        private Collider MyCollider => gameObject.GetComponent<Collider>();
        private MeshRenderer MyRenderer => gameObject.GetComponent<MeshRenderer>();

        private BlockData _blockData => gameObject.GetComponent<BlockData>();
        
        private bool _hasToggledOnce = false;
        
        private void Awake() => SetBlockState(startsActive);
        private void Start() => _hasToggledOnce = false;
        private bool IsBlockCurrentlyActive() => MyCollider.enabled && MyRenderer.enabled && _blockData.isDynamic;
        
        /// <summary>
        /// This function will handled the objects appearing / disappearing behaviour
        /// according to the settings set on this component in the inspector.
        /// </summary>
        /// <param name="state"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void DoBehaviour(PlateState state)
        {
            switch (appearingType)
            {
                case AppearingType.Once: // change state when plate is active and hasn't already toggled.
                    if (_hasToggledOnce) return;
                    ToggleState();
                    _hasToggledOnce = true;
                    break;
                case AppearingType.Toggle: // change state when getting plate active state
                    if (state == PlateState.Inactive) return;
                    ToggleState();
                    break;
                case AppearingType.Analog: // change state when plate is active and inactive
                    ToggleState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Takes a bool that sets the blocks collider and mesh to display or not.
        /// </summary>
        /// <param name="state"></param>
        private void SetBlockState(bool state)
        {
            MyCollider.enabled = state;
            MyRenderer.enabled = state;
            _blockData.isDynamic = state; // We really need to refactor this for my sanity...
            _blockData.isSolid = state;
            _blockData.isInteractable = state;
        }

        /// <summary>
        /// Simple function that inverts the active state of the object. 
        /// </summary>
        private void ToggleState() => SetBlockState(!IsBlockCurrentlyActive());
    }
}
