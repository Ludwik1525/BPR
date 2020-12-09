using UnityEngine;
using Photon.Pun;

public class Dice : MonoBehaviour
{
    PhotonView PV;


    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // turn the dice on/off
    [PunRPC]
    public void SwitchTheDice()
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
