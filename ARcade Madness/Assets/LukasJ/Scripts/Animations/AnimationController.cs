using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private BoardPlayerController playerController;
    private Animator anim;
    private GameObject rocket;

    private void Start()
    {
        rocket = gameObject.transform.GetChild(1).gameObject;
        anim = GetComponentInChildren<Animator>();
        playerController = transform.parent.GetComponent<BoardPlayerController>();
        playerController.onStartMoving.AddListener(delegate { StartBoolAnimationByName("isRunning"); });
        playerController.onStopMoving.AddListener(delegate { StopBoolAnimationByName("isRunning"); });
        playerController.onStartMovingWithRocket.AddListener(delegate { StartBoolAnimationByName("isRidingRocket"); });
        playerController.onStartMovingWithRocket.AddListener(EnableRocket);
        playerController.onStopMovingWithRocket.AddListener(delegate { StartBoolAnimationByName("isRidingRocket"); });
        playerController.onStopMovingWithRocket.AddListener(DisableRocket);

    }

    void StartBoolAnimationByName(string animationName)
    {
        anim.SetBool(animationName, true);
    }

    void StopBoolAnimationByName(string animationName)
    {
        anim.SetBool(animationName, false);
    }

    private void EnableRocket()
    {
        rocket.SetActive(true);
    }

    private void DisableRocket()
    {
        rocket.SetActive(false);
    }
}
