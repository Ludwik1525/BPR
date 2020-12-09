using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spinner : MonoBehaviour
{
    public float spinSpeed = 3600f;
    public bool doSpin = false;
    public GameObject playerGraphics;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (doSpin)
        {
            playerGraphics.transform.Rotate(new Vector3(0, spinSpeed * Time.deltaTime, 0));
        }
    }

    [PunRPC]
    void EnableEndScreen()
    {
        FindObjectOfType<BoardMenus>().TurnOnWinScreen();
    }
}
