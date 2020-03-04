using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Abstract class for all gatherable resources to derive from.
/// </summary>
public class Resource : MonoBehaviour
{
    public int HP { get; protected set; } = 40;
    public int MaxHP { get; protected set; } = 40;

    public Type ResourceType { get; protected set; }

    //Action used to update health bar
    public System.Action OnMined;

    private void Awake()
    {
        //add object to resource list
    }

    private void Update()
    {
        if (HP==0)
        {
            Die();
        }
    }


    /// <summary>
    /// Takes a request of how much to mine the resource for. Returns how much you managed to mine.
    /// </summary>
    /// <param name="amount">How much is requested from the resource </param>
    /// <returns></returns>
    public int Mine(int amount)
    {
        int amountMined = 0;

        //Decrease the health of the resource by the amount requested if possible, if not deplete it fully. Return the amount mined for.
        amountMined = System.Math.Min(HP, amount);
        HP -= amountMined;
        //Invoke mining action so subscribers (the healthbar) can do their thing
        OnMined?.Invoke();

        return amountMined;
    }

    /// <summary>
    /// Destroy object and unsubscribe it from lists.
    /// </summary>
    public void Die()
    {

    }

    //Types of resources that exist
    public enum Type { CRYSTAL, METAL };
}
