using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//This class serves as the audio information for a sound clip
//This will be called by the audio manager to play the sound with the specified information
[System.Serializable]
public class Sound {

    public string name;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
