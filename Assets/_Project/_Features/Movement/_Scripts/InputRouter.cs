using UnityEngine;
using UnityEngine.InputSystem;

public class InputRouter : MonoBehaviour
{
    // This class manages all player inputs so that other player scripts may reference these inputs more efficiently.
    // This class should be attached to the player in a scene, when referencing inputs for gameplay utilize the variables defined below.

    [SerializeField] PlayerInput playerInput;
    InputActionAsset playerInputtest;

    private bool _isPaused; // 
    
    // VerticalInput: 1 = Up, -1 = Down.
    public float verticalInput;
    // HorizontalInput: 1 = Right, -1 = Left.
    public float horizontalInput;
    // StrafeInput: 1 = Right, -1 = Left.
    public float strafeInput;

    // Simple bools to output whether or not the "Attack" and "Input" buttons are being inputted.
    public bool doAttack;
    public bool doInteract;

    private RotatingCamera camRot;

    // Sets the playerInput value if not set in the editor, returns playerInput.
    private PlayerInput playerController
    {
        get
        {
            if (playerInput == null)
            {
                playerInput = GetComponent<PlayerInput>();

                if (playerInput == null)
                {
                    Debug.Log("No Player Input Script on " + transform.name);
                    return null;
                }
            }
            return playerInput;
        }
    }

    // Adds the ReadInput function to be ran when an input is received.
    private void OnEnable()
    {
        playerController.onActionTriggered += ReadInput;
    }

    // Removes the ReadInput function to be ran when an input is received.
    private void OnDisable()
    {
        playerController.onActionTriggered -= ReadInput;
    }

    private void Awake()
    {
        if (camRot == null)
        {
            camRot = FindObjectOfType<RotatingCamera>();
        }
    }

    // ReadInput is called whenever a player enters an input into our input system. It checks every possible input and sets values accordingly.
    void ReadInput(InputAction.CallbackContext context)
    {
        if (context.action.name == "Skip")
        {
            if (context.performed)
            {
                FindObjectOfType<SceneController>().NextScene();
            }
        }

        if (context.action.name == "ReverseSkip")
        {
            if (context.performed)
            {
                FindObjectOfType<SceneController>().PrevScene();
            }
        }

        if (context.action.name == "CamRotLeft")
        {
            if (context.performed)
            {
                camRot.RotateCamera(-1);
            }
        }

        if (context.action.name == "CamRotRight")
        {
            if (context.performed)
            {
                camRot.RotateCamera(1);
            }
        }

        // Up Button
        if (context.action.name == "Up")
        {
            if (context.performed)
            {
                verticalInput = 1;
            }
            else
            {
                verticalInput = 0;
            }
        }

        // Down Button
        if (context.action.name == "Down")
        {
            if (context.performed)
            {
                verticalInput = -1;
            }
            else
            {
                verticalInput = 0;
            }
        }
        
        // Left Button
        if (context.action.name == "Left")
        {
            if (context.performed)
            {
                horizontalInput = -1;
            }
            else
            {
                horizontalInput = 0;
            }
        }

        // Right Button
        if (context.action.name == "Right")
        {
            if (context.performed)
            {
                horizontalInput = 1;
            }
            else
            {
                horizontalInput = 0;
            }
        }

        // Attack Button
        if (context.action.name == "Attack")
        {
            if (context.performed)
            {
                doAttack = true;
            }
            else
            {
                doAttack = false;
            }
        }

        // Interaction Button
        if (context.action.name == "Interact")
        {
            if (context.performed)
            {
                doInteract = true;
            }
            else
            {
                doInteract = false;
            }
        }

        // Strafe Left Button
        if (context.action.name == "LeftRoll")
        {
            if (context.performed)
            {
                 strafeInput = -1;
            }
            else
            {
                strafeInput = 0;
            }
        }

        // Strafe Right Button
        if (context.action.name == "RightRoll")
        {
            if (context.performed)
            {
                strafeInput = 1;
            }
            else
            {
                strafeInput = 0;
            }
        }

        if (context.action.name == "Pause") {
            if (context.performed)
                _isPaused = !_isPaused;
            PauseMenu.pause?.Invoke(_isPaused);
        }
    }
    
    public void Resume()
    {
       _isPaused = !_isPaused;
       PauseMenu.pause?.Invoke(_isPaused);
    }
}
