using UnityEngine;

/// <summary>
/// ADAM REVIEW: Really this can be a scriptable object so we can create this data
/// as an asset in the project files.
/// </summary>
public class BlockData : MonoBehaviour// we can implement a state pattern later.
{
    // Determines whether the player can move through the block.
    public bool isSolid;

    // Determines whether this block can be moved.
    public bool isDynamic;

    // Determines whether this block is climbable wall.
    public bool canClimb;

    // Determines whether this block is a ramp.
    public bool isRamp;

    // Determines whether this block is a death block.
    public bool isDeath;

    // Determines whether this block is interactable.
    public bool isInteractable;

    // Determines whether this block is slippery.
    public bool isSlidey;
}
