using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using UnityEngine.UI;

public class audioMute : MonoBehaviour
{
    public RawImage[] audioMuteImg;
    private AudioListener audioListener;

    void Awake()
    {
        setDefault();
    }

    public void muteUnmuteAudio()                                          
    {
        bool audioMute = Globals.audioMute;
 
        //if (AudioListener)
        //{
            if (audioMute)
            {
                AudioListener.volume = 1f;
                
                 for (int i = 0; i < audioMuteImg.Length; i++)
                    audioMuteImg[i].texture = Resources.Load<Texture2D>("others/audioMuteOn");
            }
            else
            {
                AudioListener.volume = 0f;              
                for (int i = 0; i < audioMuteImg.Length; i++) {
                    audioMuteImg[i].texture = Resources.Load<Texture2D>("others/audioMuteOff");
                }
            }

            Globals.audioMute = !audioMute;

            if (Globals.audioMute == true)
                PlayerPrefs.SetInt("audioMute", 1);
            else
                PlayerPrefs.SetInt("audioMute", 0);
            PlayerPrefs.Save();
    }

    private void setDefault()
    {
        bool audioMute = Globals.audioMute;
  
        if (audioMute)
        {
            AudioListener.volume = 0f;
            for (int i = 0; i < audioMuteImg.Length; i++)
            {
                audioMuteImg[i].texture = Resources.Load<Texture2D>("others/audioMuteOff");
            }           
        }
        else
        {
            AudioListener.volume = 1f;
            for (int i = 0; i < audioMuteImg.Length; i++)
            {
                audioMuteImg[i].texture = Resources.Load<Texture2D>("others/audioMuteOn");
            }
        }
    }
}
