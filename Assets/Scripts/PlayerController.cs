using UnityEngine;
using System.Collections;

[AddComponentMenu("STW/Player/RPG Controller")]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

	public enum playerNumber { PlayerOne, PlayerTwo };
	public playerNumber whichPlayer;
	private CharacterController m_controller;

	public Transform CameraRotation;

	public float topSpeed = 3.25f, startSpeed = 1f,	acceleration = 3f, jumpSpeed = 5f, currentSpeed = 1f;
	public float gravity = 20.0f;

	private Vector3 m_moveDirection = Vector3.zero; //creates x,y,z values
	public Vector3 m_moveVector = Vector3.zero;
	private Quaternion m_rotateOffset = Quaternion.identity;

	private float m_horizontalInput, m_verticalInput;

	public bool preventMovement = false, isDiving = false;

	public GameObject Ball, diveTargetGO;

	public AudioClip throwBall, yell;

	public float m_ballCountDown = 0;
	public float m_ballCountDownDelay = 3.0F;

	public void Start()
	{
		m_controller = GetComponent<CharacterController>();
	}

	public void Update()
	{
		
		switch (whichPlayer)
		{
			case playerNumber.PlayerOne:
			transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -7, -2), transform.localPosition.y, transform.localPosition.z);
			if (!preventMovement && !isDiving)
			{
				P1Movement();
			}
			else if (!preventMovement && isDiving)
			{
				
			}
			else if(GameState.gameRunning)
			{
				P1HasBall();
			}
			break;
			case playerNumber.PlayerTwo:
			transform.localPosition = new Vector3 (Mathf.Clamp(transform.localPosition.x, 2, 7),transform.localPosition.y, transform.localPosition.z);
			if (!preventMovement)
			{
				P2Movement();
			}
			else if (!preventMovement && isDiving)
			{

			}
			else if(GameState.gameRunning)
			{
				P2HasBall();
			}
			break;
		}
		
	}

	public void P1Movement()
	{
		if (Input.GetButtonDown("A_1") || Input.GetButtonDown("space"))
		{
			StartCoroutine(Dive(transform.position, diveTargetGO.transform.position, 25));
		}
		m_rotateOffset = Quaternion.Euler(new Vector3(0, CameraRotation.rotation.eulerAngles.y, 0));
		m_horizontalInput = Input.GetAxisRaw("L_XAxis_1");
		m_verticalInput = -Input.GetAxisRaw("L_YAxis_1");
		m_moveVector = new Vector3(m_horizontalInput, 0f, m_verticalInput).normalized;
		m_moveVector = m_rotateOffset * m_moveVector;
		SetAccelaration();
		SetRotation();
		m_moveDirection = new Vector3(0f, m_moveDirection.y, 0f);
		m_moveDirection.y -= gravity * Time.deltaTime;
		m_controller.Move(m_moveVector * Time.deltaTime * currentSpeed);
		m_controller.Move(m_moveDirection * Time.deltaTime);
		m_ballCountDown = Time.time + m_ballCountDownDelay;
	}
	public void P2Movement()
	{
		if (Input.GetButtonDown("A_2") || Input.GetButtonDown("0"))
		{
			StartCoroutine(Dive(transform.position, diveTargetGO.transform.position, 25));
		}
		m_rotateOffset = Quaternion.Euler(new Vector3(0, CameraRotation.rotation.eulerAngles.y, 0));
		m_horizontalInput = Input.GetAxisRaw("L_XAxis_2");
		m_verticalInput = -Input.GetAxisRaw("L_YAxis_2");
		m_moveVector = new Vector3(m_horizontalInput, 0f, m_verticalInput).normalized;
		m_moveVector = m_rotateOffset * m_moveVector;
		SetAccelaration();
		SetRotation();
		m_moveDirection = new Vector3(0f, m_moveDirection.y, 0f);
		m_moveDirection.y -= gravity * Time.deltaTime;
		m_controller.Move(m_moveVector * Time.deltaTime * currentSpeed);
		m_controller.Move(m_moveDirection * Time.deltaTime);
		m_ballCountDown = Time.time + m_ballCountDownDelay;
	}

	public void P1HasBall()
	{
		if (Input.GetButton("A_1") || Input.GetButtonDown("space") || Time.time >= m_ballCountDown)
		{
			m_rotateOffset = Quaternion.Euler(new Vector3(0, CameraRotation.rotation.eulerAngles.y, 0));
			m_horizontalInput = Input.GetAxisRaw("L_XAxis_1");
			m_verticalInput = -Input.GetAxisRaw("L_YAxis_1");
			m_moveVector = new Vector3(m_horizontalInput, 0f, m_verticalInput).normalized;
			m_moveVector = m_rotateOffset * m_moveVector;
			Ball.rigidbody.isKinematic = false;
			float multiplyer = 0;
			if (m_moveVector == new Vector3(0, 0, 0))
			{
				m_moveVector = new Vector3(0, 0, -200);
			}
			if (m_ballCountDown > 0)
			{
				multiplyer = (m_ballCountDown - Time.time) / 1.5f;
			}
			Ball.rigidbody.AddForce(new Vector3(m_moveVector.x, 0, m_moveVector.z) * 750 * multiplyer);
			audio.PlayOneShot(throwBall);
			audio.PlayOneShot(yell);
			preventMovement = false;
		}
	}

	public void P2HasBall()
	{
		if (Input.GetButton("A_2") || Input.GetButtonDown("0") || Time.time >= m_ballCountDown)
		{
			m_rotateOffset = Quaternion.Euler(new Vector3(0, CameraRotation.rotation.eulerAngles.y, 0));
			m_horizontalInput = Input.GetAxisRaw("L_XAxis_2");
			m_verticalInput = -Input.GetAxisRaw("L_YAxis_2");
			m_moveVector = new Vector3(m_horizontalInput, 0f, m_verticalInput).normalized;
			m_moveVector = m_rotateOffset * m_moveVector;
			Ball.rigidbody.isKinematic = false;
			float multiplyer = 0;
			if (m_moveVector == new Vector3(0, 0, 0))
			{
				m_moveVector = new Vector3(0, 0, 200);
			}
			if (m_ballCountDown >= 1)
			{
				multiplyer = (m_ballCountDown - Time.time) / 1.5f;
			}
			Ball.rigidbody.AddForce(new Vector3(m_moveVector.x, 0, m_moveVector.z) * 750 * multiplyer);
			audio.PlayOneShot(throwBall);
			audio.PlayOneShot(yell);
			preventMovement = false;
		}
	}

	public IEnumerator Dive(Vector3 originalPosition, Vector3 newPosition, float smoothing)
	{
		while (Vector3.Distance(transform.position, newPosition) > 0.05f)
		{
			isDiving = true;
			transform.position = Vector3.Lerp(transform.position, newPosition, smoothing * Time.deltaTime);
			isDiving = false;
			yield return null;
		}
	}

	private void SetAccelaration()
	{
		if (m_moveVector.magnitude > 0)
		{
			currentSpeed += (Time.deltaTime * acceleration);
			currentSpeed = Mathf.Clamp(currentSpeed, startSpeed, topSpeed);
		}
		else
		{
		}
	}

	private void SetRotation()
	{
		if (m_moveVector.magnitude > 0.01f)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_moveVector), Time.deltaTime * 50f);
		}
	}


}
