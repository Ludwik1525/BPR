using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private GameManager gm;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        gm = transform.parent.GetComponent<GameManager>();
        gm.onStartMoving.AddListener(delegate { StartBoolAnimationByName("isRunning"); });
        gm.onStopMoving.AddListener(delegate { StopBoolAnimationByName("isRunning"); });
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
