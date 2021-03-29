﻿using System.Collections;
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

	[Header("Ingredients")]
	[SerializeField] private GameObject ingredientSlotPrefab = null;
	[SerializeField] private Transform ingredientListContainer = null;
	[SerializeField] private TMP_InputField ingredientField = null;

	[Header("Recipes")]
	[SerializeField] private GameObject recipeSlotPrefab = null;
	[SerializeField] private Transform recipeGridContainer = null;

	private Player player = null;
	private bool editMode = false;

	private List<IngredientSlot> ingredientSlots = new List<IngredientSlot>();

	private void OnEnable()
	{
		trigger.OnPlayerEnter += SetInteractionText;
		trigger.OnPlayerExit += HideInteractionText;
	}

	private void Update()
	{
		if (!editMode && player != null && Keyboard.current.eKey.wasPressedThisFrame)
		{
			player.SetMove(false);
			interactText.text = "";
			panel.SetActive(true);
			editMode = true;

			for (int i = 0; i < ingredientListContainer.childCount; i++)
			{
				Destroy(ingredientListContainer.GetChild(i).gameObject);
			}

			for (int i = 0; i < recipeGridContainer.childCount; i++)
			{
				Destroy(recipeGridContainer.GetChild(i).gameObject);
			}

			AddRecipe(); //DEBUG
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

	public void AddIngredient()
	{
		if (ingredientField.text != "")
		{
			GameObject go = Instantiate(ingredientSlotPrefab, ingredientListContainer);
			IngredientSlot newSlot = go.GetComponent<IngredientSlot>();

			newSlot.SetIngredient(this, ingredientField.text);
			ingredientSlots.Add(newSlot);

			ingredientField.text = "";
		}
	}

	public void RemoveIngredient(IngredientSlot toRemove)
	{
		ingredientSlots.Remove(toRemove);
	}

	public void AddRecipe()
	{
		GameObject go = Instantiate(recipeSlotPrefab, recipeGridContainer);
		RecipeSlot newSlot = go.GetComponent<RecipeSlot>();

		newSlot.SetRecipe("Vegetable-Pasta Oven Omelet", "http://img.recipepuppy.com/560556.jpg");
	}
}
