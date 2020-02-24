using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UtilityAction : MonoBehaviour
{
    protected List<Factor> _factors;
    private Behavior _behavior;
    private float _utility = 0.0f;


    /// <summary>
    /// Initializes the Utility action with the factors that affect its utility and the behavior it executes
    /// </summary>
    /// <param name="factors"></param>
    /// <param name="behavior"></param>
    public UtilityAction(List<Factor> factors, Behavior behavior)
    {
        _factors = factors;
        _behavior = behavior;
    }

    //Updates utility to be the average of all its factors utilities
    private void _updateUtility()
    {
        //Sum all normalized utility values
        float sum = 0.0f;
        foreach (Factor factor in _factors)
        {
            factor.UpdateUtility();
            sum += factor.GetUtility();
        }
        _utility = sum / _factors.Count; //set the action's utility to the average of the factors' utility
    }

    public float GetUtility()
    {
        _updateUtility(); //update the action's utility value
        return _utility; //return it
    }

}