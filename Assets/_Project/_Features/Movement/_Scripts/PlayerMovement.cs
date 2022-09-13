using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerMovement : MonoBehaviour
{
    // Defines player input router class, used to determine player inputs.
    [SerializeField] protected InputRouter playerInput;

    [SerializeField] protected SceneController sceneManager;

    [SerializeField] private int fallLimit;

    [SerializeField] private GameObject modelParentRef;

    [SerializeField] MeshRenderer hat;
    public bool wearHat = false;

    private Vector3 direction, movePosition;
    private Quaternion targRot;

    public bool canMove = true;
    public playerType activePlayer;

    // Player state variables to determine possible movement.
    protected bool onRamp;
    protected bool isClimbing;
    protected bool isFalling;

    public bool canSlide, canPull, canPush, canClimb, canCollect;

    private bool spare;

    private SFX soundPlayer;

    private PlayerManager pm;

    public float moveSpeed;

    public bool isPulling, isMovingAnim, doPull, isRotatingAnim;
    public GameObject pullTarget;

    public bool isSnappy, isCasper;

    // Move cooldown is used to prevent the player from moving forward every frame when an input is received.
    // This functionality can be removed eventually or utilized to allow time for animations to play?
    [SerializeField] protected float movementCooldownTimer = 0.2f;
    protected float moveCooldown;

    public float interactCooldownTimer;
    public float interactCooldown = 0.5f;

    // Temporary variables used to store coordinates and variables of a required block.
    protected Vector3 targetTile, aboveTargetTile; // Projections from current point, forward and up.
    protected BlockData targetData, aboveTargetData; // Determines the behaviour of block

    /// METHODS ---

    private void OnEnable()
    {
        GameEvents.DoMoveChecksEvent += PhysicsCheck;
    }
    private void OnDisable()
    {
        GameEvents.DoMoveChecksEvent -= PhysicsCheck;
    }

    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = FindObjectOfType<SFX>();
        // Grab the InputRouter class if not defined in editor.
        if (playerInput == null)
        {
            playerInput = FindObjectOfType<InputRouter>();
        }

        if (sceneManager == null)
        {
            sceneManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>();
        }

        pm = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        if (pm.wearHat)
        {
            hat.enabled = true;
        }
        else
        {
            hat.enabled=false;
        }

        moveCooldown -= Time.deltaTime;
        interactCooldownTimer -= Time.deltaTime;

        if (isMovingAnim)
        {
            Debug.Log("Moving");
            if (doPull)
            {
                if (pullTarget == null)
                {
                    doPull = false;
                }
                pullTarget.transform.position = transform.position;
                doPull = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, movePosition, moveSpeed * Time.deltaTime);
            if (transform.position == movePosition)
            {
                isMovingAnim = false;
                if (canMove)
                {
                    GameEvents.DoMoveChecksEvent?.Invoke();
                }
                MoveLogic();
            }
        }
        else if (modelParentRef)
            {
                if (onRamp)
                {
                    modelParentRef.transform.localPosition = new Vector3(0, 0.25f, 0);
                }
                else
                {
                    modelParentRef.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

        //if (canMove && isRotatingAnim)
        //{
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targRot, moveSpeed);
        //    if (transform.rotation == targRot)
        //    {
        //        isMovingAnim = false;
        //    }
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activePlayer != pm.activePlayer || !pm.canMove)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        // For fun, wear the hat if true
        //if (wearHat)
        //{
        //    hat.enabled = true;
        //}

        // Reduce move cooldown timer.


        // Prevent inputs if player is not allowed to move
        if (canMove == false || isMovingAnim)
        {
            return;
        }

        // Reset movement cooldown if no inputs are being received.
        // If on cooldown, return and do not check for any inputs.
        if (playerInput.verticalInput == 0 && playerInput.horizontalInput == 0 && playerInput.strafeInput == 0)
        {
            moveCooldown = 0;
        } else if (moveCooldown > 0)
        {
            return;
        }

        if (playerInput.doInteract == false)
        {
            interactCooldownTimer = 0;
        }

        // Adam review -- This could be moved to a MovementHandler Class to follow SOLID. 
        // The below conditionals check for the relevant input axis and call the corresponding movement function.
        if (playerInput.verticalInput == 1)
        {
            MoveForward();
        }

        if (playerInput.verticalInput == -1)
        {
            MoveBackward();
        }

        if (playerInput.strafeInput == 1 && !isPulling)
        {
            StrafeRight();
        }

        if (playerInput.strafeInput == -1 && !isPulling)
        {
            StrafeLeft();
        }

        if (playerInput.horizontalInput == 1 && !isPulling)
        {
            TurnRight();
        }

        if (playerInput.horizontalInput == -1 && !isPulling)
        {
            TurnLeft();
        }

        if (playerInput.doInteract == true && interactCooldownTimer <= 0)
        {
            interactCooldownTimer = interactCooldown;
            Interact();
        }
    }

    public void ApplyInteractCooldown()
    {
        interactCooldownTimer = interactCooldown;
    }

    // This function grabs the BlockData class from any block occupying a specified Vector 3 coordinate.
     protected BlockData CheckBlock(Vector3 target)
    {
        // Grab all blocks and loop through them. Return null if no blocks match position or return block gameObject that does.
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Environment");
        foreach (GameObject currentBlock in allBlocks)
        {
            if (currentBlock.transform.position == target)
            {
                Debug.Log(currentBlock.GetComponent<BlockData>());
                return currentBlock.GetComponent<BlockData>();
            }
        }
        return null;
    }

    private void MoveLogic()
    {
        Debug.Log(gameObject + " MOVING");

        if (targetData != null)
        {
            if (targetData.isDeath == true)
            {
                sceneManager.Respawn();
            }
        }

        // Check block data of tile below player, if empty or not solid fall down to tile.
        targetTile = transform.position + transform.up * -1;
        targetData = CheckBlock(targetTile);
        if (transform.position.y > fallLimit && (targetData == null || targetData.isSolid == false))
        {
            isFalling = true;

            // If the player will occupy a ramp block, set onRamp to true.
            if (targetData)
            {
                onRamp = targetData.isRamp ? true : false;
            }
            else
            {
                onRamp = false;
            }

            isPulling = false;
            pullTarget = null;
            MoveToTile(targetTile);
        }
        else if (transform.position.y <= fallLimit)
        {
            sceneManager.Respawn();
        }
        else 
        {
            isFalling = false;

            // If canSlide, continuously travel in direction
            if (targetData.isSlidey && canSlide)
            {
                targetTile = transform.position + direction;
                targetData = CheckBlock(targetTile);
                TryMove();
            }
        }
    }

    // This function checks the current target blocks and attempts to make a move to that tile.
    // The boolean paramater is used to determine whether the player is facing a push block, allowing movement through it.
    protected void TryMove(bool isForward = false, bool isBackward = false)
    {
        direction = targetTile - transform.position;

        if (targetData == null || targetData.isSolid == false)
        {
            // If the player will occupy a ramp block, set onRamp to true.
            if (targetData)
            {
                onRamp = targetData.isRamp ? true : false;
            }
            else
            {
                onRamp = false;
            }
            // Play walk noise
            soundPlayer.PlaySoundEffect("walk");
            MoveToTile(targetTile, isPulling);
        }
        else if (onRamp == true && canClimb && (aboveTargetData == null || aboveTargetData.isSolid == false))
        {
            // If the player will occupy a ramp block, set onRamp to true.
            if (aboveTargetData)
            {
                onRamp = aboveTargetData.isRamp ? true : false;
            }
            else
            {
                onRamp = false;
            }

            // THIS PREVENTS CASPER MOVEMENT FROM CLIMBING BLOCKS!
            if (canClimb)
            {
                isPulling = false;
                pullTarget = null;
                // Play Spring Noise
                soundPlayer.PlaySoundEffect("spring");
                MoveToTile(aboveTargetTile);
            }

        }
        else
        {
            // If the solid block in front of us is a pushable block that can move, allow us to move.
            var dynamicBlock = targetData.GetComponent<PushBlock>();
            if (dynamicBlock != null && isForward == true && targetData.isDynamic)
            {
                if (canPush)
                {
                    if (!dynamicBlock.TryPushBlock(targetTile + transform.forward))
                    {
                        return;
                    }
                    isPulling = false;
                    pullTarget = null;
                    // Play walk Sound
                    soundPlayer.PlaySoundEffect("walk");
                    MoveToTile(targetTile);
                }
            }
        }
    }

    // This function moves the player to the vector 3 coordinates provided. After moving it moves the player down 1 unit on the Y axis if no solid block is detected.
    protected void MoveToTile(Vector3 position, bool bringPullTarget = false)
    {
        movePosition = position;
        doPull = bringPullTarget;
        MoveAnim();

        // Move the player to provided Vector 3
        //if (bringPullTarget)
        //{
        //    pullTarget.transform.position = transform.position;
        //}
        //transform.position = position;
        //moveCooldown = movementCooldownTimer;

        
    }

    // This function moves the player forward.
    protected void MoveForward()
    {
        // Grab block data of tile to the left and above then try move.
        // This TryMove() sends the "true" parameter to evaluate whether it can push a block.
        targetTile = transform.position + transform.forward;
        targetData = CheckBlock(targetTile);
        aboveTargetTile = targetTile + transform.up;
        aboveTargetData = CheckBlock(aboveTargetTile);
        TryMove(true);
    }

    public bool IsForwardClear()
    {
        targetTile = transform.position + transform.forward;
        targetData = CheckBlock(targetTile);
        aboveTargetTile = targetTile - transform.up;
        aboveTargetData = CheckBlock(aboveTargetTile);
        if (targetData == null && aboveTargetData != null)
        {
            if (aboveTargetData.isSolid == true)
            {
                return true;
            }
        }
            return false;
    }

    // This function moves the player backward.
    protected void MoveBackward()
    {
        // Grab block data of tile behind and above then try move.
        targetTile = transform.position + transform.forward * -1;
        targetData = CheckBlock(targetTile);
        aboveTargetTile = targetTile + transform.up;
        aboveTargetData = CheckBlock(aboveTargetTile);
        TryMove();
    }

    // This function moves the player to their right.
    protected void StrafeRight()
    {
        // Grab block data of tile to the right and above then try move.
        targetTile = transform.position + transform.right;
        targetData = CheckBlock(targetTile);
        aboveTargetTile = targetTile + transform.up;
        aboveTargetData = CheckBlock(aboveTargetTile);
        TryMove();
    }

    // This function moves the player to their left.
    protected void StrafeLeft()
    {
        // Grab block data of tile to the left and above then try move.
        targetTile = transform.position + transform.right * -1;
        targetData = CheckBlock(targetTile);
        aboveTargetTile = targetTile + transform.up;
        aboveTargetData = CheckBlock(aboveTargetTile);
        TryMove();
    }

    // This function rotates the player 90 degrees clockwise.
    protected void TurnRight()
    {
        //targRot = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z);
        //isRotatingAnim = true;
        transform.Rotate(0, 90, 0);
        soundPlayer.PlaySoundEffect("rotate");
        moveCooldown = movementCooldownTimer;
    }

    // This function rotates the player 90 degrees counter-clockwise.
    protected void TurnLeft()
    {
        //targRot = Quaternion.Euler(transform.rotation.x, transform.rotation.y - 90, transform.rotation.z);
        //isRotatingAnim = true;
        transform.Rotate(0,-90,0);
        soundPlayer.PlaySoundEffect("rotate");
        moveCooldown = movementCooldownTimer;
    }

    private void Interact()
    {
        Debug.LogWarning("PLAYER IS INTERACTING");
        targetTile = transform.position + transform.forward;
        targetData = CheckBlock(targetTile);
        if (targetData != null)
        {
            if (targetData.isInteractable == true)
            {
                targetData.gameObject.SendMessage("PlayerInteract");
                interactCooldownTimer = interactCooldown;

                if (targetData.isDynamic && canPull)
                {
                    pullTarget = !pullTarget ? targetData.gameObject : null;
                    isPulling = pullTarget ? true : false;
                }
            }
        }
    }

    public void PlayerInteract()
    {
        pm.MergePlayer(transform);
    }

    void MoveAnim()
    {
        moveCooldown = movementCooldownTimer;
        // Move the player to provided Vector 3

                    isMovingAnim = true;

        //transform.position = Vector3.MoveTowards(transform.position, movePosition, moveSpeed * Time.deltaTime);

        //if (doPull)
        //{
        //    pullTarget.transform.position = transform.position;
        //}
        //transform.position = movePosition;
    }

    void PhysicsCheck()
    {

        targetTile = transform.position + Vector3.up * -1;

        targetData = CheckBlock(targetTile);


        // Convert this into a loop. 
        if (transform.position.y > fallLimit && (targetData == null || targetData.isSolid == false))
        {
            Debug.Log(gameObject + " is Falling");
            MoveToTile(targetTile);
        }
        else if (transform.position.y <= fallLimit)
        {
            sceneManager.Respawn();
        }
    }
}
