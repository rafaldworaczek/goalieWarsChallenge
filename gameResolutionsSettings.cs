using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;
using UnityEngine.SceneManagement;

public class gameResolutionsSettings : MonoBehaviour
{
    private float HIGH_RES_MAX = 1800000f;
    //private float STANDARD_RES_MAX = 1100000f;
    private float STANDARD_RES_MAX = 1000000f;
    private float LOW_RES_MAX = 500000f;
    private float VERY_LOW_RES_MAX = 256000f;

    // Start is called before the first frame update
    void Awake()
    {
        int nativeWidth = Globals.originalScreenWidth;
        int nativeHeight = Globals.orignalScreenHeight;

        if (!Globals.loaderBarSceneName.Equals("gameScene") &&
            !SceneManager.GetActiveScene().name.Equals("multiplayerMenu"))
            return;

        /*print("nativeWidth " + nativeWidth + " nativeHeight "
            + nativeHeight + " Globals.graphicsQuality " + Globals.graphicsQuality 
            + Globals.originalScreenWidth 
            + " Globals.orignalScreenHeight " 
            + Globals.orignalScreenHeight);*/

        float pixels = (float) nativeWidth * (float) nativeHeight;
        Vector2 newRes = new Vector2((float) nativeWidth, (float) nativeHeight);

        /*take resolution as default for phone*/
        if (Globals.graphicsQuality.Equals("VERY HIGH"))
        {
            Globals.recoverOriginalResolution();
            QualitySettings.SetQualityLevel(5, true);
            return;
        }

        float maxRes = STANDARD_RES_MAX;
        //QualitySettings.SetQualityLevel(5, true);
        if (Globals.graphicsQuality.Equals("LOW"))
        {
            maxRes = LOW_RES_MAX;
            QualitySettings.SetQualityLevel(1, true);
        }
        else if (Globals.graphicsQuality.Equals("VERY LOW"))
        {
            maxRes = VERY_LOW_RES_MAX;
            QualitySettings.SetQualityLevel(0, true);
        }
        else if (Globals.graphicsQuality.Equals("HIGH"))
        {
            QualitySettings.SetQualityLevel(4, true);
            maxRes = HIGH_RES_MAX;
        }

        if (pixels > maxRes)
         {
            newRes = divRes(nativeWidth, nativeHeight, maxRes);
         } else
         {
            return;
         }

        //print("RESOLUTIONS SETUP OLD RES " + Screen.width + "x" + Screen.height + " NEWRES " + newRes);
        Screen.SetResolution((int) newRes.x, (int) newRes.y, true);
    }

    /*return new res as a vector width/height*/
    private Vector2 divRes(int width, int height, float smallerThan)
    {
        float widthFloat = (float) width;
        float heightFloat = (float) height;
        float divFactor = 1.05f;
        int loops = 0;

        float pixels = widthFloat * heightFloat;

        while (true)
        {
            if (pixels <= smallerThan ||
                loops > 200)
            {
                return new Vector2(widthFloat, heightFloat);
            }

            widthFloat /= divFactor;
            heightFloat /= divFactor;
            pixels = widthFloat * heightFloat;
            loops++;
        }
    }
}
