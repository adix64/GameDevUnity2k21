using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedBhvr : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //calculeza procentajul (lauyerWeight) influentei layerului cu mainile pe baza distantei pana la adversar
        float distToOpp = animator.GetFloat("DistToOpp");
        float layerWeight = 1f - (distToOpp - 1f) / 4f; //din intervalul [5, 1]->[0, 1]
        //foloseste 100% layerul mainilor daca tinteste:
        layerWeight = animator.GetBool("Aiming") ? 1f : layerWeight;
        animator.SetLayerWeight(1, layerWeight);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {//forteaza 0% influenta layerului cu mainile cand iese din "Grounded"
        animator.SetLayerWeight(1, 0f);
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
