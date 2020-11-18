using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class FireballPlayerScript : MonoBehaviour
{
    private PhotonView PV;

    [PunRPC]
    private void SetName(string name)
    {
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    }
}
