using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic Inventory system for actors and structures. Just contains a dictionary of resource type and amount at present.
/// </summary>
public class Inventory : MonoBehaviour
{
    /// <summary>
    /// The string will be resource type, the int will be amount.
    /// </summary>
    public Dictionary<string, int> Contents { get; private set; }

    [SerializeField]
    /// <summary>
    /// How many resources the Inventory could hold
    /// </summary>
    public int Capacity;

    /// <summary>
    /// How 
    /// </summary>
    private int _amountInInventory;


    private void Awake()
    {
        Contents = new Dictionary<string, int>();
    }
    /// <summary>
    /// Put Amount of Resource ResourceType in the inventory. Returns the amount that could be deposited.
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="amount"></param>
    public int Deposit(string resourceType, int amount)
    {
        //find out if we have space for everything, and if not figure out how much we can deposit
        int amountToDeposit = Mathf.Min(amount, GetAvailableSpace());

        //Debug.Log(Contents);
        //Debug.Log(resourceType);

        //if the resource is already there
        if (Contents.ContainsKey(resourceType))
        {
            //add to the amount we have of it
            _amountInInventory += amountToDeposit;
            Contents[resourceType] += amountToDeposit;
            Debug.Log(name +":Successfully deposited " + amountToDeposit + " " + resourceType + ", new total is " + Contents[resourceType]);
        } else //if not,
        {
            Debug.Log(name + ":Did not have resource type " + resourceType);
            //add it to Contents.
            _amountInInventory += amountToDeposit;
            Contents.Add(resourceType, amountToDeposit);
        }

        //return how much we managed to deposit
        return amountToDeposit;
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
        return Capacity - _amountInInventory;
    }

    /// <summary>
    /// Returns a list with all the keys in the Inventory
    /// </summary>
    /// <returns></returns>
    public List<string> GetKeyList()
    {
        List<string> keyList = new List<string>();

        foreach (string key in Contents.Keys)
        {
            keyList.Add(key);
        }

        return keyList;
    }

    public override string ToString()
    {
        string str = base.ToString();
        str += "\n";
        str += "Capacity = " + Capacity + "\n";
        return str;
    }

    public void SetCapacityFromFile(string filename)
    {

    }
}
