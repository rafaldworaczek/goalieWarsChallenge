using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisableUnusedObjects : MonoBehaviour
{
    public GameObject[] goalDownCrossLine;
    public GameObject[] goalUpCrossLine;

    // Start is called before the first frame update
    void Awake()

    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < goalUpCrossLine.Length; i++)
            {
                Destroy(goalUpCrossLine[i]);
            }
        } else
        {
            for (int i = 0; i < goalDownCrossLine.Length; i++)
            {
                Destroy(goalDownCrossLine[i]);
            }
        }
    }
}
