using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitControl : MonoBehaviour
{
    public UnityEvent ReachPortal;

    public bool portalActive;
    [SerializeField] private SceneController sceneCon;
    [SerializeField] private GameObject portalTexture;

    // IF THIS IS TRUE THE PORTAL IS OPEN!
    public bool isPortalOpen;

    private void Start()
    {
        if (sceneCon == null)
        {
            sceneCon = FindObjectOfType<SceneController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (portalActive && pm.isSnappy && pm.isCasper)
        {
            Debug.Log("Game End");
            ReachPortal.Invoke();
        }
    }

    private void Update()
    {
        if (isPortalOpen)
        {
            portalActive = true;
        }
        EnablePortal();
    }
    public void EnablePortal()
    {
        if (portalActive)
        {
            portalTexture.SetActive(true);
        }
        else
        {
            portalTexture.SetActive(false);
        }
    }

    public void SetPortalActive()
    {
        isPortalOpen = true;
    }
}
