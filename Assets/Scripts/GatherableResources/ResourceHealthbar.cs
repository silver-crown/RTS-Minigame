using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages health bar component of Resources
/// </summary>
public class ResourceHealthbar : MonoBehaviour
{
    Transform bar;
    Resource parent;

    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<Resource>();
        bar = transform.GetComponent<ResourceHealthbar>().transform; //find HealthBar component
        parent.OnMined += OnMined;
    }

    void Update()
    {
        //face the healthbar towards the camera
        transform.rotation = Camera.main.transform.rotation;
    }

    //When parent takes damage
    void OnMined()
    {
        //calculate HP percentage
        float HealthPercentage = (float)parent.HP / (float)parent.MaxHP;
        //set bar to that full
        bar.localScale = new Vector3(HealthPercentage, 1f);
    }
}
