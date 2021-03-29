using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAreaTrigger : MonoBehaviour
{
	public System.Action<Player> OnPlayerEnter = null;
	public System.Action OnPlayerExit = null;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Player player = other.GetComponent<Player>();

			OnPlayerEnter?.Invoke(player);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			OnPlayerExit?.Invoke();
		}
	}
}
