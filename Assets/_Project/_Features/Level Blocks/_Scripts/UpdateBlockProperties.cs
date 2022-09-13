using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBlockProperties : MonoBehaviour
{
    public bool makeSolidOnEntered;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER");
        if (other.GetComponent<BlockData>() && makeSolidOnEntered)
        {
            if (other.GetComponent<BlockData>().isSolid)
            {
                Debug.Log("RAMP IS SOLID");
                GetComponent<BlockData>().isSolid = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BlockData>() && makeSolidOnEntered)
        {
            Debug.Log("RAMP IS NOT SOLID");
            GetComponent<BlockData>().isSolid = false;
        }
    }
}
