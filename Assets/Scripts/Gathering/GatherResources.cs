using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherResources : MonoBehaviour
{
    //The resource the entity is currently gathering
    [SerializeField] GameObject _Resource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GatherResource();
    }

    void GatherResource()
    {
        if(_Resource != null)
        {
            //Don't gather if there's nothing left
            if (_Resource.GetComponent<ResourceMaterial>().GetQuantity() > 0)
            {
                _Resource.GetComponent<ResourceMaterial>().DecreaseQuantity();
            }
        }
    }
}
