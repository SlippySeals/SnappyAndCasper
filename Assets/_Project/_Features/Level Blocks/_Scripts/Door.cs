using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<GameObject> doorObjects;
    public bool hideOnTrigger = true;
    private bool setActive;

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<BlockData>())
        {
            if (other.GetComponent<BlockData>().isSolid)
            {
                foreach (var cur in doorObjects)
                {
                    cur.SetActive(setActive = hideOnTrigger ? false : true);
                }

            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BlockData>())
        {
            if (other.GetComponent<BlockData>().isSolid)
            {
                foreach (var cur in doorObjects)
                {
                    cur.SetActive(setActive = hideOnTrigger ? true : false);
                }
            }

        }

    }
}
