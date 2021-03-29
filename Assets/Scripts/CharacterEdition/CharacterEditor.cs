using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterEditor : MonoBehaviour
{
	[Header("External references")]
	[SerializeField] private PlayerAreaTrigger trigger = null;
	[SerializeField] private CinemachineVirtualCamera vCamera = null;
	[SerializeField] private TextMeshProUGUI interactText = null;

	[Header("Internal references")]
	[SerializeField] private GameObject editPanel = null;
	[SerializeField] private List<CharacterField> fields = null;

	private Player player = null;
	private bool editMode = false;

	private void OnEnable()
	{
		trigger.OnPlayerEnter += GetPlayer;
		trigger.OnPlayerExit += RemovePlayer;
	}

	private void Update()
	{
		if (player != null && Keyboard.current.eKey.wasPressedThisFrame)
		{
			ShowEditMode();
		}
		else if (player != null && Keyboard.current.fKey.wasPressedThisFrame)
		{
			HideEditMode();
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
		interactText.text = "Edit character";

		for (int i = 0; i < (int)PlayerParts.Count; i++)
		{
			CharacterField field = fields.Find(x => (int)x.GetPart() == i);
			if (field != null)
			{
				field.SetTarget(player.GetPart((PlayerParts)i));
			}
		}
	}

	private void RemovePlayer()
	{
		player = null;
		interactText.text = "";
	}

	private void ShowEditMode()
	{
		if (!editMode)
		{
			editMode = true;
			player.SetMove(false);
			vCamera.Priority = 100;
			interactText.text = "";

			editPanel.SetActive(true);
		}
	}

	private void HideEditMode()
	{
		if (editMode)
		{
			editMode = false;
			player.SetMove(true);
			vCamera.Priority = 10;
			interactText.text = "Edit character";

			editPanel.SetActive(false);
		}
	}
}
