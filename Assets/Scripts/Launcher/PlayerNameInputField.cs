using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


// Player name input. Allows user to enter custom name.
[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    #region Private Constants
    // Store Player Pref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";
    #endregion



    #region MonoBehaviour Callbacks
    // Monobehaviour method called on gameObject by Unity during initialisation phase.
    void start() 
    {
        string defaultName = string.Empty;
        InputField inputField = this.GetComponent<InputField>();
        if (inputField!=null)
        {
            if(PlayerPrefs.HasKey(playerNamePrefKey))
            {
                inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }
    #endregion



    #region Public Methods
    // Sets player name and saves it in PlayerPrefs for future sessions.
    // <param name="value">The name of the Player</param>
    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player name is null or empty.");
            return;
        }
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }

    #endregion 

}
