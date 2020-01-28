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

    }

    void GatherResource()
    {
        _Resource.GetComponent<ResourceMaterial>().DecreaseQuantity();
    }
}
