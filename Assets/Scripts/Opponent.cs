using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : Fighter
{
    NavMeshAgent agent;
   
    public float farFromPlayerSpeed = 2f, nearPlayerSpeed = 0.7f;
    public float attackRange = 1.1f;
    [Range(10e-3f, 1f)]
    public float offensiveness = 1f;
    Vector3 destinationOffset = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        base.GetCommonComponents();
        agent = GetComponent<NavMeshAgent>();
        MovePlayer movePlayerScript = activeOpponent.GetComponent<MovePlayer>();
        movePlayerScript.opponents.Add(transform);
        StartCoroutine(SeedRandomDirCoroutine(0f));
    }
    IEnumerator SeedRandomDirCoroutine(float t)
    {
        yield return new WaitForSeconds(t);
        int r = Random.Range(0, 10);
        switch (r)
        {//se schimba comportamentul oponentului in functie de numarul aleator cu anumita probabilitate de a ocoli playerul:
            case 0: case 1: destinationOffset =  transform.right; break;
            case 2: case 3: case 9: destinationOffset = -transform.right; break;
            case 4: case 5: case 6: destinationOffset = -transform.forward * 3f; break;
            case 7: case 8: destinationOffset = Vector3.zero; break;
        }
        float newT = Random.Range(0.5f, 1.5f);
        yield return StartCoroutine(SeedRandomDirCoroutine(newT));
    }
    // Update is called once per frame
    void Update()
    {
        toOpp = activeOpponent.position - transform.position;
        CheckGroundedStatus();
        float r = Random.Range(0f, 1f);
        animator.SetFloat("DistToOpp", toOpp.magnitude);
        if (toOpp.magnitude < attackRange && r < offensiveness)
            animator.SetTrigger("Punch");
        //agent.enabled = grounded;
        ApplyRootMotion();
        ApplyRootRotation(toOpp.normalized);
    }
    private void LateUpdate()
    {
        LookAtOpponent();
    }
    private void ApplyRootMotion()
    {
        if (!grounded)
            return;

        if(toOpp.magnitude > 10f)
             agent.SetDestination(transform.position);//nu se deplaseaza
        else // targeteaza playerul cu offsetul calculat random in functia SeedRandomDirCoroutine
            agent.SetDestination(activeOpponent.position + destinationOffset);
        
        ComputeAgentSpeed();
        Vector3 characterSpaceMoveDir = transform.InverseTransformDirection(agent.velocity);
        animator.SetFloat("Forward", characterSpaceMoveDir.z, 0.15f, Time.deltaTime);
        animator.SetFloat("Right", characterSpaceMoveDir.x, 0.15f, Time.deltaTime);
    }

    private void ComputeAgentSpeed()
    {
        float disttoOpp = toOpp.magnitude;
        //calculeaza factor de interpolare bazat pe distanta din interval [5, 10] -> [0, 1]
        float changeSpeedFactor = Mathf.Clamp01((disttoOpp - 5f) / 5f);
        agent.speed = Mathf.Lerp(nearPlayerSpeed, farFromPlayerSpeed, changeSpeedFactor);
        if (!grounded)//cand cade are viteza mai mare
            agent.speed = 5f;
        if (stateInfo.IsTag("takeHit"))
            agent.speed = 0f;//ramane pe loc cand isi ia lovitura
    }
}
