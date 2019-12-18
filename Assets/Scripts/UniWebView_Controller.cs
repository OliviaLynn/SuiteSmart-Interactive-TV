using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniWebView_Controller : MonoBehaviour
{
    public UniWebView webView;
	public GameObject TargetPanel;
    
    void Start()
    {
        GameObject WebViewGameObject = new GameObject("UniWebView");
        webView = WebViewGameObject.AddComponent<UniWebView>();

		if (TargetPanel != null) {
			webView.ReferenceRectTransform = TargetPanel.GetComponent<RectTransform>();
		} else {
			webView.Frame = new Rect (0, 0, Screen.width, 0.8f * Screen.height);
		}
        webView.Load("https://www.netflix.com");
        webView.Show();

        webView.OnPageFinished += (view, statusCode, url) =>
        {
        };

        webView.OnShouldClose += (view) =>
        {
            webView = null;
            return true;
        };

        webView.OnMessageReceived += (view, message) =>
        {
            //if (message.Path.Equals("game-over"))
            //{
            //    var score = message.Args["score"];
            //    Debug.Log("Your final score is: " + score);

            //    // Restart game
            //    Invoke("Restart", 3.0f);
            //}
            //else if (message.Path.Equals("close"))
            //{
            //    CloseWebView();
            //}
        };

    }

	void PrintSize() {

		Debug.Log (TargetPanel.GetComponent<RectTransform>().rect);
	}

    public void NavigateToUrl(string url)
    {
        if (webView != null)
        {
            webView.Load(url);
        }

    }

    void CloseWebView()
    {
        Destroy(webView);
        webView = null; 
    }

    private void OnDestroy()
    {
        CloseWebView();
    }

    void Restart()
    {
        if (webView != null)
        {
            webView.Reload();
        }
    }

    void Update()
    {
        
    }
}
