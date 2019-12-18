using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {

	void Start () {
		
	}

	void Update () {
		
	}

	public void doExitGame() {
		Debug.Log ("Exiting game...");
		Application.Quit();
	}
}
