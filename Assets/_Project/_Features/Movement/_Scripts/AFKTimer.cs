using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AFKTimer : MonoBehaviour
{
    [SerializeField] private SceneController sceneManager;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float afkTimeLimit;
    private float currentTime;

    private void OnEnable()
    {
        // subscribe to the new input system
        playerInput.onActionTriggered += PlayerInput_onActionTriggered;
    }

    private void OnDisable()
    {
        // unsubscribe to the new input system.
        playerInput.onActionTriggered -= PlayerInput_onActionTriggered;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (sceneManager == null)
        {
            sceneManager = FindObjectOfType<SceneController>();
        }

        currentTime = Time.time + afkTimeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= currentTime)
        {
            sceneManager.MainMenu();
        }
    }
    private void PlayerInput_onActionTriggered(InputAction.CallbackContext obj)
    {
        currentTime = Time.time + afkTimeLimit;
    }
}
