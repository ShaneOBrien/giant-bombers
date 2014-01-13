using UnityEngine;
using System.Collections;

public class diveClamp : MonoBehaviour {

	public enum Player { playerOne, playerTwo };
	public Player currentPlayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentPlayer)
		{
			case Player.playerOne:
			transform.localPosition = new Vector3(0, 0, 2.5f);
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4, 4), transform.position.y, Mathf.Clamp(transform.position.z, -7, -2));
			break;
			case Player.playerTwo:
			transform.localPosition = new Vector3(0, 0, 2.5f);
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4, 4), transform.position.y, Mathf.Clamp(transform.position.z, 2, 7));
			
			break;
		}
		
	}
}
