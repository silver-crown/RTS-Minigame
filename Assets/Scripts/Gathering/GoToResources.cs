using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToResources : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float _speed;
    enum MaterialCurrentlyGathering
    {
        Crystal, Metal
    }
    [SerializeField] MaterialCurrentlyGathering _myMaterial;
    GameObject[] _crystals, _metals;
    [SerializeField] 
    // Start is called before the first frame update
    void Start()
    {
        //Find all the objects 
        if(_crystals == null)
        {
            _crystals = GameObject.FindGameObjectsWithTag("Crystal");
        }   
        if(_metals == null)
        {
            _metals = GameObject.FindGameObjectsWithTag("Metal");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GoToResource(FindClosestResource());
        //tag the resource 
    }

    //find the closest resource currently available
    Transform FindClosestResource()
    { 
        Transform closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        switch (_myMaterial)
        {
            //Currently gathering crystals
            case (MaterialCurrentlyGathering.Crystal):
                {
                    //Recalculate _crystals cause one might disappear at some point
                    _crystals = GameObject.FindGameObjectsWithTag("Crystal");
                    foreach (GameObject crystal in _crystals)
                    {
                        float distance = Vector3.Distance(crystal.transform.position, currentPos);
                        if (distance < minDistance)
                        {
                            closest = crystal.transform;
                            minDistance = distance;
                        }
                    }
                    _crystals = GameObject.FindGameObjectsWithTag("Crystal");
                    return closest;
                }
            //Currently gathering metals
            case (MaterialCurrentlyGathering.Metal):
                {
                    //Recalculate _metals cause one might disappear at some point
                    _metals = GameObject.FindGameObjectsWithTag("Metal");
                    foreach (GameObject metal in _metals)
                    {
                        float distance = Vector3.Distance(metal.transform.position, currentPos);
                        if (distance < minDistance)
                        {
                            closest = metal.transform;
                            minDistance = distance;
                        }
                    }
                    return closest;
                }
            default:
                {
                    return closest;
                }
        }
    }
    //Self explanatory
    void GoToResource(Transform resource)
    {
        agent.SetDestination(resource.position);
    }
}
