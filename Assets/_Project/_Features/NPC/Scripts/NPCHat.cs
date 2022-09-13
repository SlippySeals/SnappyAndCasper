using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCHat : MonoBehaviour
{
    public ExitControl nextLevel;
    public PlayerInventory hatRequirement;
 
    // Update is called once per frame
    void Update()
    {
        if(PlayerInventory.numberOfHats == 1)
        {
        nextLevel.SetPortalActive();
        }
    }
}
