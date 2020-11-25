using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Rocket : MonoBehaviour
{
    public bool isAvailable = true;

    private int numberOfTilesToMove = 5;

    private Button rocketB;

    private PhotonView PV;

    private GameManager BPC;
    

    void Start()
    {
        PV = GetComponent<PhotonView>();
        BPC = GetComponent<GameManager>();
        rocketB = GameObject.Find("ButtonRocket").GetComponent<Button>();

        rocketB.onClick.AddListener(UseRocket);

        TurnOffRocket();
    }

    // function to move forward with the rocket
    public void UseRocket()
    {
        if (BPC.PV.IsMine)
        {
            BPC.hasUsedPowerUp = true;
            int randomNo = Random.Range(5, 8);
            PV.RPC("UnifyTheRandomNumber", RpcTarget.AllBuffered, randomNo);
            StartCoroutine(BPC.MoveWithRocket(numberOfTilesToMove));
        }
    }

    // function to disable the power-up
    public void TurnOffRocket()
    {
        if (isAvailable)
        {
            isAvailable = false;
            rocketB.interactable = false;
        }
    }

    // function to enable the power-up
    public void TurnOnRocket()
    {
        if (!isAvailable)
        {
            isAvailable = true;
            rocketB.interactable = true;
        }
    }

    // function to make the random number the same for all the players
    [PunRPC]
    void UnifyTheRandomNumber(int number)
    {
        numberOfTilesToMove = number;
    }
}
