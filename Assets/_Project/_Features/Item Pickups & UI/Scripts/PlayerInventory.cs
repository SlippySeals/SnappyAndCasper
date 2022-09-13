using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public static int numberOfGems, numberOfHats, maxGems, maxHats;
    [SerializeField] private AudioClip hatPickup;
    [SerializeField] private AudioClip gemPickup;

    public UnityEvent<PlayerInventory> OnGemCollected;

    private void Start()
    {

    }

    public void ItemCollected(string collectableName)
    {
        switch(collectableName)
        {
            default:
                break;
            case "gem":
                if (gemPickup != null) // Adam OD, 16/6 'added a null check'. 
                {
                    AudioSource.PlayClipAtPoint(gemPickup, transform.position);
                }
                numberOfGems++;
                break;
            case "hat":
                if (hatPickup != null)
                {
                    AudioSource.PlayClipAtPoint (hatPickup, transform.position);  
                }
                numberOfHats++;
                break;
        }
        OnGemCollected.Invoke(this);

        if (numberOfHats >= maxHats)
        {
            // YOU WIN, LOAD WIN/END SCREEN
            Debug.Log("Victory Condition Fulfilled");
        }
    }
}