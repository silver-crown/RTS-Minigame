using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRequirements : MonoBehaviour
{
    Dictionary<string, int> _buildCost;

    //4 crystal and 4 metal
    public Dictionary<string, int> BuildCostScout()
    {
        //clean it up first 
        _buildCost.Clear();
        //Set the build cost values
        _buildCost.Add("Crystal", 4);
        _buildCost.Add("Metal", 4);
        return _buildCost;
    }
    //6 crystal and 2 metal
    public Dictionary<string, int> BuildCostScanner()
    {
        //clean it up first 
        _buildCost.Clear();
        //Set the build cost values
        _buildCost.Add("Crystal", 6);
        _buildCost.Add("Metal", 2);
        return _buildCost;
    }
    //4 crystal and 8 metal
    public Dictionary<string, int> BuildCostTank()
    {
        //clean it up first 
        _buildCost.Clear();
        //Set the build cost values
        _buildCost.Add("Crystal", 4);
        _buildCost.Add("Metal", 8);
        return _buildCost;
    }
    //6 crystal and 6 metal
    public Dictionary<string, int> BuildCostCamel()
    {
        //clean it up first 
        _buildCost.Clear();
        //Set the build cost values
        _buildCost.Add("Crystal", 6);
        _buildCost.Add("Metal", 6);
        return _buildCost;
    }
}
