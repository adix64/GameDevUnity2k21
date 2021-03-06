﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateBhvr : StateMachineBehaviour
{
    public float colliderEnableTimestamp = 0.032f;
    public float colliderDisableTimestamp = 0.07f;
    public HumanBodyBones bone;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float t = stateInfo.normalizedTime;
        //collider activ pe maini doar in framurile in care poate lovi, intre timestamps
        animator.GetBoneTransform(bone).GetComponent<Collider>().enabled =
                t > colliderEnableTimestamp && t < colliderDisableTimestamp;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetBoneTransform(bone).GetComponent<Collider>().enabled = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
