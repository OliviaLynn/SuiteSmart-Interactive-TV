using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Beacon //: MonoBehaviour
{
    // Beacon data
    public string address;      // the beacon address
    public string url;       // the url of the img displayed when the beacon is in range
    public string personName;

    // RSSI
    public int rssi;            // the beacon's current rssi
    public int lastSignalCounter;

    public Beacon(string _address, string _url, string _personName)
    {
        // Assign args
        address = _address;
        url = _url;
        personName = _personName;

        // Initialize
        rssi = -100;
        lastSignalCounter = 0;
    }

    public void Print()
    {
        Debug.Log("A: " + address + " | R: " + rssi.ToString());
    }

    public void SetVisible(bool visible)
    {
    }

    public void IncLastSignalCounter()
    {
        // Use either this or RssiDecay to solve problem:
        // A beacon that is close (say, rssi = -40) that is able to leave the range
        // or get broken will no longer be sending signals, so it will forever be
        // recorded with an rssi of -40, despite no longer being that close

        // This solution below resets the rssi after a long enough time of not having
        // received a signal; I don't remember testing or even finishing it so I'm not sure
        // if it works. RssiDecay will be called every so often by BeaconHandler and will 
        // lower the rssis of all beacons linearly, so any beacon that does not periodically
        // send a signal with its true rssi will gradually end up with a very low or "far away" rssi

        lastSignalCounter += 1;
        if (lastSignalCounter > 60*20) // framerate TODO get
        {
            rssi = -100;
        }
    }

    public void RssiDecay()
    {
        // See comment above in IncLastSignalCounter
        if (rssi >= -100)
        {
            rssi -= 1;
            //Debug.Log(personName + "-> " + rssi);
        }
    }

    public void UpdateRssi(int _rssi)
    {
        rssi = _rssi;
        lastSignalCounter = 0;
    }

    public bool CheckRange(int range)
    {
        if (0 >= rssi && rssi >= range) 
        {
            //Debug.Log("IN RANGE: " + rssi.ToString() + "\t(" + address + ")");
            return true;
        }
        else
        {
            //Debug.Log("not in range: " + rssi.ToString() + "\t(" + addr + ")");
            return false;
        }
    }


}
