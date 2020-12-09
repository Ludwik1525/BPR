using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationController : MonoBehaviour
{
    private GameManager playerController;
    private Animator anim;
    private PhotonView PV;

    private void Start()
    {
        PV = transform.parent.GetComponent<PhotonView>();
        anim = GetComponentInChildren<Animator>();

        playerController = transform.parent.GetComponent<GameManager>();
        playerController.onStartMoving.AddListener(delegate { StartBoolAnimationByName("isRunning"); });
        playerController.onStopMoving.AddListener(delegate { StopBoolAnimationByName("isRunning"); });
        playerController.onStartMovingWithRocket.AddListener(delegate { StartBoolAnimationByName("isRidingRocket"); });
        playerController.onStartMovingWithRocket.AddListener(delegate { PV.RPC("EnableRocket", RpcTarget.AllBuffered); });
        playerController.onStopMovingWithRocket.AddListener(delegate { StopBoolAnimationByName("isRidingRocket"); });
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


}
