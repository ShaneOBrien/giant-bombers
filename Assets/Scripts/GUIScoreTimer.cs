using UnityEngine;
using System.Collections;

public class GUIScoreTimer : MonoBehaviour {

	public int Player1Score, Player2Score;

	public string player1, player2;

	public GUISkin gameSkin;

	public float x1, y1, width1, height1, x2;

	// Use this for initialization
	public void AddScoreToPlayer1 (int scoreToAdd)
	{
		Player1Score += scoreToAdd;
	}

	public void AddScoreToPlayer2(int scoreToAdd)
	{
		Player2Score += scoreToAdd;
	}
	
	// Update is called once per frame
	public void Update () {
		player1 = Player1Score.ToString();
		player2 = Player2Score.ToString();
	}

	public void OnGUI()
	{
		GUI.skin = gameSkin;
		GUILayout.BeginArea(new Rect (Screen.width /10, Screen.height / 10f, 100, 100));
		GUILayout.Label(player1);
		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect(Screen.width / 1.2f, Screen.height / 10f, 100, 100));
		GUILayout.Label(player2);
		GUILayout.EndArea();
	}
}
