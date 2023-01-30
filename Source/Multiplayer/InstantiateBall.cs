using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Photon.Pun;
using Com.Osystems.GoalieStrikerFootball;

public class InstantiateBall : MonoBehaviour, IPunInstantiateMagicCallback
{
    public void OnPhotonInstantiate(Photon.Pun.PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            GameManager.ball = gameObject;
            print("DBGBALLGAMEOBJECT SET POINTER " + GameManager.ball);
        }
    }
}
