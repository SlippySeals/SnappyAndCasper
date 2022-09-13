using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
    private Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);

    private bool isBusy;
    private int direction;
    [SerializeField] float speed;
    private Quaternion targetRotation;

    private float yRotation;
    private float currentRotation;
    private void Start()
    {
        isBusy = false;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (isBusy)
        {
            currentRotation += yRotation * speed * Time.deltaTime;
        }

        if (Mathf.Abs(currentRotation) >= Mathf.Abs(yRotation))
        {
            // stop the rotation
            isBusy = false;
            currentRotation = 0;
            yRotation = 0;
        }
        else
        {
            transform.RotateAround(target, Vector3.up, yRotation * speed * Time.deltaTime);
        }

        
        // Spin the object around the world origin at 20 degrees/second.
        //transform.RotateAround(target, Vector3.up, 90 * Time.deltaTime);

    }

    public void RotateCamera(int dir)
    {
        direction = dir;
        if (isBusy)
        {
            return;
        }
        if (dir == -1)
        {

            yRotation += 90;

            //transform.RotateAround(target, Vector3.up, 90);
        }
        if (dir == 1)
        {
            yRotation += -90;
            //transform.RotateAround(target, Vector3.up, -90);
        }
        isBusy = true;
    }
}
