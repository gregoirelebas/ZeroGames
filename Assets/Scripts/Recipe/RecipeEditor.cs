using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecipeEditor : MonoBehaviour
{
	[Header("Interaction")]
	[SerializeField] private PlayerAreaTrigger trigger = null;
	[SerializeField] private TextMeshProUGUI interactText = null;
	[SerializeField] private GameObject panel = null;

	private Player player = null;

	private void OnEnable()
	{
		trigger.OnPlayerEnter += SetInteractionText;
		trigger.OnPlayerExit += HideInteractionText;
	}

	private void Update()
	{
		if (player != null && Keyboard.current.eKey.wasPressedThisFrame)
		{
			player.SetMove(false);
			interactText.text = "";
			panel.SetActive(true);
		}
	}

	private void OnDisable()
	{
		trigger.OnPlayerEnter -= SetInteractionText;
		trigger.OnPlayerExit -= HideInteractionText;
	}

	private void SetInteractionText(Player player)
	{
		this.player = player;
		interactText.text = "Search recipes";
		
	}

	private void HideInteractionText()
	{
		player = null;
		interactText.text = "";
	}
}
