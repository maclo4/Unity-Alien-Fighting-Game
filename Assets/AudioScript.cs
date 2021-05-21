using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource swoosh;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayAudio()
    {
        swoosh.Play();
    }
}
