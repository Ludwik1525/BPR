using UnityEngine;
using Photon.Pun;

public class PlayerScript : MonoBehaviour
{
    private Transform playersParent;

    private GameObject rocket;

    private PhotonView PV;


    private void Start()
    {
        rocket = gameObject.transform.GetChild(1).GetChild(1).gameObject;
    }

    //ready for game after placing ar board
    public bool readyForGame;

    // add the player to the list of players
    [PunRPC]
    void RPC_AddToList()
    {
        PV = GetComponent<PhotonView>();
        
        GameSetupController.players.Add(this.gameObject);
    }

    // add the player to a new list of players
    [PunRPC]
    void RPC_AddToNewList()
    {
        PV = GetComponent<PhotonView>();

        GameSetupController.newPlayers.Add(this.gameObject);
    }

    // set the player's parent
    [PunRPC]
    void RPC_SetParent()
    {
        playersParent = GameObject.Find("PlayersParent").transform;

        this.gameObject.transform.SetParent(playersParent);
    }

    // enable the end screen
    [PunRPC]
    void EnableEndScreen()
    {
        FindObjectOfType<BoardMenus>().TurnOnWinScreen();
    }

    // enable the rocket power-up
    [PunRPC]
    private void EnableRocket()
    {
        rocket.SetActive(true);
    }

    // disable the rocket power-up
    [PunRPC]
    private void DisableRocket()
    {
        rocket.SetActive(false);
    }

}
