using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dice : MonoBehaviour
{
    PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void TurnOffTheDice()
    {
        print("I CALLED YO MAMA");
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void TurnOnTheDice()
    {
        gameObject.SetActive(true);
    }
}
