using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    private GameObject whoWinsText;

    [SerializeField]
    private GameObject player1MoveText;

    [SerializeField]
    private GameObject player2MoveText;

    [SerializeField]
    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static bool gameOver = false;

    void Start() {

        whoWinsText = GameObject.Find("whoWinsText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        // player1 = GameObject.Find("Player1");
        // player2 = GameObject.Find("Player2");

        if (PhotonNetwork.IsConnected) 
        {
            if (PhotonNetwork.IsMasterClient) 
            {
                Debug.Log("Initialising Player 1");
                player1 = PhotonNetwork.Instantiate("Player1", player1.transform.position, Quaternion.identity);

            } else 
            {
                Debug.Log("Initialising Player 2");
                player2 = PhotonNetwork.Instantiate("Player2", player2.transform.position, Quaternion.identity);
            }
        }

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        whoWinsText.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);

        
    }

    void Update()
    {
        if (player1.GetComponent<FollowThePath>().waypointIndex >
            player1StartWaypoint + diceSideThrown) {

            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;

        }

        if (player2.GetComponent<FollowThePath>().waypointIndex >
            player2StartWaypoint + diceSideThrown) {

            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player1.GetComponent<FollowThePath>().waypointIndex ==
            player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsText.GetComponent<Text>().text = "Player 1 wins";
            gameOver = true;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex ==
            player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsText.GetComponent<Text>().text = "Player 2 wins";
            gameOver = true;
        }
    }

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) {

            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                break;
        }
    }
}

