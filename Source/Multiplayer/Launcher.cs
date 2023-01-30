using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using GlobalsNS;
using LANGUAGE_NS;
using AudioManagerMultiNS;

namespace Com.Osystems.GoalieStrikerFootball
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        /*This client's version number. Users are separated from each other
          by gameVersion (which allows you to make breaking changes). */
        string gameVersion = "1";

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 2;
        public GameObject spinner;
        //private GameObject controlPanel;
        //private GameObject progressLabel;
        bool isConnecting;
        PhotonView photonView;
        string roomName = "test1";
        bool updateTextureConfirm = false;
        public GameObject blockingCanvas;
        public MultiplayerMenu multiplayerMenu;
        public float connectStartTime = 0f;
        public bool isConnectionActive = false;
        public teamManagement TeamManagement;
        private AudioManager audioManager;
        private bool isEnergyChange = false;
        public gameSettings GameSettings;
        private float maxWaitTime = 15f;

        [PunRPC]
        void RPC_textureUpdateConfirmed()
        {
            updateTextureConfirm = true;
            spinner.SetActive(false);
        }

        [PunRPC]
        void RPC_updateGameSettings(int teamAid,
                                    string teamAname,
                                    string leagueAName,
                                    string playerADesc,
                                    string teamADesc,
                                    byte effectNumber,
                                    int teamGkStrength,
                                    int teamAttackStrength,
                                    string stadiumColor,
                                    float stoppageTime,
                                    int energy)
        {
            //Debug.Log("Globals.teamAAttackStrength received  " + teamADesc
            //    + " leagueAName " + leagueAName
            //    + " playerBDesc " + playerADesc
            //    + " teamGkStrength " + teamGkStrength
           //     + " teamBAttackStrength " + teamAttackStrength
            //    + " energy " + energy);

            Globals.teamBid = teamAid;
            Globals.teamBname = teamAname;
            Globals.teamBleague = leagueAName;
            Globals.playerBDesc = playerADesc;
            Globals.teamBDesc = teamADesc;
            if (!PhotonNetwork.IsMasterClient)
            {
                Globals.effectNumber = effectNumber;
                Globals.stadiumColorTeamA = stadiumColor;
                Globals.stoppageTime = stoppageTime;
            }

            Globals.teamBCustomize = false;
            Globals.teamBGkStrength = teamGkStrength;
            Globals.teamBAttackStrength = teamAttackStrength;

            Globals.teamBcumulativeStrength =
            Globals.teamBGkStrength + Globals.teamBAttackStrength;
            Globals.energyPlayerB = energy;

            /*print("Update arrived teamId " + Globals.teamBid
                + " Globals.teamBname " + Globals.teamBname
                + " Globals.teamBleague " + Globals.teamBleague
                + " Globals.playerBDesc " + Globals.playerBDesc);*/

            photonView.RPC("RPC_textureUpdateConfirmed",
                            RpcTarget.Others);
        }

        [PunRPC]
        void RPC_updateGameSettingsCustomize(int teamAid,
                                             string teamAname,
                                             string leagueAName,
                                             string playerADesc,
                                             byte effectNumber,
                                             string customizeTeamName,
                                             string customizePlayerName,
                                             string customizePlayerNationality,
                                             string customizePlayerShirt,
                                             string customizePlayerShorts,
                                             string customizePlayerSocks,
                                             string customizePlayerSkinHair,
                                             string customizePlayerGloves,
                                             string customizePlayerShoe,
                                             string customizePlayer,
                                             string customizeFansColor,
                                             string customizeFlagColor,
                                             string customizePlayerCardName,
                                             int teamGkStrength,
                                             int teamBAttackStrength,
                                             string stadiumColor,
                                             float stoppageTime,
                                             int energy)
        {
            Globals.teamBid = teamAid;
            Globals.teamBname = teamAname;
            Globals.teamBleague = leagueAName;
            Globals.playerBDesc = playerADesc;
            if (!PhotonNetwork.IsMasterClient)
            {
                Globals.effectNumber = effectNumber;
                Globals.stadiumColorTeamA =
                    customizeFansColor + "|"+ customizeFlagColor + "|" + customizeFlagColor;
                Globals.stoppageTime = stoppageTime;
            }
            Globals.teamBCustomize = true;
            Globals.teamBGkStrength = teamGkStrength;
            Globals.teamBAttackStrength = teamBAttackStrength;
            Globals.teamBcumulativeStrength =
            Globals.teamBGkStrength + Globals.teamBAttackStrength;
            Globals.energyPlayerB = energy;

            Globals.teamBDesc = customizePlayerShirt + "|" +
                                customizePlayerShorts + "|" +
                                customizePlayerSocks + "|" +
                                customizePlayerSkinHair + "|" +
                                customizePlayerGloves + "|" +
                                customizePlayerShoe + "|" +
                                customizeFansColor + "|" +
                                customizeFlagColor;

            /*Globals.customizeTeamName = customizeTeamName;
            Globals.customizePlayerName = customizePlayerName;
            Globals.customizePlayerNationality = customizePlayerNationality;
            Globals.customizePlayerShirt = customizePlayerShirt;
            Globals.customizePlayerShorts = customizePlayerShorts;
            Globals.customizePlayerSocks = customizePlayerSocks;
            Globals.customizePlayerSkinHair = customizePlayerSkinHair;
            Globals.customizePlayerGloves = customizePlayerGloves;
            Globals.customizePlayerShoe = customizePlayerShoe;
            Globals.customizePlayer = customizePlayer;
            Globals.customizeFansColor = customizeFansColor;
            Globals.customizeFlagColor = customizeFlagColor;
            Globals.customizePlayerCardName = customizePlayerCardName;*/

            /*print("Update arrived teamId " + Globals.teamBid
                + " Globals.teamBname " + Globals.teamBname
                + " Globals.teamBleague " + Globals.teamBleague
                + " Globals.playerBDesc " + Globals.playerBDesc);*/

            photonView.RPC("RPC_textureUpdateConfirmed",
                            RpcTarget.Others);
        }

        void init()
        //void Awake()
        {
            Globals.isMultiplayerMatchNotFound = false;
            Globals.isMultiplayerFriendConActive = false;

            maxWaitTime = UnityEngine.Random.Range(8f, 12f);
            //maxWaitTime = 1f;
            isEnergyChange = false;
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

            Time.timeScale = 1f;

            cleanUp();
            //UnityEngine.Random.Range(4, 10);
            //ConnectToPhoton();

            print("Launche START ########");
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = false;
            spinner.SetActive(false);
            photonView = GetComponent<PhotonView>();

            Globals.effectNumber = setTexturesMulti.playWheatherEffect();
        }

        /*
         MonoBehaviour method called on GameObject by Unity during initialization phase.
        */
        void Start()
        {
            //progressLabel.SetActive(false);
            //controlPanel.SetActive(true);           
        }

        private bool notInit = false;
        private bool loadingSceneActive = false;
        private void Update()
        {
            if (loadingSceneActive)
                return;
      
            if (!notInit)
            {
                init();
                notInit = true;
            }

            if (isConnectionActive &&
               (Time.time - connectStartTime > 60f)) 
            {
                multiplayerMenu.showInfoCanvas(
                    "Oops..",
                    Languages.getTranslate("Sorry. We can't find any opponent now"),
                    "error/error");
                multiplayerMenu.setBottomButtonInteractable(true);
                multiplayerMenu.setBottomButtonsAlpha(1f);
                spinner.SetActive(false);
                connectStartTime = Time.time;
                isConnectionActive = false;
                Globals.isMultiplayerFriendConActive = false;
                cleanUp();
                return;
            }

            if (!Globals.isMultiplayerFriendConActive &&
                isConnectionActive &&
                ((Time.time - connectStartTime) > maxWaitTime) &&
                 PhotonNetwork.IsConnected &&
                 (PhotonNetwork.CurrentRoom != null &&
                  PhotonNetwork.CurrentRoom.PlayerCount == 1))
            {
                loadingSceneActive = true;
                StartCoroutine(loadNormalGame(UnityEngine.Random.Range(3.5f, 4.5f)));        
                //PhotonNetwork.LoadLevel("gameSceneMultiplayer_Room_2");
            }
        }

        public IEnumerator loadNormalGame(float delay) {

            PhotonNetwork.AutomaticallySyncScene = false;

            GameSettings.getRandomTeamBAndFill();
            spinner.SetActive(false);

            yield return new WaitForSeconds(delay);

            Globals.isMultiplayerMatchNotFound = true;
            Globals.onlyTrainingActive = false;
            Globals.isTrainingActive = false;
            Globals.isBonusActive = false;
            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom();

            Globals.loadSceneWithBarLoader("gameScene");
        }

        public void ConnectToPhoton()
        {
            PhotonNetwork.GameVersion = gameVersion; //1
            PhotonNetwork.ConnectUsingSettings(); //2
            isConnecting = true;
        }

        /* Start the connection process.
         - If already connected, we attempt joining a random room
         - if not yet connected, Connect this application instance to Photon Cloud Network
        */
        public void Connect(string actionType, string roomName)
        {
            //blockingCanvas.SetActive(true);
            connectStartTime = Time.time;
            //progressLabel.SetActive(true);
            //controlPanel.SetActive(false);
            //print("PUN CONNECT");
            cleanUp();
            isConnectionActive = true;
            spinner.SetActive(true);

            StartCoroutine(initPhotonConnection(actionType, roomName));
           
            return;    
        }

        public IEnumerator initPhotonConnection(string actionType, string roomName)
        {

            for (int i = 0; i < 1000; i++) {
                cleanUp();
                if (PhotonNetwork.IsConnected ||
                    PhotonNetwork.InRoom ||
                    ((PhotonNetwork.CurrentRoom != null) && (PhotonNetwork.CurrentRoom.PlayerCount != 0)))
                {
                    yield return null;
                }
                else
                    break;
            }

            if (PhotonNetwork.IsConnected)
            {
                //TODO
                PhotonNetwork.LocalPlayer.NickName = "goaliewarsfootball_kqp23#xdj34#-2022"; //1
                Debug.Log("PhotonNetwork.IsConnected! | Trying to JOIN Room ");
                if (actionType.Equals("random"))
                {
                    PhotonNetwork.JoinRandomRoom();
                } else
                {
                    PhotonNetwork.JoinRoom(roomName);
                }

                //inRandomOrCreateRoom.JoinRandomOrCreateRoom
                //TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default); //3
                ///PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby); //4
                ///PhotonNetwork.JoinOrCreateRoom(null, roomOptions, null); //4
            }
            else
            {
                ConnectToPhoton();
            }
        }

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                connectStartTime = Time.time;
                Debug.Log(" OnConnectedToMaster() ");
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()

                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public void LoadGame()
        {
            // 5
            /*NULL POINTER HERE*/
            print("PhotonNetwork.CurrentRoom.PlayerCount Loadgame " + PhotonNetwork.CurrentRoom.PlayerCount);
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                connectStartTime = Time.time;

                print("PhotonNetwork.CurrentRoom.PlayerCount load level");
                //PhotonNetwork.LoadLevel("gameSceneMultiplayer_Room_2");
           
                StartCoroutine(loadMainGame(0.5f));
                //PhotonNetwork.LoadLevel("testScene");
            }
            else
            {

            }
        }

        public IEnumerator loadMainGame(float delay)
        {
            ///if (photonView.IsMine)
            StartCoroutine(syncPlayerTextures(0.7f));

            for (int i = 0; i <= 1000; i++)
            {
                if (!updateTextureConfirm)
                    yield return new WaitForSeconds(delay);
                else
                    break;
            }

            spinner.SetActive(false);
            yield return new WaitForSeconds(1.5f);

          
            /*if (!Globals.outstandingEnergyName.Equals("EMPTY"))
            {
                PlayerPrefs.SetInt(Globals.outstandingEnergyName, Globals.outstandingEnergyVal);
                PlayerPrefs.Save();
                Globals.outstandingEnergyName = "EMPTY";
            }*/

            PhotonNetwork.AutomaticallySyncScene = true;
            if (!PhotonNetwork.IsMasterClient)
            {
                //Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                yield break;
            }

            print("photon load scene");
            //Debug.LogError("PhotonNetwork : Trying to Load a  master Client");
            PhotonNetwork.LoadLevel("gameSceneMultiplayer_Room_2");
            //yield return 0;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("player join");
            Debug.Log(newPlayer.ToStringFull());
            LoadGame(); 
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            ///progressLabel.SetActive(false);
            //ontrolPanel.SetActive(true);
            PhotonNetwork.Disconnect();
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            isConnecting = false;
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(PhotonChatController.roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }


        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            print("OnJoinRandomFailed create rooom");
            //RoomOptions room = new RoomOptions();
            ///room.PublishUserId = true;
            //room.MaxPlayers = maxPlayersPerRoom;

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
            return;
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        //public override void OnConnected()
        //{
        //    base.OnConnected();    
        // }

        // Photon Methods
        public override void OnConnected()
        {
            print("onConnected");
            // 1
            base.OnConnected();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() ### called by PUN. Now this client is in a room. PlayerCount " +
                PhotonNetwork.CurrentRoom.PlayerCount);

            // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            print("#PHOTONDBG123 PhotonNetwork.CurrentRoom.PlayerCount " +
                PhotonNetwork.CurrentRoom.PlayerCount);

            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                Debug.Log("We load the 'Room for 1' ");
                LoadGame();

                // #Critical
                // Load the Room Level.
                //PhotonNetwork.LoadLevel("Room for 2");
            } 
        }
        public override void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom()");
            PhotonNetwork.Disconnect();
        }

        private void cleanUp()
        {
            isConnecting = false;

            PhotonNetwork.Disconnect();
            Globals.player1MainScript = null;
            Globals.player2MainScript = null;

            if (GameManager.player1)
                PhotonNetwork.Destroy(GameManager.player1);

            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom();

            isConnecting = false;

            Globals.teamBid = -1;
            Globals.peersReady = 0;

            Globals.stoppageTime = UnityEngine.Random.Range(4, 10);
        }

        private IEnumerator syncPlayerTextures(float delay)
        {
            if (updateTextureConfirm)
                yield return 0;

            int i = 0;
            while (i < 100)
            {
                if (photonView == null)
                {
                    yield return new WaitForSeconds(0.3f);
                    continue;
                }

                //print("send RPC_updateGameSettings " + photonView.IsMine);
                if (!Globals.isTeamCustomize(Globals.teamAname))
                {
                    photonView.RPC("RPC_updateGameSettings",
                                    RpcTarget.Others,
                                    Globals.teamAid,
                                    Globals.teamAname,
                                    Globals.leagueName,
                                    Globals.playerADesc,
                                    Globals.teamADesc,
                                    Globals.effectNumber,
                                    Globals.teamAGkStrength,
                                    Globals.teamAAttackStrength,
                                    Globals.stadiumColorTeamA,
                                    Globals.stoppageTime,
                                    Globals.energyPlayerA);
                    //print("Globals.teamAAttackStrength send " + Globals.teamAname
                    //    + " Globals.teamAGkStrength " + Globals.teamAGkStrength
                    //    + " Globals.teamAAttackStrength " + Globals.teamAAttackStrength);

                } else
                {
                    photonView.RPC("RPC_updateGameSettingsCustomize",
                         RpcTarget.Others,
                         Globals.teamAid,
                         Globals.customizeTeamName,
                         Globals.leagueName,
                         Globals.playerADesc,
                         Globals.effectNumber,
                         Globals.customizeTeamName,
                         Globals.customizePlayerName,
                         Globals.customizePlayerNationality,
                         Globals.customizePlayerShirt,
                         Globals.customizePlayerShorts,
                         Globals.customizePlayerSocks,
                         Globals.customizePlayerSkinHair,
                         Globals.customizePlayerGloves,
                         Globals.customizePlayerShoe,
                         Globals.customizePlayer,
                         Globals.customizeFansColor,
                         Globals.customizeFlagColor,
                         Globals.customizePlayerCardName,
                         Globals.teamAGkStrength,
                         Globals.teamAAttackStrength,
                         Globals.stadiumColorTeamA,
                         Globals.stoppageTime,
                         Globals.energyPlayerA);
                }   

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
