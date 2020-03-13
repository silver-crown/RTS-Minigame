using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomisedPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3 (
            Random.Range(-4f, 4f),
            Random.Range(-4f, 4f),
            0f
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
