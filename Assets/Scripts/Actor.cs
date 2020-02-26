using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Actor class
/// </summary>
public class Actor : MonoBehaviour
{
    /// <summary>
    /// Behavior tree used for AI
    /// </summary>
    private BehaviorTree _behaviorTree;

    /// <summary>
    /// How long the actor can see
    /// </summary>
    public float LineOfSight { get; protected set; }


    /// <summary>
    /// init variables
    /// </summary>
    private void Awake()
    {
        LineOfSight = 20.0f;
    }



}
