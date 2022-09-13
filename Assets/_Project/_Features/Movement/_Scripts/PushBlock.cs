
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// CLASS COMMENT --- Only goes on the 'push' block, allows the block instance to be pushed forward.
public class PushBlock : MonoBehaviour
{
    /// PRIVATE AND PROTECTED FEILDS ---

    [SerializeField] int fallLimit;

    private void OnEnable()
    {
        GameEvents.DoMoveChecksEvent += MoveLogic;
    }

    private void OnDisable()
    {
        GameEvents.DoMoveChecksEvent -= MoveLogic;
    }

    private Vector3 targetTile; // {what is this}
    private BlockData targetData; // {what is this}

    /// <summary>
    /// Attempt to push the block, return success as bool.
    /// </summary>> 
    public bool TryPushBlock (Vector3 pushPosition)
    {
        // Grab block data of tile to the left and above then try move.
        targetTile = pushPosition;
        targetData = CheckBlock(targetTile);

        // Cannot move if the target is solid or a ramp.
        if (targetData == null || (targetData.isSolid == false && targetData.isRamp == false))
        {
            MoveToTile(targetTile);
            return true;
        }
        return false;
    }

    ///<summary>
    /// This function grabs the BlockData class from any block occupying a specified Vector 3 coordinate.
    /// </summary>> 
    private BlockData CheckBlock(Vector3 target)
    {
        // Grab all blocks and loop through them. Return null if no blocks match position or return block gameObject that does.
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Environment");
        foreach (GameObject currentBlock in allBlocks)
        {
            if (currentBlock.transform.position == target)
            {
                return currentBlock.GetComponent<BlockData>();
            }
        }
        return null;
    }

    ///<summary>>
    /// This function moves the player to the vector 3 coordinates provided.
    /// After moving it moves the player down 1 unit on the Y axis if no solid block is detected.
    /// </summary> 
    private void MoveToTile(Vector3 position)
    {
        transform.LookAt(targetTile);
        // Move the player to provided Vector 3
        transform.position = position;

        // Check block data of tile below player, if empty or not solid fall down to tile.
        
        targetTile = position + Vector3.up * -1;
        
        targetData = CheckBlock(targetTile);
        
        // Convert this into a loop. 
        if (transform.position.y > fallLimit && (targetData == null || targetData.isSolid == false))
        {
            MoveToTile(targetTile);
        }
        else if (transform.position.y <= fallLimit)
        {
            Destroy(gameObject);
        }
        else if (targetData.isSlidey)
            {
                TryPushBlock(transform.position + transform.forward);
            }
    }

    void MoveLogic()
    {
        targetTile = transform.position + Vector3.up * -1;

        targetData = CheckBlock(targetTile);

        // Convert this into a loop. 
        if (transform.position.y > fallLimit && (targetData == null || targetData.isSolid == false))
        {
            MoveToTile(targetTile);
        }
        else if (transform.position.y <= fallLimit)
        {
            Destroy(gameObject);
        }
    }
}
