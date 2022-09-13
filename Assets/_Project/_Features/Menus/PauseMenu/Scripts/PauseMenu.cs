using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanelRef;
    public static Action<bool> pause;
    
    private void OnEnable()
    {
        pause += PauseGame;
    }

    private void OnDisable()
    {
        pause -= PauseGame;;
    }

    private void PauseGame(bool enable)
    {
        pausePanelRef.gameObject.SetActive(enable);
        Time.timeScale = enable ? 0 : 1;
    }

}
