using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainMenu : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Game for 2");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        PhotonNetwork.LeaveRoom();
    }
}
