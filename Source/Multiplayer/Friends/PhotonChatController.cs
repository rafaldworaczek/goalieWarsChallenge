using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;
using GlobalsNS;
using LANGUAGE_NS;
using Com.Osystems.GoalieStrikerFootball;
using AudioManagerMultiNS;
using System.Text.RegularExpressions;

public class PhotonChatController : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    private string nickName = "";
    private int listNumRows = 6;
    private int gameLoadPageIdx = 0;
    public TextMeshProUGUI usernameID;
    public GameObject[] ListGameButtonGO;
    public GameObject[] listDeleteButtonGO;
    public TextMeshProUGUI[] ListText;
    public TextMeshProUGUI listHeaderText;
    public Button[] listDeleteButton;
    public Button[] listAcceptButton;
    public RawImage[] listAcceptImage;
    public RawImage[] listAcceptButtonImage;

    public GameObject[] listIsOnlineGO;
    public RawImage[] listIsOnlineImage;

    public GameObject invitePartyPanel;
    private List<string> friendList;
    private List<string> friendInvities;
    private List<string> gameInviteList;
    private string friendListFileName = "ONLINE_FRIENDS_LIST";
    private string friendInvitiesListFileName = "ONLINE_FRIENDS_REQUEST_LIST";
    private string gameInvitesFileName = "ONLINE_MESSAGES";
    public InputField inputFriendUserId;
    private string lastUserIdText = "";
    public GameObject friendInvitiesTextGO;
    public GameObject gameInvitiesTextGO;
    public GameObject invitePartMessageTextGO;
    string username;
    public GameObject userIdInfoPanel;
    public GameObject listPanel;
    public GameObject sendFriendRequestPanel;
    public GameObject yesNoMenuPanel;
    public Button yesNoMenuNoAnswer;
    public Button yesNoMenuYesAnswer;
    public TextMeshProUGUI yesNoMenuHeaderText;
    private int activeMenu = -1;
    public static string roomName;
    public InputField inputNickname;
    public GameObject nicknamePanel;
    public Launcher launcher;
    private AudioManager audioManager;
    public GameObject oopsPanel;
    public TextMeshProUGUI oopsPanelInfoText;
    private GameObject admobGameObject;
    private admobAdsScript admobAdsScript;
    public RawImage[] onlineStatusImg;
    private Dictionary<string, int> userStatus;
    private string lastInviteToRoomMsg = String.Empty;
    // Start is called before the first frame update

    void Start()
    {
   
        admobGameObject = GameObject.Find("admobAdsGameObject");
        admobAdsScript = admobGameObject.GetComponent<admobAdsScript>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if (Globals.coins < Globals.MIN_COINS_PLAY_ONLINE)
            return;


        initLists();
  
        username = PlayerPrefs.GetString("ONLINE_USER" +
            "NAME");
        roomName = Globals.getRandomStr(16);

        ///PlayerPrefs.SetString(friendListFileName, "zbychu:xdfds|rafal2:a|rafal3:a|rafal4:a|rafal5:a|rafal6:a|rafal7:a|rafal8:a");
        //PlayerPrefs.SetString(friendInvitiesListFileName, "zbychu:xdfds|rafal2:a|rafal3:a|rafal4:a|rafal5:a|rafal6:a|rafal7:a");
        //PlayerPrefs.SetString(gameInvitesFileName, "zbychu:xdfds:xfgbgfa|rafal2:a:b|rafal3:a:b|rafal4:a:b|" +
        //    "rafal5:a:b|rafal6:a:b|rafal7:a:b|rafal8:a:b");
        //PlayerPrefs.Save();

        initUIObjects();

        ///chatClient.ChatRegion = "eu";
        //////ConnectToPhotonChat();
        nickName = PlayerPrefs.GetString("ONLINE_NICKNAME");
        //onClickShowFriends();

        //print("DBGPHOTONCHAT START executed ");
    }

    void Update()
    {
        if (Globals.coins < Globals.MIN_COINS_PLAY_ONLINE)
            return;

        //print("send message");
        if (chatClient != null)
        {
            chatClient.Service();
        }

        /*Debug.Log("DBGPHOTONCHAT chat status Client state FrontendAddress " + chatClient.FrontendAddress
            + " canChat " + chatClient.CanChat
            + " ChatRegion " + chatClient.ChatRegion
            + " chatPeer " + chatClient.chatPeer
            + " chatNameServer " + chatClient.NameServerAddress);
        print("DBGPHOTONCHAT chat status Client state FrontendAddress " + chatClient.FrontendAddress
          + " canChat " + chatClient.CanChat
          + " ChatRegion " + chatClient.ChatRegion
          + " chatPeer " + chatClient.chatPeer
          + " chatNameServer " + chatClient.NameServerAddress);*/
    }

    private void ConnectToPhotonChat()
    {
        //print("DBGPHOTONCHAT connectToPhotonChat");
        chatClient = new ChatClient(this);
        chatClient.UseBackgroundWorkerForSending = true;
        //chatClient.AuthValues = new AuthenticationValues(username);
        //chatClient.ConnectUsingSettings(PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings());

        bool ret = chatClient.Connect(
                    PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
                    //PhotonNetwork.AppVersion,
                    "goaliewarsfootball_v1",
                    new AuthenticationValues(username));
        //Debug.Log("#DBGchat client");
        /*Debug.Log("DBGPHOTONCHAT " + username + " ret " + ret + "" +
            " id " + PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat
            + " appversion " + PhotonNetwork.AppVersion 
            + " version end");*/
    }

    public void OnGetMessages(string channelName,
                              string[] senders,
                              object[] messages)
    {
        //print("#DBGPHOTONCHAT get messages message!");
    }

    public void OnPrivateMessage(string sender,
                                 object message,
                                 string channelName)
    {

        string[] msg = message.ToString().Split(':');

        //print("#DBGPHOTONCHAT message To string " + message.ToString());
        if (msg[2].Equals(username))
            return;

        if (!invitePartyPanel.activeSelf)
        {
            invitePartMessageTextGO.SetActive(true);
        }

        //A - friend accepted your invities
        if (msg[0].Equals("A")) {
            if (checkIfItemExists(friendList, msg[1] + ":" + msg[2]))
                return;

            audioManager.Play("bell1");
            friendList.Add(msg[1] + ":" + msg[2]);
            saveToPrefs(msg[1] + ":" + msg[2], friendListFileName);

            if (activeMenu == 0)
                onClickShowFriends();

        } //game invities
        else if (msg[0].Equals("IG"))
        {
            //if (checkIfItemExists(gameInviteList, msg[1] + ":" + msg[2]))
            //    return;
            //Debug.Log("#DBGINVIATEA lastgameplayed before");

            if (Globals.multiplayer_lastGameName.Equals(msg[1] + ":" + msg[2] + ":" + msg[3]))
            {
                //Debug.Log("#DBGINVIATEA lastgameplayed duplication");
                return;
            }

            audioManager.Play("bell1");

            //keep only the latest inviation from the user
            string itemFullName =
                Globals.getFullItemName(msg[1] + ":" + msg[2], gameInvitesFileName, '|');

            if (itemFullName != String.Empty) { 
                deleteListItemYes(itemFullName,
                                  gameInvitesFileName);
            }

            gameInviteList.Add(msg[1] + ":" + msg[2]);
            saveToPrefs(msg[1] + ":" + msg[2] + ":" + msg[3], gameInvitesFileName);
            //Debug.Log("DBGINVIATEA gameInvitesFileName prefs " + PlayerPrefs.GetString(gameInvitesFileName)
            //          + " itemFullName " + itemFullName + " newADD " + msg[1] + ":" + msg[2]);

            if (activeMenu == 2)
                onClickShowGameInvites();
            else
            {
                gameInvitiesTextGO.SetActive(true);
            }
        } else if (msg[0].Equals("IF"))
        //friend invities
        {
           //Debug.Log("DBGPHOTONCHAT friendInvities.Count " + friendInvities.Count + " " +
           //    " checkIfItemExists " + 
           // checkIfItemExists(friendInvities, msg[1] + ":" + msg[2]));

            if (checkIfItemExists(friendInvities, msg[1] + ":" + msg[2]) ||
                checkIfItemExists(friendList, msg[1] + ":" + msg[2]))
                return;
           
            audioManager.Play("bell1");
            friendInvities.Add(msg[1] + ":" + msg[2]);
            saveToPrefs(msg[1] + ":" + msg[2], friendInvitiesListFileName);
            if (activeMenu == 1)
                onClickShowFriendsInvities();
            else
                friendInvitiesTextGO.SetActive(true);
        }

        //Console.WriteLine("OnPrivateMessage: {0} ({1}) > {2}", channelName, sender, message);
    }
    
    private void printActiveList()
    {
        if (activeMenu == 0)
            printList(gameLoadPageIdx, friendList, 0);
        if (activeMenu == 1)
            printList(gameLoadPageIdx, friendInvities, 1);
        if (activeMenu == 2)
            printList(gameLoadPageIdx, gameInviteList, 2);
    }

    private int getMaxListIdx()
    {        
        if (activeMenu == 0)
            return (Mathf.Max(0, friendList.Count - 1) / listNumRows);
        if (activeMenu == 1)
            return (Mathf.Max(0, friendInvities.Count - 1) / listNumRows);
        if (activeMenu == 2)
            return (Mathf.Max(0, gameInviteList.Count - 1) / listNumRows);
        return 1;
    }

    private bool checkIfItemExists(List<string> list, string msg)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(msg))
                return true;
        }

        return false;
    }

    private void saveToPrefs(string value, string fileName)
    {
        string item = "";
        if (PlayerPrefs.HasKey(fileName))
        {
            item =
                PlayerPrefs.GetString(fileName);
            item += "|" + value;
        }
        else
        {
            item = value;
        }

        PlayerPrefs.SetString(fileName, item);
        PlayerPrefs.Save();
    }
   
    public void DebugReturn(DebugLevel level,
                            string message)
    {
        //print("DBGPHOTONCHAT debug " + level + " msg " + message);
    }

    private void deleteListItemYes(string itemName,
                                   string itemFileName)
    {
        Globals.deleteListItem(itemName,
                               itemFileName);

        //initGameSaves();
        yesNoMenuPanel.SetActive(false);

        if (activeMenu == 0)
            onClickShowFriends();
        else if (activeMenu == 1)
            onClickShowFriendsInvities();
        else if (activeMenu == 2)
            onClickShowGameInvites();
    }

    private void closeYesNoMenuPanel()
    {
        yesNoMenuPanel.SetActive(false);
    }

    public void OnChatStateChange(ChatState statusCode)
    {
        ///print("DBGPHOTONCHAT status code " + statusCode);
    }

    public void OnConnected()
    {
        chatClient.SetOnlineStatus(ChatUserStatus.Online); 
        // You can set your online state (without a mesage).
        //Debug.Log("DBGPHOTONCHAT connected");
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        return;

        //Debug.Log("DBGPHOTONCHAT OnApplicationPause pauseStatus " + pauseStatus 
        //    + " chatClient " + chatClient);

        if (pauseStatus)
        {
            if (chatClient != null)
            {               
               chatClient.Disconnect();
            }
            //chatClient.Disconnect();
            // app moved to background
        }

        else
        {
            if (chatClient == null)
            {
                ConnectToPhotonChat();
            }
            // app is foreground again
        }
    }

    public void OnDisconnected()
    {
        //Debug.Log("DBGPHOTONCHAT disconnected");
        //chatClient.Disconnect();
    }

    public void OnStatusUpdate(string user,
                               int status,
                               bool gotMessage,
                               object message)
    {
        userStatus[user] = status;
        //if (activeMenu == 0)
        //    printList(gameLoadPageIdx, friendList, 0);
        printActiveList();

        //Debug.Log("DBGchat get status changed " + user + " status " + status + " msg " + gotMessage + " activeMenu "
        //    + activeMenu);
    }

    public void OnSubscribed(string[] channels,
                             bool[] results)
    {
        //Debug.Log("DBGPHOTONCHAT OnSubscribed");
    }

    public void OnUnsubscribed(string[] channels)
    {
        //print("DBGPHOTONCHAT OnUnsubscribed");
    }

    public void OnUserSubscribed(string channel,
                                 string user)
    {

        //print("DBGPHOTONCHAT OnUserSubscribed");

    }

    public void OnUserUnsubscribed(string channel,
                                   string user)
    {
        //print("DBGPHOTONCHAT OnUserUnsubscribed");


    }

    public void sendMessage(string recipient, string message)
    {
        //chatClient.SendPrivateMessage(recipient, message);
        /*print("DBGPHOTONCHAT chat status Client state FrontendAddress " + chatClient.FrontendAddress
        + " canChat " + chatClient.CanChat
        + " ChatRegion " + chatClient.ChatRegion
        + " chatPeer " + chatClient.chatPeer
        + " chatNameServer " + chatClient.NameServerAddress);*/

        //Debug.Log("#DBGPHOTONCHAT recipient " + recipient + " message " + message);
        //   Debug.Log("DBGPHOTONCHAT send private message return value " + chatClient.SendPrivateMessage(recipient, message)
        //       + " msg " + message);
        chatClient.SendPrivateMessage(recipient, message);
    }

    public void prevLoadListPrevPage() 
    {
        if (gameLoadPageIdx > 0)
        {
            gameLoadPageIdx--;
            printActiveList();
        }
    }

    public void nextLoadListPage()
    {
        int maxIdx = getMaxListIdx();
            
        if (gameLoadPageIdx < maxIdx)
        {
            gameLoadPageIdx++;
            printActiveList();
        }
    }

    /*public void printFriends(int gameLoadPageIdx, List<string> list)
    {
        int startIdx = gameLoadPageIdx * listNumRows;
        int currIdx = 0;
        Debug.Log("print Friends list " + list.Count);

        for (int i = startIdx; i < list.Count; i++)
        {
            ListGameButtonGO[currIdx].SetActive(true); 
            ListText[currIdx].text = list[i].Split(':')[0];
           
            currIdx++;

            if (currIdx > (listNumRows - 1))
                break;
        }

        for (int i = currIdx; i < listNumRows; i++)
        {
            ListText[i].text = "";
            ListGameButtonGO[i].SetActive(false);
        }
    }*/

    public void printList(int gameLoadPageIdx, List<string> list, int type)
    {
        int startIdx = gameLoadPageIdx * listNumRows;
        int currIdx = 0;

        for (int i = startIdx; i < list.Count; i++)
        {
            ListGameButtonGO[currIdx].SetActive(true);
            ListText[currIdx].text = list[i].Split(':')[0];

            if (type == 0)
            {
                listAcceptButtonImage[currIdx].texture =
                    Resources.Load<Texture2D>("online/icon_01_119");

                //add username to get status of user
              
            }
            else 
            {
                listAcceptButtonImage[currIdx].texture =
                    Resources.Load<Texture2D>("online/icon_01_18");
            }

            if (chatClient != null)
            {
                StartCoroutine(addToCheckOnlineStatus(list[i].Split(':')[1]));
            }

            //Debug.Log("DBGchat type find " + chatClient);
            int keyVal = 0;
            bool keyExists = userStatus.TryGetValue(list[i].Split(':')[1], out keyVal);
            if (keyExists && keyVal == 2)
            {
                onlineStatusImg[currIdx].texture =
                    Resources.Load<Texture2D>("online/online_yes");
            }
            else
            {
                onlineStatusImg[currIdx].texture =
                     Resources.Load<Texture2D>("online/online_no");
            }


            currIdx++;

            if (currIdx > (listNumRows - 1))
                break;
        }

        /*Leave others empty*/
        for (int i = currIdx; i < listNumRows; i++)
        {
            ListText[i].text = "";
            ListGameButtonGO[i].SetActive(false);
        }
    }


    private void initFriendsList()
    {
        if (friendList == null)
            friendList = new List<string>();
        else
            friendList.Clear();

        if (PlayerPrefs.HasKey(friendListFileName))
        {
            string friends =
                   PlayerPrefs.GetString(friendListFileName);
            string[] friend = friends.Split('|');

            for (int i = friend.Length - 1; i >= 0; i--)
            {
                friendList.Add(friend[i]);
            }
        }
        
        for (int i = 0; i < listNumRows; i++)
        {
            int tmpIdx = i;
            //print("DBGLOADSAVED123 add listener " + listNumRows + " TMPIDX " + tmpIdx);

            listAcceptButton[i].onClick.RemoveAllListeners();
            listDeleteButton[i].onClick.RemoveAllListeners();

            //use accept button as a friends invite messages          
            listDeleteButton[i].onClick.AddListener(
                 delegate { deleteListItem(tmpIdx, friendListFileName, friendList); });

            listAcceptButton[i].onClick.AddListener(
               delegate { inviteToRoom(tmpIdx, friendListFileName); });
        }
    }

    private void initLists()
    {
        if (userStatus == null)
            userStatus = new Dictionary<string, int>();

        if (friendList == null)
            friendList = new List<string>();
        else
            friendList.Clear();

        if (friendInvities == null)
            friendInvities = new List<string>();
        else
            friendInvities.Clear();

        if (gameInviteList == null)
            gameInviteList = new List<string>();
        else
            gameInviteList.Clear();

        PlayerPrefs.DeleteKey(gameInvitesFileName);
        //PlayerPrefs.DeleteKey(friendInvitiesListFileName);
    }

    private void initFriendsInvitiesList()
    {
        if (friendInvities == null)
            friendInvities = new List<string>();
        else
            friendInvities.Clear();

        if (PlayerPrefs.HasKey(friendInvitiesListFileName))
        {
            string friends =
                   PlayerPrefs.GetString(friendInvitiesListFileName);
            string[] friend = friends.Split('|');

            for (int i = friend.Length - 1; i >= 0; i--)
            {
                friendInvities.Add(friend[i]);
            }
        }

        for (int i = 0; i < listNumRows; i++)
        {
            int tmpIdx = i;
            listAcceptButton[i].onClick.RemoveAllListeners();
            listDeleteButton[i].onClick.RemoveAllListeners();

            listAcceptButton[i].onClick.AddListener(
                 delegate { acceptFriend(tmpIdx, friendListFileName, friendInvities); });
            listDeleteButton[i].onClick.AddListener(
                delegate { deleteListItem(tmpIdx, friendInvitiesListFileName, friendInvities); });
        }
    }

    private void initInviteGameList()
    {
        if (gameInviteList == null)
            gameInviteList = new List<string>();
        else
            gameInviteList.Clear();

        if (PlayerPrefs.HasKey(gameInvitesFileName))
        {
            string invites =
                   PlayerPrefs.GetString(gameInvitesFileName);
            string[] invite = invites.Split('|');

            for (int i = invite.Length - 1; i >= 0; i--)
            {
                gameInviteList.Add(invite[i]);
            }
        }

        for (int i = 0; i < listNumRows; i++)
        {
            int tmpIdx = i;
            listAcceptButton[i].onClick.RemoveAllListeners();
            listDeleteButton[i].onClick.RemoveAllListeners();

            listAcceptButton[i].onClick.AddListener(
                 delegate { acceptGame(tmpIdx, gameInvitesFileName, gameInviteList); });
            listDeleteButton[i].onClick.AddListener(
                delegate { deleteListItem(tmpIdx, gameInvitesFileName, gameInviteList); });
        }
    }

    /*public void AreFriendsOnline()
    {
        chatClient.AddFriends(friendList.ToArray());
    }
    */

    public void onClickInpuUserIdChanged()
    {
        string inputText = inputFriendUserId.text;

        lastUserIdText = inputText;  
    }

    public void onClickExit()
    {
        initUIObjects();
        Globals.dontCheckOnlineUpdate = true;
        Globals.loadSceneWithBarLoader("multiplayerMenu");
    }

    public void onClickShowSendFriendRequestPanel()
    {
        sendFriendRequestPanel.SetActive(true);
    }

    public void onClickCloseSendFriendRequestPanel()
    {
        sendFriendRequestPanel.SetActive(false);
    }

    //username should be unique
    public void onClickSendFriendRequestButton()
    {
        sendMessage(lastUserIdText, "IF:" + nickName + ":" + username);
    }

    public void onClickCloseFriendRequestPanel()
    {
        sendFriendRequestPanel.SetActive(false);
    }

    public void onClickShowUserIdPanel()
    {
        userIdInfoPanel.SetActive(true);
    }

    public void onClickCloseUserIdPanel()
    {
        userIdInfoPanel.SetActive(false);
    }

    public void onClickShowInvitePartyPanel()
    {        
        if (chatClient == null)
            ConnectToPhotonChat();

        invitePartyPanel.SetActive(true);
        if (!PlayerPrefs.HasKey("ONLINE_NICKNAME"))
        {
            if (admobGameObject != null &&
                admobAdsScript != null)
            {
                admobAdsScript.showBannerAd();
            }
            nicknamePanel.SetActive(true);
            return;
        }

        onClickShowFriends();
    }
  

    public void onClickCloseListPanel()
    {        
        listPanel.SetActive(false);
        activeMenu = -1;
    }


    public void onClickShowFriends()
    {
        activeMenu = 0;
        gameLoadPageIdx = 0;
        listHeaderText.text = "Friend's list";
        initFriendsList();
        printList(gameLoadPageIdx, friendList, 0);
        listPanel.SetActive(true);
    }

    public void onClickShowFriendsInvities()
    {
        activeMenu = 1;
        gameLoadPageIdx = 0;
        listHeaderText.text = "Friend's invities";
        initFriendsInvitiesList();
        printList(gameLoadPageIdx, friendInvities, 1);
        listPanel.SetActive(true);
        friendInvitiesTextGO.SetActive(false);
    }

    public void onClickShowGameInvites()
    {
        activeMenu = 2;
        listHeaderText.text = "Game invities";
        gameLoadPageIdx = 0;
        initInviteGameList();
        printList(gameLoadPageIdx, gameInviteList, 2);
        listPanel.SetActive(true);
        gameInvitiesTextGO.SetActive(false);
    }

    public void onClickShowSendFriendRequests()
    {
        gameLoadPageIdx = 0;
        sendFriendRequestPanel.SetActive(true);
        //initFriendsRequestList();
        //listPanel.SetActive(true);
    }

    public void onClickShowCloseFriendRequests()
    {
        sendFriendRequestPanel.SetActive(false);
    }

    private void initUIObjects()
    {
        usernameID.text = username;
        listPanel.SetActive(false);
        userIdInfoPanel.SetActive(false);
        sendFriendRequestPanel.SetActive(false);

        friendInvitiesTextGO.SetActive(false);
        gameInvitiesTextGO.SetActive(false);
        invitePartMessageTextGO.SetActive(false);
        oopsPanel.SetActive(false);
        nicknamePanel.SetActive(false);
    }

    public void onClickNickNameSet()
    {
        string name = inputNickname.text;

        if (string.IsNullOrEmpty(name))
        {
            oopsPanel.SetActive(true);
            oopsPanelInfoText.text =
                Languages.getTranslate("Player name cannot be empty");
            return;
        }

        if (!Regex.IsMatch(name, "^[a-zA-Z ]*$"))
        {
            oopsPanel.SetActive(true);
            oopsPanelInfoText.text =
               Languages.getTranslate("Sorry. Only English characters are allowed");
            return;
        }

        admobAdsScript.hideBanner();
        nicknamePanel.SetActive(false);

        //onClickShowFriends();

        PlayerPrefs.SetString("ONLINE_NICKNAME", name);
        PlayerPrefs.Save();

        nickName = name;
    }

    public void acceptFriend(int buttonIdx, string friendListFileName, List<string> list)
    {
        int chosenGameIdx = (gameLoadPageIdx * listNumRows) + buttonIdx;
        string newFriendName = list[chosenGameIdx];

        if (PlayerPrefs.HasKey(friendListFileName))
        {
            string friends =
                   PlayerPrefs.GetString(friendListFileName);
            string[] friend = friends.Split('|');

            for (int i = 0; i < friend.Length; i++)
            {
                if (friends[i].Equals(newFriendName))
                    return;
            }
        }

        yesNoMenuNoAnswer.onClick.RemoveAllListeners();
        yesNoMenuYesAnswer.onClick.RemoveAllListeners();

        if (checkIfItemExists(friendList, newFriendName))
            return;

        friendList.Add(newFriendName);
        chatClient.AddFriends(new string[] { newFriendName.Split(':')[1] });
        saveToPrefs(newFriendName, friendListFileName);
        deleteListItemYes(newFriendName,
                          friendInvitiesListFileName);

        sendMessage(newFriendName.Split(':')[1], "A:" + nickName + ":" + username);
    }

    /*public void createParty()
    {
        onClickShowFriends();
    }*/

    public void acceptGame(int buttonIdx, string fileName, List<string> list)
    {
        int chosenGameIdx = (gameLoadPageIdx * listNumRows) + buttonIdx;
        string newGame = list[chosenGameIdx];

        deleteListItemYes(newGame,
                          gameInvitesFileName);

        roomName = newGame.Split(':')[2];
        //print("roomName " + roomName);
        Globals.isMultiplayerFriendConActive = true;
        Globals.multiplayer_lastGameName = newGame;
        launcher.Connect("playwithfriend", roomName);
        invitePartyPanel.SetActive(false);        
    }

    public void inviteToRoom(int buttonIdx, string friendListFileName)
    {
        int chosenGameIdx = (gameLoadPageIdx * listNumRows) + buttonIdx;
        string recipient = friendList[chosenGameIdx].Split(':')[1];
        string msg = "IG:" + nickName + ":" + username + ":" + roomName;
        
        //print("invite to room executed " +
        //    "IG:" + nickName + ":" + username + ":" + roomName
        //    + " recipient " + recipient);
        sendMessage(recipient, msg);
        print("inviteToRoommsg " + msg);
        launcher.Connect("playwithfriend", roomName);
        Globals.isMultiplayerFriendConActive = true;
        invitePartyPanel.SetActive(false);
    }

    public void deleteListItem(int buttonIdx, string fileName, List<string> list)
    {
        int chosenGameIdx = (gameLoadPageIdx * listNumRows) + buttonIdx;
        string itemName = list[chosenGameIdx];

        yesNoMenuNoAnswer.onClick.RemoveAllListeners();
        yesNoMenuYesAnswer.onClick.RemoveAllListeners();

        if (PlayerPrefs.HasKey(fileName))
        {
            yesNoMenuHeaderText.text =
                Languages.getTranslate("Do you want to delete?");

            yesNoMenuYesAnswer.onClick.AddListener(
            delegate {
                deleteListItemYes(itemName,
                                  fileName);
            });
        }        

        yesNoMenuNoAnswer.onClick.AddListener(
          delegate { closeYesNoMenuPanel(); });

        yesNoMenuPanel.SetActive(true);
    }
  
    public void deleteGameInvite(int buttonIdx, string fileName)
    {
        int chosenGameIdx = (gameLoadPageIdx * listNumRows) + buttonIdx;
        string itemName = gameInviteList[chosenGameIdx];

        yesNoMenuNoAnswer.onClick.RemoveAllListeners();
        yesNoMenuYesAnswer.onClick.RemoveAllListeners();

        if (PlayerPrefs.HasKey(fileName))
        {
            yesNoMenuHeaderText.text =
                Languages.getTranslate("Do you want to delete game invite?");

            yesNoMenuYesAnswer.onClick.AddListener(
            delegate {
                deleteListItemYes(itemName,
                                  fileName);
            });
        }

        yesNoMenuNoAnswer.onClick.AddListener(
          delegate { closeYesNoMenuPanel(); });

        yesNoMenuPanel.SetActive(true);
    }
 
    public void onCloseUserIdPanel()
    {
        userIdInfoPanel.SetActive(false);
    }

    public void onClickCloseInvitePartyPanel()
    {
        invitePartyPanel.SetActive(false);
    }

    public void onClickCloseOopsPanel()
    {
        oopsPanel.SetActive(false);
    }

    IEnumerator addToCheckOnlineStatus(string username)
    {
        for (int i = 0; i < 30; i++)
        {
            bool ret = chatClient.AddFriends(new string[] { username });
            if (ret) yield break;
            yield return new WaitForSeconds(1);
        }
    }
}
