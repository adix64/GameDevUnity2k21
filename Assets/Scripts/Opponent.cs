using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : Fighter
{
    NavMeshAgent agent;
    public Transform player;
    Vector3 toPlayer;
    public float farFromPlayerSpeed = 2f, nearPlayerSpeed = 0.7f;
    public float attackRange = 1.1f;
    // Start is called before the first frame update
    void Start()
    {
        base.GetCommonComponents();
        agent = GetComponent<NavMeshAgent>();
        MovePlayer movePlayerScript = player.GetComponent<MovePlayer>();
        movePlayerScript.opponents.Add(transform);
    }

    // Update is called once per frame
    void Update()
    {
        toPlayer = player.position - transform.position;
        CheckGroundedStatus();
        if (toPlayer.magnitude < attackRange)
            animator.SetTrigger("Punch");
        //agent.enabled = grounded;
        ApplyRootMotion();
    }

    private void ApplyRootMotion()
    {
        if (!grounded)
            return;
        agent.SetDestination(player.position);
        
        ComputeAgentSpeed();
        Vector3 characterSpaceMoveDir = transform.InverseTransformDirection(agent.velocity);
        animator.SetFloat("Forward", characterSpaceMoveDir.z, 0.15f, Time.deltaTime);
        animator.SetFloat("Right", characterSpaceMoveDir.x, 0.15f, Time.deltaTime);
    }

    private void ComputeAgentSpeed()
    {
        float distToPlayer = toPlayer.magnitude;
        //calculeaza factor de interpolare bazat pe distanta din interval [5, 10] -> [0, 1]
        float changeSpeedFactor = Mathf.Clamp01((distToPlayer - 5f) / 5f);
        agent.speed = Mathf.Lerp(nearPlayerSpeed, farFromPlayerSpeed, changeSpeedFactor);
        if (grounded)
            agent.speed = 3f;
    }
}
