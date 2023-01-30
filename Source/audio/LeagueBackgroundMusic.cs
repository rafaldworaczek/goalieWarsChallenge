using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;

public class LeagueBackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        if (Globals.leagueBackground == null)
        {
            Globals.leagueBackground = transform.gameObject;
            DontDestroyOnLoad(transform.gameObject);
            audioSource = GetComponent<AudioSource>();
        } else
        {
            Destroy(transform.gameObject);
        }

        Globals.leagueBackground.GetComponent<LeagueBackgroundMusic>().stop();
    }

    public void play()
    {
        audioSource.volume = 0.5f;
        //if (Globals.audioMute)
        //    AudioListener.volume = 0f;
        //else
        //    AudioListener.volume = 1f;

        if (audioSource.isPlaying)
            return;

        audioSource.Play();
    }

    public void stop()
    {
        audioSource.Stop();
    }
}
