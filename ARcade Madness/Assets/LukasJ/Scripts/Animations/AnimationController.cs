using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationController : MonoBehaviour
{
    private BoardPlayerController playerController;
    private Animator anim;
    private GameObject rocket;
    private PhotonView PV;

    private void Start()
    {
        PV = transform.parent.GetComponent<PhotonView>();
        rocket = gameObject.transform.GetChild(1).gameObject;
        anim = GetComponentInChildren<Animator>();

        playerController = transform.parent.GetComponent<BoardPlayerController>();
        playerController.onStartMoving.AddListener(delegate { StartBoolAnimationByName("isRunning"); });
        playerController.onStopMoving.AddListener(delegate { StopBoolAnimationByName("isRunning"); });
        playerController.onStartMovingWithRocket.AddListener(delegate { StartBoolAnimationByName("isRidingRocket"); });
        playerController.onStartMovingWithRocket.AddListener(delegate { PV.RPC("EnableRocket", RpcTarget.AllBuffered); });
        playerController.onStopMovingWithRocket.AddListener(delegate { StartBoolAnimationByName("isRidingRocket"); });
        playerController.onStopMovingWithRocket.AddListener(delegate { PV.RPC("DisableRocket", RpcTarget.AllBuffered); });

    }

    void StartBoolAnimationByName(string animationName)
    {
        anim.SetBool(animationName, true);
    }

    void StopBoolAnimationByName(string animationName)
    {
        anim.SetBool(animationName, false);
    }

    [PunRPC]
    private void EnableRocket()
    {
        rocket.SetActive(true);
    }

    [PunRPC]
    private void DisableRocket()
    {
        rocket.SetActive(false);
    }
}
