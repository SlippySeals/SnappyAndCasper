using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelPreview : MonoBehaviour
{
    [SerializeField] private List<Sprite> levelImages;
    [SerializeField] private Image imageDisplay;

    public void UpdatePreviewImage(int levelIndex)
    {
        Debug.Log("OVER");
        if (imageDisplay && levelImages[levelIndex])
        {
            imageDisplay.sprite = levelImages[levelIndex];
        }
    }
}
