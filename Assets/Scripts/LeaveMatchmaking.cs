using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LeaveMatchmaking : MonoBehaviour
{
    [SerializeField]
    public Button LeaveMatch = null;

    public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("Launcher");
        }
}
