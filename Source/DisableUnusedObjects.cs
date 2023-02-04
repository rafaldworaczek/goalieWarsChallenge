using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GlobalsNS;

public class DisableUnusedObjects : MonoBehaviour
{
    public GameObject[] goalDownCrossLine;
    public GameObject[] goalUpCrossLine;
    public GameObject[] buidlingBehindGoals;
    // Start is called before the first frame update
    void Awake()

    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Globals.PITCHTYPE.Equals("STREET")) {
                Destroy(buidlingBehindGoals[0]);
                return;
            }

            for (int i = 0; i < goalUpCrossLine.Length; i++)
            {
                Destroy(goalUpCrossLine[i]);
            }
        } else
        {
            if (Globals.PITCHTYPE.Equals("STREET"))
            {
                Destroy(buidlingBehindGoals[1]);
                return;
            }

            for (int i = 0; i < goalDownCrossLine.Length; i++)
            {
                Destroy(goalDownCrossLine[i]);
            }
        }
    }
}
