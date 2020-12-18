using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    // Max players per room. When max players is reached, a new room is created.
    [SerializeField]
    private byte maxPlayersPerRoom = 1;
    #endregion



    #region Public Fields
    // The UI panel that allows the player to enter name, connect and play.
    [SerializeField]
    private GameObject findOpponentPanel;
    // The label to inform connection is in progress.
    [SerializeField]
    private GameObject waitingStatusPanel;
    [SerializeField]
    private TextMeshProUGUI waitingStatusText;
    bool isConnecting;
    #endregion
    

    
    #region Private Fields
    // This is the clients version number.
    // Users are seperated from each other by gameVersion. (Allows you to make breaking changes)
    string gameVersion = "0.1";
    #endregion

   
    
    // A MonoBehaviour class essentially turns our class into an Unity Component that we can then drop onto a GameObject or Prefab. 
    // A class extending a MonoBehaviour has access to many very important methods and properties.
    // In your case, we use two callback methods, Awake() and Start().
    
    #region MonoBehaviour CallBacks
    // MonoBehaviour method called on GameObject by Unity during early initialization phase
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        
        findOpponentPanel.SetActive(false);
        waitingStatusPanel.SetActive(false);
    } 
        // CRITICAL
        // This makes sure we can use PhotonNetwork.LoadLevel() on master client and all clients in the same room sync automatically
        

    public void FindOpponent()
    {   
        findOpponentPanel.SetActive(false);
        waitingStatusPanel.SetActive(true);

        waitingStatusText.text = "Searching...";
    
        // If already connected, we attempt joining a random room
        if (PhotonNetwork.IsConnected)
        {
            // We need at this point to attempt to join random room
            PhotonNetwork.JoinRandomRoom();
        }
        // If not yet connected, connect the application instance to Photon Cloud Network
        else
        {
            // We MUST first and foremost connect to Photon Online Server
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
        #endregion
    }


    #region MonoBehaviourPunCallbacks Callbacks     
    public override void OnConnectedToMaster()
    {
        Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");
        // We first try to join an existing room. If failed we call OnJoinRandomFailed().
        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        findOpponentPanel.SetActive(true);
        waitingStatusPanel.SetActive(false);
        isConnecting = false;
        Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Launcher: OnJoinedRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
        // If OnJoinRandomFailed() is called, we create a room.
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Game for 1' ");


            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("Game for 1");
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            
            waitingStatusText.text = "Opponent Found";
            Debug.Log("Match is ready to begin");

            PhotonNetwork.LoadLevel("Game for 2");
        }
    }

    

    
    #endregion

}
