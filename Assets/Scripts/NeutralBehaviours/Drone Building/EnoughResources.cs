using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnoughResources : MonoBehaviour
{
    [SerializeField] ResourceRequirements _requirements;
    [SerializeField] ResourcesAvailable _myResources;
    //used for temporarily storing the requirements
    Dictionary<string, int> _req;
    //Central Intelligence has AT LEAST 4 Crystal and 4 Metal
    // Start is called before the first frame update
    int _crystals, _metals;
    void Start()
    {
        _crystals = _myResources.GetCrystal();
        _metals = _myResources.GetMetals();
    }

    // Update is called once per frame
    void Update()
    {
        _crystals = _myResources.GetCrystal();
        _metals = _myResources.GetMetals();

        //Drones aren't made here, we're only checking that we can make them with the functions below
    }

    //A scout takes 4 crystal and 4 metal
    public bool CanIMakeScout()
    {
        _req = _requirements.BuildCostScout();
        if(_req["Crystal"] == 4 && _req["Metal"] == 4)
        {
            return true;
        }
        return false;
    }
    //A scanner takes 6 crystals and 2 metals
    public bool CanIMakeScanner()
    {
        _req = _requirements.BuildCostScanner();
        if (_req["Crystal"] == 6 && _req["Metal"] == 2)
        {
            return true;
        }
        return false;
    }
    //The tank takes 4 crystals and 8 metals
    public bool CanIMakeTank()
    {
        _req = _requirements.BuildCostTank();
        if (_req["Crystal"] == 4 && _req["Metal"] == 8)
        {
            return true;
        }
        return false;
    }
    //The camel takes 6 crystals and 6 metals
    public bool CanIMakeCamel()
    {
        _req = _requirements.BuildCostTank();
        if (_req["Crystal"] == 4 && _req["Metal"] == 8)
        {
            return true;
        }
        return false;
    }
}
