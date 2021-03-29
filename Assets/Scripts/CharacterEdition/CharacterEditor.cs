using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterEditor : MonoBehaviour
{
	[SerializeField] private PlayerAreaTrigger trigger = null;
	[SerializeField] private CinemachineVirtualCamera vCamera = null;

	private Player player = null;
	private bool editMode = false;

	private void OnEnable()
	{
		trigger.OnPlayerEnter += GetPlayer;
		trigger.OnPlayerExit += RemovePlayer;
	}

	private void Update()
	{
		if (player != null && !editMode && Keyboard.current.eKey.wasPressedThisFrame)
		{
			editMode = true;
			player.SetMove(false);
			vCamera.Priority = 100;
		}
		else if (player != null && editMode && Keyboard.current.fKey.wasPressedThisFrame)
		{
			editMode = false;
			player.SetMove(true);
			vCamera.Priority = 10;
		}
	}

	private void OnDisable()
	{
		trigger.OnPlayerEnter += GetPlayer;
		trigger.OnPlayerExit += RemovePlayer;
	}

	private void GetPlayer(Player player)
	{
		this.player = player;
	}

	private void RemovePlayer()
	{
		player = null;
	}
}
