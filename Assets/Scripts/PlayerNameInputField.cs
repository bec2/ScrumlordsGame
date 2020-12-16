using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// using Photon.Pun;
//using Photon.Realtime;


// Player name input. Allows user to enter custom name.
[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    #region 
    [SerializeField]
    private TMP_InputField nameInputField = null;
    [SerializeField]
    private Button setNameButton = null;

    #endregion

    #region Private Constants
    // Store Player Pref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";
    #endregion



    #region MonoBehaviour Callbacks
    // Monobehaviour method called on gameObject by Unity during initialisation phase.
    private void start() => SetUpInputField();
    
    private void SetUpInputField()
    {
        if(PlayerPrefs.HasKey(playerNamePrefKey))
        {
            return;
        }
        
        string defaultName = PlayerPrefs.GetString(playerNamePrefKey);
        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }
    #endregion



    #region Public Methods
    public void SetPlayerName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            setNameButton.interactable = false;
        }
    }

    // Sets player name and saves it in PlayerPrefs for future sessions.
    // <param name="value">The name of the Player</param>
    public void SavePlayerName()
    {
        string playerName = nameInputField.text;
        //PhotonNetwork.NickName = playerName;

        PlayerPrefs.SetString(playerNamePrefKey, playerName);
        
    }
    #endregion

}