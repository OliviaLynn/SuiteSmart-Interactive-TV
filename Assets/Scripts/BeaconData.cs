using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeaconContainer
{
    public List<BeaconData> beacons;

    public BeaconContainer(List<BeaconData> _beacons)
    {
        beacons = _beacons;
    }
}

[System.Serializable]
public class BeaconData
{
    public string address;
    public string url;
    public string name;

    public BeaconData(string _address, string _url, string _name)
    {
        address = _address;
        url = _url;
        name = _name;
        
    }
}