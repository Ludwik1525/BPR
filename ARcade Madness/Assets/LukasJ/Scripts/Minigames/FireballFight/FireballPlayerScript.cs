using UnityEngine;
using Photon.Pun;
using TMPro;

public class FireballPlayerScript : MonoBehaviour
{
    private PhotonView PV;


    // setting the name over the player's head
    [PunRPC]
    private void SetName(string name)
    {
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    }

    // enabling the end screen (if somebody leaves and there's only 1 player left)
    [PunRPC]
    void EnableEndScreen()
    {
        FindObjectOfType<BoardMenus>().TurnOnWinScreen();
    }

    [PunRPC]
    void SetParent(int index)
    {
        GameObject go = GameObject.Find("SpawnPositions").transform.GetChild(index).gameObject;
        gameObject.transform.SetParent(go.transform);
    }

}
