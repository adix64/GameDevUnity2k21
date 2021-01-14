using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class TakeHitBhvr : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //figher-ul isi ia damage, decrementeaza HP:
        int HP = animator.GetInteger("HP");
        HP -= animator.GetInteger("TakenDamage");
        animator.SetInteger("HP", HP);
        if (HP <= 0)
        {
            animator.Play("Die");
            //cand un personaj moare, i se distrug toate componentele active:
            Destroy(animator.GetComponent<CapsuleCollider>());
            Destroy(animator.GetComponent<Rigidbody>());
            Destroy(animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Collider>());
            Destroy(animator.GetBoneTransform(HumanBodyBones.RightHand).GetComponent<Collider>());
            Destroy(animator.GetBoneTransform(HumanBodyBones.LeftHand).GetComponent<Collider>());
            Opponent opp = animator.GetComponent<Opponent>();
            //oponentul are alte componente fata de player, se distrug componentele specifice:
            if (opp != null)
            {
                Destroy(opp);
                Destroy(animator.GetComponent<NavMeshAgent>());
            }
            else
            {
                Destroy(animator.GetComponent<MovePlayer>());
            }
        }
    }  
    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
        
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
