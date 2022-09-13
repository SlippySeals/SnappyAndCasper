using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public ExitControl nextLevel;
    public PlayerInventory hatRequirement;
    private bool isTrue;
    [SerializeField] private MeshRenderer hat;

    [SerializeField] GameObject particles;


    public List<string> npcDialogue;
    public List<string> npcCompletedDialogue;

    private void Update()
    {
        if (PlayerInventory.numberOfHats >= 1 && !isTrue)
        {
            particles.SetActive(true);
            isTrue = true;
        }
    }

    public void PlayerInteract()
    {
        if(PlayerInventory.numberOfHats >= 1)
        {
            nextLevel.SetPortalActive();
            particles.SetActive(false);
            FindObjectOfType<TextManager>().StartNewText(npcCompletedDialogue);
            FindObjectOfType<PlayerManager>().wearHat = false;
            hat.enabled = true;
        }
        else
        {
            FindObjectOfType<TextManager>().StartNewText(npcDialogue);
        }
    }

}
