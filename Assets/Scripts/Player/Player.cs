using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public enum PlayerParts
{
	Hair,
	Body,
	LeftArm,
	RightArm,
	Count //KEEP AT END
}

public class Player : MonoBehaviour
{
	[SerializeField] private GameObject hairMesh = null;
	[SerializeField] private GameObject bodyMesh = null;
	[SerializeField] private GameObject leftArmMesh = null;
	[SerializeField] private GameObject rightArmMesh = null;

	[Header("NavMesh")]
	[SerializeField] private LayerMask navMeshMask = 1;

	private NavMeshAgent agent = null;
	private Camera mainCamera = null;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		mainCamera = Camera.main; //DEBUG
	}

	private void Update()
	{
		if (!agent.isStopped && Mouse.current.leftButton.isPressed)
		{
			Vector2 mousePosition = Mouse.current.position.ReadValue();

			Ray ray = mainCamera.ScreenPointToRay(mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, navMeshMask))
			{
				agent.SetDestination(hit.point);
			}			
		}
	}

	/// <summary>
	/// Allow or not the player to move with mouse click.
	/// </summary>
	public void SetMove(bool canMove)
	{
		agent.isStopped = !canMove;
	}

	public GameObject GetPart(PlayerParts part)
	{
		switch (part)
		{
			case PlayerParts.Hair:
				return hairMesh;

			case PlayerParts.Body:
				return bodyMesh;

			case PlayerParts.LeftArm:
				return leftArmMesh;

			case PlayerParts.RightArm:
				return rightArmMesh;

			default:
				Debug.LogWarning("Unknown player body part : " + part.ToString() + ", return null!");
				return null;
		}
	}
}
