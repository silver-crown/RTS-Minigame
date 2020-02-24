using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceAvailabilityChecker : MonoBehaviour
{
    [SerializeField] int _crystals, _metals;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetCrystal()
    {
        return _crystals;
    }
    public void SetCrystals(int crystals)
    {
        _crystals = crystals;
    }
    public int GetMetals()
    {
        return _metals;
    }
    public void SetMetals(int metals)
    {
        _metals = metals;
    }

}
