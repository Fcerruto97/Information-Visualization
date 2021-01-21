using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public Transform Destination;
    private NavMeshAgent Agent;
    private Rigidbody rigidBody;
    public float posX, posZ;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Agent = this.GetComponent<NavMeshAgent>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        if (Agent == null)
        {
            Debug.LogError("Null agent");

        }
        else
        {
            SetDestination();
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        Agent.SetDestination(Destination.position);
        //Debug.Log(Destination.position);
    }

    void SetDestination()
    {
        if (Destination != null)
        {
            Vector3 targetVector = Destination.transform.position;
            Agent.SetDestination(targetVector);
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") { 
           
            this.transform.localPosition = new Vector3(0f + posX, 0.084f, 0.615f + posZ);
        }

    }


}
