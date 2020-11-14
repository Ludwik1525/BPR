using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private GameManager playerController;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerController = transform.parent.GetComponent<GameManager>();
        playerController.onStartMoving.AddListener(delegate { StartBoolAnimationByName("isRunning"); });
        playerController.onStopMoving.AddListener(delegate { StopBoolAnimationByName("isRunning"); });
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
