using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;
using UnityEngine.UI;

public class WeatherController : MonoBehaviour {
	
	/* WEATHER API
	 * 
	 * openweathermap.org
	 * OLynn account name
	 * Rain1234 to enter	 */

	void Start () {
		StartCoroutine (GetWeather ());
		StartCoroutine (GetWeatherFiveDays ());
	}

	IEnumerator GetWeatherFiveDays() {
		WWW www = new WWW("api.openweathermap.org/data/2.5/forecast?zip=15220&units=imperial&APPID=092b6ffafcf0ac72a0973cc27e79f031");
		yield return www;
		//print (www.text);
		PopulatePanelWithThreeDayData (www.text);
	}

	IEnumerator GetWeather() {
		//WWW www = new WWW("api.openweathermap.org/data/2.5/weather?id=4277241&units=imperial&APPID=092b6ffafcf0ac72a0973cc27e79f031"); // pittsburgh id is 4277241
		WWW www = new WWW("api.openweathermap.org/data/2.5/weather?zip=15220&units=imperial&APPID=092b6ffafcf0ac72a0973cc27e79f031"); // using zipcode bc pgh is too big
		yield return www;
		//print (www.text);
		PopulatePanelWithTodayData (www.text);
	}

	void PopulatePanelWithTodayData(string wwwText) {
		var weatherData = JSON.Parse (wwwText);

		//Debug.Log (weatherData);

		string weatherType = weatherData ["weather"][0]["main"];
		float cloudCoverage = weatherData ["clouds"] ["all"].AsFloat;
		int temp = weatherData ["main"] ["temp"].AsInt; // temperatures in F, so long as API call contains "&units=imperial"
		int tempMin = weatherData ["main"] ["temp_min"].AsInt;
		int tempMax = weatherData ["main"]["temp_max"].AsInt;

		DisplayWeatherTypeIcon (weatherType);
		DisplayTempAndType (temp, weatherType);
	}

	void PopulatePanelWithThreeDayData(string wwwText) {
		var weatherData = JSON.Parse (wwwText);
		Debug.Log (weatherData);

		JSONNode intervalList = weatherData ["list"];

		for (int i = 1; i <= 3; i++) {

			// Figure out which day we're looking at
			int targetDay = System.DateTime.Now.DayOfYear + i;
			System.DateTime targetDate = System.DateTime.Now.AddDays (i);

			// The info we'll be grabbing
			int count = 0;
			int lowestLow = 420;
			int highestHigh = 420;

			// Go through our list of 3-hr intervals
			foreach (JSONNode interval in intervalList) {
				
				// Try to parse the given date
				System.DateTime parsedDate;
				if (!System.DateTime.TryParse (interval ["dt_txt"], out parsedDate)) {
					Debug.Log ("Could not parse date: " + interval["dt_txt"]);
				}
				if (parsedDate.DayOfYear == targetDay) {
					
					count++;

					// Get the lowest low
					if (lowestLow == 420) {
						lowestLow = interval ["main"] ["temp_min"].AsInt;
					} else {
						if (lowestLow > interval ["main"] ["temp_min"].AsInt) {
							lowestLow = interval ["main"] ["temp_min"].AsInt;
						}
					}

					// Get the highest high
					if (highestHigh == 420) {
						highestHigh = interval ["main"] ["temp_max"].AsInt;
					} else {
						if (highestHigh < interval ["main"] ["temp_max"].AsInt) {
							highestHigh = interval ["main"] ["temp_max"].AsInt;
						}
					}

				}
			}

			// Update information in the Weather Panel
			UpdateFutureDayData (targetDate, i, lowestLow, highestHigh);
		}

	}

	void UpdateFutureDayData(System.DateTime targetDay, int dayId, int lowTemp, int highTemp) {
		GameObject panel = GameObject.Find ("Day" + dayId + "_Panel");
		if (panel != null) {

			// Update day of the week text
			GameObject dayText = panel.transform.Find ("Day_Text").gameObject;
			if (dayText != null) {
				dayText.GetComponent<UnityEngine.UI.Text> ().text = targetDay.DayOfWeek.ToString();
			} else {
				Debug.Log ("Could not find Day_Text GameObject");
			}

			// Update low temp text
			GameObject lowText = panel.transform.Find ("Low_Text").gameObject;
			if (lowText != null) {
				lowText.GetComponent<UnityEngine.UI.Text> ().text = lowTemp + "°F";
			} else {
				Debug.Log ("Could not find Low_Text GameObject");
			}

			// Update high temp text
			GameObject highText = panel.transform.Find ("High_Text").gameObject;
			if (highText != null) {
				highText.GetComponent<UnityEngine.UI.Text> ().text = highTemp + "°F";
			} else {
				Debug.Log ("Could not find High_Text GameObject");
			}

		} else {
			Debug.Log ("Cannot find day " + dayId + " panel.");
		}
	}

	void DisplayWeatherTypeIcon(string weatherType) {
		Debug.Log ("Weather Type: " + weatherType);
		GameObject IconContainer = GameObject.Find ("WeatherIcons");
		if (IconContainer != null) {
			foreach (Transform child in IconContainer.transform) {
				child.gameObject.SetActive (false);
			}
			switch (weatherType) {
			case "Clear":
				ActivateIconByName (IconContainer, "Sun_Icon");
				break;
			case "Clouds":
				ActivateIconByName (IconContainer, "Cloud_Icon");
				break;
			case "Rain":
				ActivateIconByName (IconContainer, "Rain_Icon");
				break;
			case "Snow":
				ActivateIconByName (IconContainer, "Snow_Icon");
				break;
			}
		} else {
			Debug.Log ("Could not find Icon Container");
		}
	}

	void ActivateIconByName(GameObject IconContainer, string iconName) {
		GameObject icon = IconContainer.transform.Find (iconName).gameObject;
		if (icon != null) {
			icon.SetActive (true);
			//icon.GetComponent<Image>().enabled = true;
		}
		else {
			Debug.Log ("Could not find icon called " + iconName);
		}
	}

	void DisplayTempAndType(int temp, string weatherType) {
		GameObject tempTextField = GameObject.Find ("Temp_Text");
		if (tempTextField != null) {
			tempTextField.GetComponent<UnityEngine.UI.Text> ().text = temp + "°F";
		} else {
			Debug.Log ("Could not find Temperature text field!");
		}

		GameObject typeTextField = GameObject.Find ("Type_Text");
		if (typeTextField != null) {
			typeTextField.GetComponent<UnityEngine.UI.Text> ().text = weatherType;
		} else {
			Debug.Log ("Could not find Weather Type text field!");
		}
	}

	void Update () {
		
	}
}
	