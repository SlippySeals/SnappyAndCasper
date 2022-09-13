using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerType { none = 0, snappy = 1, casper = 2, both = 3 };
public class PlayerManager : MonoBehaviour
{
    public playerType activePlayer;
    public playerType prevActive;
    private InputRouter playerInput;
    public bool canSwitch;
    [SerializeField] private bool banSwitching;
    public bool canMove;
    public GameObject activePlayerRef;

    public bool wearHat;

    [SerializeField] private GameObject playerBoth;
    [SerializeField] private GameObject playerSnappy;
    [SerializeField] private GameObject playerCasper;

    private bool buttonUp;

    // Start is called before the first frame update
    void Awake()
    {
        ChangeActivePlayer(activePlayer);
        prevActive = activePlayer;
        playerInput = FindObjectOfType<InputRouter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (banSwitching)
        {
            canSwitch = false;
        }

        if (playerInput.doAttack && buttonUp)
        {
            buttonUp = false;
            if (canSwitch)
            {
                prevActive = activePlayer;
                switch (activePlayer)
                {
                    case playerType.snappy:
                        ChangeActivePlayer(playerType.casper);
                        break;
                    case playerType.casper:
                        ChangeActivePlayer(playerType.snappy);
                        break;
                    case playerType.both:
                        TrySplitPlayer();
                        break;
                    default:
                        ChangeActivePlayer(playerType.both);
                        break;
                }
            }
        }

        if (buttonUp == false)
        {
            if (playerInput.doAttack == false)
            {
                buttonUp = true;
            }
        }
    }

    public void ChangeActivePlayer(playerType newType)
    {
        Debug.Log("SWITCH PLAYER");
        playerBoth.GetComponent<PlayerMovement>().isPulling = false;
        playerSnappy.GetComponent<PlayerMovement>().isPulling = false;
        playerCasper.GetComponent<PlayerMovement>().isPulling = false;

        playerBoth.GetComponent<PlayerMovement>().pullTarget = null;
        playerSnappy.GetComponent<PlayerMovement>().pullTarget = null;
        playerCasper.GetComponent<PlayerMovement>().pullTarget = null;

        activePlayer = newType;

        switch (newType)
        {
            case playerType.none:
                activePlayerRef = null;
                break;
            case playerType.snappy:
                activePlayerRef = playerSnappy;
                break;
            case playerType.casper:
                activePlayerRef = playerCasper;
                break;
            case playerType.both:
                activePlayerRef = playerBoth;
                break;
            default:
                activePlayerRef = playerBoth;
                break;
        }
        SwitchEffect playerFX = activePlayerRef.GetComponent<SwitchEffect>();
        if (playerFX)
        {
            playerFX.ActivateParticles();
        }
    }

    public void TrySplitPlayer()
    {
        Debug.Log("SPLIT PLAEYER");
        PlayerMovement pm = playerBoth.GetComponent<PlayerMovement>();
        if (pm.IsForwardClear())
        {
            ChangeActivePlayer(playerType.casper);
            SpawnCasper();
        }
        else
        {
            Debug.Log("FAILED TO SPAWN");
        }
    }

    public void SpawnCasper()
    {
        playerSnappy.transform.position = playerBoth.transform.position;
        playerSnappy.transform.rotation = playerBoth.transform.rotation;
        playerCasper.transform.position = playerBoth.transform.position + playerBoth.transform.forward;
        playerCasper.transform.rotation = playerBoth.transform.rotation;
        playerBoth.SetActive(false);
        playerSnappy.SetActive(true);
        playerCasper.SetActive(true);
    }

    public void MergePlayer(Transform trans)
    {
        playerBoth.transform.position = trans.position;
        playerBoth.transform.rotation = trans.rotation;
        playerSnappy.SetActive(false);
        playerCasper.SetActive(false);
        playerBoth.SetActive(true);
        activePlayer = playerType.both;
        activePlayerRef = playerBoth;
        SwitchEffect playerFX = activePlayerRef.GetComponent<SwitchEffect>();
        if (playerFX)
        {
            playerFX.ActivateParticles();
        }
    }

    public void InteractCooldownActivePlayer()
    {
        switch (activePlayer)
        {
            case playerType.snappy:
                playerSnappy.GetComponent<PlayerMovement>().ApplyInteractCooldown();
                break;
            case playerType.casper:
                playerCasper.GetComponent<PlayerMovement>().ApplyInteractCooldown();
                break;
            case playerType.both:
                playerBoth.GetComponent<PlayerMovement>().ApplyInteractCooldown();
                break;
            case playerType.none:
            default:
                break;
        }
    }
}
