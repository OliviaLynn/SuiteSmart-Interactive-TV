using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;

public class BeaconHandler : MonoBehaviour
{
	private string fileName = "data-all.json";

	private int range = -90;
	// The RSSI range at which to start showing a beacon's display
	private int rssiDecayCountdown = 0;
	private int rssiDecayInterval = 1 * 60;
	// Every x seconds, we decrement all RSSIs by 1
	// This helps keep us from getting stuck with an RSSI
	// if a beaocn suddenly dies or something

	public UDPReceiver udpReceiver;
	public UniWebView_Controller webViewController;
	public List<Beacon> beacons = new List<Beacon> ();
	private Beacon closestBeacon;

	public UnityEngine.UI.Text WelcomeText;

	// ------------------------------------------------------------------------
	void Start ()
	{
		//Debug.Log ("PERSISTANT DATA PATH:");
		//Debug.Log (Application.persistentDataPath);

		// Display IP
		string localIP;
		using (Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
			socket.Connect ("8.8.8.8", 65530);
			IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
			localIP = endPoint.Address.ToString ();
			Debug.Log (localIP);
		}
		/*
		GameObject IPText = GameObject.Find ("LocalIP");
		if (IPText) {
			IPText.GetComponent<UnityEngine.UI.Text> ().text = localIP;
		}*/

		// Read in data
		DeserializeJSON ();

		// Range warning
		if (range >= 0) {
			Debug.Log ("Range is currently set to a nonnegative number (" + range + "). RSSI values are never positive, are you sure this is the range you want?");
		}

		// Initialize the closest beacon
		closestBeacon = new Beacon ("", "", "");
	}

	// ------------------------------------------------------------------------
	public void Temp_MockSauron ()
	{
		string mockMessage = "ac:23:3f:23:56:f3|-55";
		UpdateBeaconRssi (mockMessage);
	}

	public void Temp_MockGandalf ()
	{
		string mockMessage = "ac:23:3f:23:57:07|-55";
		UpdateBeaconRssi (mockMessage);
	}

	public void Temp_NoNearbyBeacons ()
	{
		beacons.ForEach (bcn => bcn.UpdateRssi (-101));
	}

	// ------------------------------------------------------------------------
	void Update ()
	{
		// Check for received message
		string message = udpReceiver.getMessage ();
		if (message != null) {
			UpdateBeaconRssi (message);
			//Debug.Log (message);
		}
			
		//DecayRSSIs ();
	}

	// ------------------------------------------------------------------------
	private void UpdateBeaconRssi (string receivedMessage)
	{
		// Update the beacon's RSSI
		string[] bluetoothAddrAndRssi = receivedMessage.Split ('|');
		if (bluetoothAddrAndRssi.Length == 2) {
			// Get bluetooth address
			string bluetoothAddr = bluetoothAddrAndRssi [0];

			// Check if relevant beacon
			Beacon beacon = beacons.Find (b => Equals (b.address, bluetoothAddr));
			if (beacon != null) {
				//Debug.Log("List<T>.Find() found: " + beacon.personName);

				// Get current rssi
				int rssi = -100;
				System.Int32.TryParse (bluetoothAddrAndRssi [1], out rssi);

				// Update beacon information
				beacon.UpdateRssi (rssi);
				//Debug.Log("UPDATED " + beacon.personName + " with RSSI " + rssi);

				// Update the closest beacon
				UpdateClosestBeacon (beacon);
			}
		}
	}

	private void UpdateClosestBeacon (Beacon beacon)
	{
		if (beacon != null) {
			if (beacon.rssi > closestBeacon.rssi) {
				if (!Equals (beacon.personName, closestBeacon.personName)) {
					closestBeacon = beacon;
					Debug.Log ("Closest beacon now " + beacon.personName);
					if (WelcomeText) {
						WelcomeText.text = "Welcome to Pittsburgh, " + beacon.personName + "!";
					}
					webViewController.webView.Load (beacon.url);
					//Debug.Log ("SHOW");
					webViewController.webView.Show ();
				}
			}
		}
	}

	private void DecayRSSIs() {
		if (rssiDecayCountdown <= 0) {
			rssiDecayCountdown = rssiDecayInterval;
			beacons.ForEach (bcn => bcn.RssiDecay ());

			// Hide webview if no beacons in range
			if (closestBeacon != null) {
				if (closestBeacon.rssi < range) {
					webViewController.webView.Hide (); // TODO maybe just delete and recreate
				}
			}
		} else {
			rssiDecayCountdown--;
		}
	}

	// ------------------------------------------------------------------------
	private void DeserializeJSON ()
	{
		// We store our data in the persistent data path, which on the phone I'm using rn is
		// /storage/emulated/0/Android/data/com.OliviaLynn.BeaconAndroidApp/files
		string filePath = System.IO.Path.Combine (Application.persistentDataPath, fileName);
        
		BeaconContainer beaconContainer = JsonUtility.FromJson<BeaconContainer> (File.ReadAllText (filePath)); 

		for (int i = 0; i < beaconContainer.beacons.Count; i++) {
			//Debug.Log ("Deserialized data for: " + beaconContainer.beacons [i].address);
			beacons.Add (new Beacon (
				beaconContainer.beacons [i].address,
				beaconContainer.beacons [i].url,
				beaconContainer.beacons [i].name
			));
		}
	}
}