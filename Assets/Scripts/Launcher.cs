using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    // Max players per room. When max players is reached, a new room is created.
    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    #endregion



    #region Public Fields
    // The UI panel that allows the player to enter name, connect and play.
    [SerializeField]
    private GameObject controlPanel;
    // The label to inform connection is in progress.
    [SerializeField]
    private GameObject progressLabel;
    [SerializeField]
    private TextMeshProUGUI waitingStatusText;
    #endregion
    

    
    #region Private Fields
    // This is the clients version number.
    // Users are seperated from each other by gameVersion. (Allows you to make breaking changes)
    string gameVersion = "1.0";
    #endregion

   
    
    // A MonoBehaviour class essentially turns our class into an Unity Component that we can then drop onto a GameObject or Prefab. 
    // A class extending a MonoBehaviour has access to many very important methods and properties.
    // In your case, we use two callback methods, Awake() and Start().
    
    #region MonoBehaviour CallBacks
    // MonoBehaviour method called on GameObject by Unity during early initialization phase
    void Awake()
    {
        // CRITICAL
        // This makes sure we can use PhotonNetwork.LoadLevel() on master client and all clients in the same room sync automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }
    #endregion



    #region Public Methods
    // Start the connection process
    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
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
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
        #endregion
    }



    #region MonoBehaviourPunCallbacks Callbacks     
    public override void OnConnectedToMaster()
    {
        Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");
        // We first try to join an existing room. If failed we call OnJoinRandomFailed().
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
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

        if (playerCount != maxPlayersPerRoom) 
        {
            waitingStatusText.text = "Waiting for Oppenent";
            Debug.Log("Client is waiting for Opponent...");
        }
        else 
        {
            waitingStatusText.text = "Opponent found";
            Debug.Log("Match is ready to begin");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            
            waitingStatusText.text = "Opponent Found";
            Debug.Log("Match is ready to begin");

            PhotonNetwork.LoadLevel("SampleScene");
        }
    }
    #endregion

}
