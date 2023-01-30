using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;

public class startResolutionsSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Globals.originalScreenWidth = Screen.width;
        Globals.orignalScreenHeight = Screen.height;
    }
}
