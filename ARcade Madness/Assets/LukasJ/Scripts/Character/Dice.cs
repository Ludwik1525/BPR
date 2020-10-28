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
    public void TurnOnTheDice()
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    public IEnumerator SwitchTheDice()
    {
        yield return new WaitForSeconds(1.1f);
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
