using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public static AudioManager instance;
    public AudioMixerGroup audioMixer;
    public bool audioEnabled = true;

    void Awake()
    {
       if(instance == null)
        {
            instance = this;
        }

       else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = audioMixer;
        }
    }

    void Start()
    {
        Play("MenuMusic");
    }

    //Play the sound by name
    public void Play(string name)
    {
        if(audioEnabled)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null)
            {
                s.source.volume = 0.15f;
                s.source.Play();
            }
        }

        else
        {
            return;
        }
        
        
    }

    public void EnableAudio()
    {
        if (audioEnabled == false)
        {
            PauseMusic();
        }

        if (audioEnabled == true)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            if (sceneName == "_Menu")
            {
                Play("MenuMusic");
            }

            else
            {
                Play("CombatMusic");
            }

        }
    }

    //Use this when calling music as only one song should be playing at a time
    public void PlayMusic(string songToPlay)
    {

    }

    //This is used for sounds, as we can have more than one at a time
    public void PlaySounds()
    {

    }

    public void StopMusic()
    {
        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void PauseMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s != null)
                s.source.Pause();

            else
                return;
        }
    }

    public IEnumerator FadeOut(string audioName, float timeToFade)
    {
        while(timeToFade > 0)
        {
            foreach (Sound s in sounds)
            {
                if(s.name == audioName)
                {
                    s.source.volume -= 0.1f;
                }               
            }

            yield return new WaitForSeconds(timeToFade);
            break;
        }

        //After the music has faded, pause it
        PauseMusic();
    }


	
}
