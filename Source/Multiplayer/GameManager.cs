using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalsNS;
using Photon.Pun;
using Photon.Realtime;


namespace Com.Osystems.GoalieStrikerFootball
{

    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Photon Callbacks
        public static GameObject player1;
        public static GameObject player2;
        public static GameObject ball;

        void Start()
        {
            Debug.Log("AwakeSCENENAME:" + SceneManager.GetActiveScene().name
                + " PhotonNetwork.IsConnected " + PhotonNetwork.IsConnected);


            if (!PhotonNetwork.IsConnected) // 1
            {
                //SceneManager.LoadScene("Launcher");
                return;
            }

            //if (controllerRigid.LocalPlayerInstance == null)
            //{         
                
            //}

        
            // 4
            print("PHOTONDBG1 ball create");

            //ball.name = "Ball";
            //}
            //else // 5
            //{
            //    player1 = PhotonNetwork.Instantiate("player1",
            //        new Vector3(0,0, 12),
            //        Quaternion.identity, 0);
            //}
        }

        void Awake()
        {
            if (!PhotonNetwork.IsConnected)
            {
                return;
            }

            print("PHOTONDBG1 GAME MANAGER ENTERED " + gameObject.name);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("PHOTONDBG1 Instantiating Player 1");
                player1 = PhotonNetwork.Instantiate("player1",
                    new Vector3(0f, 0.03f, -10),
                    Quaternion.identity, 0);

                //ball = PhotonNetwork.Instantiate("ball",
                //new Vector3(0, 0, -3),
                //Quaternion.identity, 0);
            }
            else
            {
                Debug.Log("PHOTONDBG1 Instantiating Player 2");
                player1 = PhotonNetwork.Instantiate("player2",
                        new Vector3(0f, 0.03f, 10),
                        //Quaternion.identity, 0);
                        Quaternion.Euler(0, 180f, 0), 0);
            }

            if (Globals.player1MainScript == null)
                Globals.player1MainScript = player1.GetComponent<playerControllerMultiplayer>();
            else
                Globals.player2MainScript = player1.GetComponent<playerControllerMultiplayer>();
        }

        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            print("SPAWN gameObject.name " + gameObject.name);
            if (!PhotonNetwork.IsMasterClient)
            {
                
            }
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            //PhotonNetwork.DestroyAll();
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        /* Called when the local player left the room. We need to load the launcher scene. */
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("multiplayerMenu");
        }
        #endregion

        public void LeaveRoom()
        {
            return;
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            return;
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                //LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            return;
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                //LoadArena();
            }
        }

    }
}