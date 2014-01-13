using UnityEngine;
using System.Collections;

public class PlayerTrigger : MonoBehaviour {

	public PlayerController playerController;

	public void Start()
	{
		playerController = transform.parent.GetComponent<PlayerController>();
	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Ball"))
		{
			col.rigidbody.isKinematic = true;
			playerController.preventMovement = true;
			playerController.m_ballCountDown = Time.time + 3;
		}
	}
}
