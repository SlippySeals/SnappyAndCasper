using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] AudioSource mySource;
    [SerializeField] AudioClip spring;
    [SerializeField] AudioClip walk;
    [SerializeField] AudioClip rotate;
    [SerializeField] AudioClip collide;

    // Start is called before the first frame update
    void Start()
    {
        if (!mySource)
        {
            mySource = GetComponent<AudioSource>();
        }
    }

    public void PlaySoundEffect(string soundName)
    {
        switch (soundName)
        {
            default:
                break;
            case "spring":
                if (spring)
                {
                    mySource.PlayOneShot(spring);
                }
                break;
            case "walk":
                if (walk)
                {
                    mySource.PlayOneShot(walk);
                }
                break;
            case "rotate":
                if (rotate)
                {
                    mySource.PlayOneShot(rotate);
                }
                break;
            case "collide":
                if (collide)
                {
                    mySource.PlayOneShot(collide);
                }
                break;
        }
    }
}
