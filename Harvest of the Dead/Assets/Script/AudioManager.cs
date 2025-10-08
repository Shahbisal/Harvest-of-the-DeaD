using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour

{

    public Boolean isMuted = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleAudio()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            AudioListener.pause = true;
        }
        else
        {
            AudioListener.pause = false;
        }

    }
}