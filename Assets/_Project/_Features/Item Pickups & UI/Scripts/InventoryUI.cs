using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public string pickUpType = "gem";
    private TextMeshProUGUI collectibleText;

    // Start is called before the first frame update
    void Start()
    {
        collectibleText = GetComponent<TextMeshProUGUI>();
        PlayerInventory.maxGems = GameObject.FindGameObjectsWithTag("Gem").Length;
        PlayerInventory.maxHats = 1; // THIS IS HARD CODED, MORE THAN 1 HAT WONT UPDATE
        PlayerInventory.numberOfGems = 0;
        PlayerInventory.numberOfHats = 0;
        UpdateGemText(FindObjectOfType<PlayerInventory>());
    }

    public void UpdateGemText(PlayerInventory playerInventory)
    {
        switch (pickUpType)
        {
            default:
                collectibleText.text = PlayerInventory.numberOfGems.ToString() + "/" + PlayerInventory.maxGems;
                break;
            case "gem":
                collectibleText.text = PlayerInventory.numberOfGems.ToString() + "/" + PlayerInventory.maxGems;
                break;
            case "hat":
                collectibleText.text = PlayerInventory.numberOfHats.ToString() + "/" + PlayerInventory.maxHats;
                break;
        }
    }

}
