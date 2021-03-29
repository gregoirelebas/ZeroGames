using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
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
			if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
			{
				agent.SetDestination(hit.point);
			}			
		}
	}

	//DEBUG
	private void OnDrawGizmos()
	{
		if (agent != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(agent.destination, 0.5f);
		}
	}

	/// <summary>
	/// Allow or not the player to move with mouse click.
	/// </summary>
	public void SetMove(bool canMove)
	{
		agent.isStopped = !canMove;
	}
}
