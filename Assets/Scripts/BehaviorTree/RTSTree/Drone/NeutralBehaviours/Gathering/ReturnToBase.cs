using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReturnToBase : MonoBehaviour
{
    bool _returnToBase;
    GameObject[] _bases;
    [SerializeField] NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        //Get all the bases
        if (_bases == null)
        {
            _bases = GameObject.FindGameObjectsWithTag("Base");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ReturnBackToBase();
    }
    void ReturnBackToBase()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        //Get the closest base and get over there
        _bases = GameObject.FindGameObjectsWithTag("Base");
        foreach (GameObject myBase in _bases)
        {
            float distance = Vector3.Distance(myBase.transform.position, currentPos);
            if (distance < minDistance)
            {
                closest = myBase.transform;
                minDistance = distance;
            }
        }
        agent.SetDestination(closest.position);
    }
}
