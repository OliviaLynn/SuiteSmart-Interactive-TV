using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

public class EventsController : MonoBehaviour {
	/* Ticketmaster API
	 * u: OLynn
	 * p: EVent1234
	 * key: YL9rF33U5wJbx1VXNvpv7tGj2zAF7Fla
	 * */

	void Start () {
		StartCoroutine (GetEvents ());
	}

	public IEnumerator GetEvents() {
		//WWW www = new WWW ("https://app.ticketmaster.com/discovery/v2/events.json?city=Pittsburgh&classificationName=-sports&size=20&apikey=YL9rF33U5wJbx1VXNvpv7tGj2zAF7Fla");
		WWW www = new WWW ("https://app.ticketmaster.com/discovery/v2/events.json?city=Pittsburgh&sort=date,asc&size=20&apikey=YL9rF33U5wJbx1VXNvpv7tGj2zAF7Fla");
		yield return www;
		print (www.text);
		PopulatePanelWithData (www.text);
	}

	void PopulatePanelWithData(string wwwText) {
		JSONNode eventsData = JSON.Parse (wwwText);

		JSONNode events = eventsData ["_embedded"]["events"];

		/*
		foreach (var keyValuePair in events) {
			print (keyValuePair.Value["classifications"][0]["segment"]["name"]);
			print (keyValuePair.Value["dates"]["start"]["localDate"]);
		}
		*/

		GameObject EventCard0 = GameObject.Find ("Event_Card0");
		PopulateEventCard (EventCard0, events[1]); // a bit hacky but I'm 1-indexing to skip this one season-long event I don't want to see. TODO filter these out properly
		GameObject EventCard1 = GameObject.Find ("Event_Card1");
		PopulateEventCard (EventCard1, events[2]);
		GameObject EventCard2 = GameObject.Find ("Event_Card2");
		PopulateEventCard (EventCard2, events[3]);

	}

	void PopulateEventCard(GameObject EventCard, JSONNode eventData) {
		/*
		print (eventData["name"]);
		print (eventData["dates"]["start"]["localDate"]);
		print (eventData["dates"]["start"]["localTime"]);
		print(eventData["images"][0]["url"]);
		*/
		;
		if (EventCard != null) {
			GameObject EventNameTextField = EventCard.transform.Find ("Event_Name").gameObject;
			if (EventNameTextField != null) {
				EventNameTextField.GetComponent<UnityEngine.UI.Text> ().text = eventData ["name"];
			} else {
				Debug.Log ("Could not find Event Name text field");
			}

			GameObject EventDateTextField = EventCard.transform.Find ("Event_Date").gameObject;
			if (EventDateTextField != null) {
				EventDateTextField.GetComponent<UnityEngine.UI.Text> ().text = eventData["dates"]["start"]["localDate"];
			} else {
				Debug.Log ("Could not find Event Date text field");
			}

			GameObject EventImage = EventCard.transform.Find ("Event_Image").gameObject;
			if (EventImage != null) {
				StartCoroutine (SetImage (EventImage, eventData ["images"] [0] ["url"]));
			} else {
				Debug.Log ("Could not find Event image");
			}
		} else {
			Debug.Log ("Could not find Event Card!");
		}
	}

	public IEnumerator SetImage(GameObject EventImage, string url) {
		WWW www = new WWW (url);
		yield return www;

		//Renderer renderer = GetComponent<Renderer> ();
		var img = EventImage.GetComponent<CanvasRenderer>();
		img.SetTexture(www.texture);

	}
	
	void Update () {
		
	}
}
