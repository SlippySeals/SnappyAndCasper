using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string objectName;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool doBob;

    private Vector3 startPosition;

    [SerializeField] private float frequency = 5f;
    [SerializeField] private float magnitude = 5f;
    [SerializeField] private float offset = 0f;

    [SerializeField] private bool doPickUpEvent;
    [SerializeField] public UnityEvent onPickUpEvent;

    [SerializeField] GameObject vfx;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        RotateCollectible();
        if (doBob)
        {
            BobCollectible();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if(pm == null)
        {
            return;
        }

        if (!pm.canCollect)
        {
            return;
        }

        if (objectName == "hat")
        {

            FindObjectOfType<PlayerManager>().wearHat = true;
        }

        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            if (doPickUpEvent)
            {
                onPickUpEvent.Invoke();
            }
            playerInventory.ItemCollected(objectName);
            if (vfx != null)
            {
                Instantiate(vfx, transform.position, Quaternion.identity);
            }
            gameObject.SetActive(false);
        }
    }

    private void RotateCollectible()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
    }

    private void BobCollectible()
    {
        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
    }


}
