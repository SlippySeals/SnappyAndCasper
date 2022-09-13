using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEffect : MonoBehaviour
{

    [SerializeField] private float defaultTime;
    private float timer;

    [SerializeField] private GameObject particles;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if(particles != null)
            particles.SetActive(false);
        }
    }

    public void ActivateParticles()
    {
        particles.SetActive(false);
        particles.SetActive(true);
        timer = defaultTime;
    }
}
