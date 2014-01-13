using UnityEngine;
using System.Collections;

public class BallCollision : MonoBehaviour {

	public AudioClip hitBoundries;

	public void OnCollisionEnter(Collision col)
	{
		if (!col.collider.CompareTag("Player1") && !col.collider.CompareTag("Player2"))
		{
			audio.PlayOneShot(hitBoundries);
		}

		if (col.collider.CompareTag("3points") && transform.localPosition.x > 0)
		{
			transform.parent.BroadcastMessage("AddScoreToPlayer1", 3);
			col.collider.GetComponent<Animator>().Play("GoalBounceReverse");
			rigidbody.isKinematic = true;
		}
		else if (col.collider.CompareTag("3points") && transform.localPosition.x < 0)
		{
			transform.parent.BroadcastMessage("AddScoreToPlayer2", 3);
			col.collider.GetComponent<Animator>().Play("GoalBounce");
			rigidbody.isKinematic = true;
		}

		if (col.collider.CompareTag("5points") && transform.localPosition.x > 0)
		{
			transform.parent.BroadcastMessage("AddScoreToPlayer1", 5);
			col.collider.GetComponent<Animator>().Play("GoalBounceReverse");
			rigidbody.isKinematic = true;
		}
		else if (col.collider.CompareTag("5points") && transform.localPosition.x < 0)
		{
			transform.parent.BroadcastMessage("AddScoreToPlayer2", 5);
			col.collider.GetComponent<Animator>().Play("GoalBounce");
			rigidbody.isKinematic = true;
		}
	}
}
