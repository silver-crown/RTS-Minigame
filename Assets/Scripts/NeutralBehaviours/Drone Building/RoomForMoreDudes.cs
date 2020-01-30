using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomForMoreDudes : MonoBehaviour
{
    [SerializeField] totalDrones _drones;
    //Central Intelligence has no more than 4 of each guy
    [SerializeField] int _maxScouts;
    [SerializeField] int _maxScanners;
    [SerializeField] int _maxTanks;
    [SerializeField] int _maxCamels;

    // Start is called before the first frame update
    void Start()
    {
        if(_maxScouts == 0)
        {
            _maxScouts = 4;
        }
        if (_maxScanners == 0)
        {
            _maxScanners = 2;
        }
        if (_maxTanks == 0)
        {
            _maxTanks = 3;
        }
        if (_maxCamels == 0)
        {
            _maxCamels = 2;
        }
    }
    ///get the current total number of drones for the type specified, if it's lower than the max then you're given to go sign.
    bool RoomForMore(string s)
    {
        s = s.ToLower();
        switch (s)
        {
            case ("scout"):
                {
                    if (_drones.GetDroneTotal(s) < _maxScouts)
                    {
                        return true;
                    }
                    break;
                }
            case ("scanner"):
                {
                    if (_drones.GetDroneTotal(s) < _maxScanners)
                    {
                        return true;
                    }
                    break;
                }
            case ("tank"):
                {
                    if (_drones.GetDroneTotal(s) < _maxTanks)
                    {
                        return true;
                    }
                    break;
                }
            case ("camel"):
                {
                    if (_drones.GetDroneTotal(s) < _maxCamels)
                    {
                        return true;
                    }
                    break;
                }
        }
        return false;
    }

}
