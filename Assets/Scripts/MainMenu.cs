using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public string p1PressStart, p2PressStart;

	public GUITexture logo;

	public bool p1Ready, p2Ready, showControls, showMainMenu;

	// Use this for initialization
	void Start () {
		logo.pixelInset = new Rect(0,0, Screen.width, Screen.height);
		//GameState.gameRunning = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown("Start_1"))
		{
			p1Ready = true;
			p1PressStart = "Player 1\nReady";
		}

		if (Input.GetButtonDown("Start_2"))
		{
			p2Ready = true;
			p2PressStart = "Player 2\nReady";
		}

		if (p1Ready && p2Ready)
		{
			Application.LoadLevel(1);
		}
	}

	public void OnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width / 1.75f, Screen.height / 1.5f, 200, 150));
		GUILayout.BeginHorizontal();
		GUILayout.Label(p1PressStart);
		GUILayout.Label(p2PressStart);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}
