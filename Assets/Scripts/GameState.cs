using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public enum State { startGame, player1Ball, player2Ball, player1Scored, player2Scored, gameRunning, gameOver };
	public State currentState;

	public GameObject Ball;
		
	public PlayerController Player1, Player2;
	public GUIScoreTimer gui;

	public string playerScore;

	public GUISkin gameSkin;

	public float countDown = 3.0F;
	public float CountDownDelay = 3.0F;

	public bool firstMessageDisplayed = false, playerSelect = false;

	public static bool gameRunning = false;
	public int playerToStart = 0;

	public Vector3 player1Start, player2Start;

	public AudioClip cheer, clap, horn;

	public string gameText;

	// Use this for initialization
	void Start () {
		countDown = Time.time + CountDownDelay;
		player1Start = Player1.transform.position;
		player2Start = Player2.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState)
		{
			case State.startGame:
			StartGame();
			break;
			case State.player1Ball:
			Player1Ball();
			break;
			case State.player2Ball:
			Player2Ball();
			break;
			case State.player1Scored:
			Score(1);
			break;
			case State.player2Scored:
			Score(2);
			break;
			case State.gameRunning:
			GameRunning();
			break;
			case State.gameOver:
			GameOver();
			break;
		}
	}

	public void StartGame()
	{
		gameRunning = false;
		if (!firstMessageDisplayed && !playerSelect && Time.time <= countDown)
		{
			gameText = "Selecting Player";
			playerToStart = Random.Range(1,3);
			firstMessageDisplayed = true;

		}
		else if (firstMessageDisplayed && !playerSelect && Time.time >= countDown)
		{
			gameText = "Player " + playerToStart.ToString() + " Selected";
			playerSelect = true;
			countDown = Time.time + CountDownDelay;
		}
		else if (firstMessageDisplayed && playerSelect && Time.time > countDown)
		{
			if (playerToStart == 1)
			{
				currentState = State.player1Ball;
				countDown = Time.time + CountDownDelay;
			}
			if (playerToStart == 2)
			{
				currentState = State.player2Ball;
				countDown = Time.time + CountDownDelay;
			}
		}
	}

	public void Player1Ball()
	{
		gameRunning = false;
		gameText = "Go!";
		
		Player1.transform.position = player1Start;
		Player1.transform.rotation = Quaternion.Euler(0, 0, 0);
		Player2.transform.position = player2Start;
		Player2.transform.rotation = Quaternion.Euler(0, 180, 0);
		Ball.transform.localPosition = new Vector3(Player1.transform.localPosition.x + 1f, Player2.transform.localPosition.y, -1f);
		if (Time.time > countDown && playerSelect)
		{
			Player1.preventMovement = true;
			Player2.preventMovement = false;
			gameText = "";
			Player1.m_ballCountDown = Time.time + Player1.m_ballCountDownDelay;
			currentState = State.gameRunning;
		}
	}

	public void Player2Ball()
	{
		gameRunning = false;
		gameText = "Go!";
		Player1.transform.position = player1Start;
		Player1.transform.rotation = Quaternion.Euler(0, 0, 0);
		Player2.transform.position = player2Start;
		Player2.transform.rotation = Quaternion.Euler(0, 180, 0);
		Ball.transform.localPosition = new Vector3(Player2.transform.localPosition.x + -1f, Player2.transform.localPosition.y, -1f);
		if (Time.time > countDown && playerSelect)
		{
			Player1.preventMovement = false;
			Player2.preventMovement = true;
			gameText = "";
			Player2.m_ballCountDown = Time.time + Player2.m_ballCountDownDelay;
			currentState = State.gameRunning;
		}
	}

	public void Score(int playerID)
	{
		gameRunning = false;
		Player1.StopAllCoroutines();
		Player2.StopAllCoroutines();
		if (playerID == 1)
		{
			gameText = "Player 1 Scored!";
			if (Time.time > countDown)
			{
				countDown = Time.time + CountDownDelay;
				currentState = State.player2Ball;
				if (gui.Player2Score >= 12 || gui.Player1Score >= 12)
				{
					countDown = Time.time + CountDownDelay;
					currentState = State.gameOver;
				}
			}
		}
		else if (playerID == 2)
		{
			gameText = "Player 2 Scored!";
			if (Time.time > countDown)
			{
				countDown = Time.time + CountDownDelay;
				currentState = State.player1Ball;
				if (gui.Player2Score >= 12 || gui.Player1Score >= 12)
				{
					countDown = Time.time + CountDownDelay;
					currentState = State.gameOver;
				}
			}
		}
	}

	public void GameRunning()
	{
		gameRunning = true;
		playerScore = "";
		if (gui.Player2Score >= 12 || gui.Player1Score >= 12)
		{
			countDown = Time.time + CountDownDelay;
			currentState = State.gameOver;
		}
	}

	public void AddScoreToPlayer1(int scoreToAdd)
	{
		audio.PlayOneShot(horn);
		//audio.PlayOneShot(cheer);
		audio.PlayOneShot(clap);
		currentState = State.player1Scored;
		countDown = Time.time + CountDownDelay;
	}

	public void AddScoreToPlayer2(int scoreToAdd)
	{
		audio.PlayOneShot(horn);
		//audio.PlayOneShot(cheer);
		audio.PlayOneShot(clap);
		currentState = State.player2Scored;
		countDown = Time.time + CountDownDelay;
	}

	public void GameOver()
	{
		//gameRunning = false;
		Player1.StopAllCoroutines();
		Player2.StopAllCoroutines();
		if (gui.Player1Score > gui.Player2Score)
		{
			gameText = "Player 1 Wins!";
		}
		if (gui.Player2Score > gui.Player1Score)
		{
			gameText = "Player 2 Wins!";
		}
		if (Time.time > countDown)
		{
			gameText = "Press Start To Play Again!";
			if (Input.GetButtonDown("Start_1") || Input.GetButtonDown("Start_2"))
			{
				Application.LoadLevel(0);
			}
		}

	}

	public void OnGUI()
	{
		GUI.skin = gameSkin;
		GUI.skin.label.fontSize = 100;
		GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 500), gameText);
		GUI.Label(new Rect(Screen.width/2 - 200, Screen.height/2 - 200, 400, 500), playerScore);
	}
}
