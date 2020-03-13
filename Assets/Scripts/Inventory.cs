using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic Inventory system for actors and structures. Just contains a dictionary of resource type and amount at present.
/// </summary>
public class Inventory
{
    /// <summary>
    /// The string will be resource type, the int will be amount.
    /// </summary>
    public Dictionary<string, int> Contents { get; private set; }

    /// <summary>
    /// How many resources the Inventory could hold
    /// </summary>
    private int _capacity;

    /// <summary>
    /// How 
    /// </summary>
    private int _amountInInventory;

    public Inventory(int Capacity)
    {
        Contents = new Dictionary<string, int>();
    }

    /// <summary>
    /// Put Amount of Resource ResourceType in the inventory.
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="amount"></param>
    public bool Deposit(string resourceType, int amount)
    {
        //if we don't have space
        if (GetAvailableSpace() < amount)
        {
            //the deposit fails
            return false;
        }
        //if the resource is already there
        if (Contents.ContainsKey(resourceType))
        {
            //add to the amount we have of it
            _amountInInventory += amount;
            Contents[resourceType] += amount;
        } else //if not,
        {
            //add it to Contents.
            _amountInInventory += amount;
            Contents.Add(resourceType, amount);
        }

        return true;
    }

    /// <summary>
    /// Try to withdraw the given amount of a resource. Returns the amount actually withdrawn, which could be smaller if one attempts to withdraw more than the inventory has.
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int Withdraw(string resourceType, int amount)
    {
        //if we don't have any of the resource
        if (!Contents.ContainsKey(resourceType))
        {
            //return nothing
            return 0;
        } else if (Contents[resourceType] <= amount) //if we don't have enough of the resource or this is all we have
        {
            //find out how much that amount is
            int amountToReturn = Contents[resourceType];
            //remove the now empty entry
            Contents.Remove(resourceType);

            _amountInInventory -= amountToReturn;
            return amountToReturn;
        } else //if we have more than enough
        {
            //take that much from the inventory
            Contents[resourceType] -= amount;
            //return it
            _amountInInventory -= amount;
            return amount;
        }
    }

    /// <summary>
    /// Returns amount of space available for resources in the inventory
    /// </summary>
    /// <returns></returns>
    public int GetAvailableSpace()
    {
        return _capacity - _amountInInventory;
    }
}
