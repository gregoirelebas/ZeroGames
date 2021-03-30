using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class RecipeEditor : MonoBehaviour
{
	private class Recipe
	{
		public string title = "";
		public string href = "";
		public string ingredients = "";
		public string thumbnail = "";
	}

	private class RecipeSearchResult
	{
		public string title = "";
		public float version = 0.0f;
		public string href = "";
		public List<Recipe> results = null;
	}

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
	[SerializeField] private TMP_InputField recipeField = null;

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

			ClearIngredientDisplay();
			ClearRecipeDisplay();
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

	private void ClearIngredientDisplay()
	{
		for (int i = 0; i < ingredientListContainer.childCount; i++)
		{
			Destroy(ingredientListContainer.GetChild(i).gameObject);
		}
	}

	private void ClearRecipeDisplay()
	{
		for (int i = 0; i < recipeGridContainer.childCount; i++)
		{
			Destroy(recipeGridContainer.GetChild(i).gameObject);
		}
	}

	private IEnumerator SearchForRecipes(string url)
	{
		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{
			yield return request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
			}
			else
			{
				string json = request.downloadHandler.text;

				RecipeSearchResult search = JsonConvert.DeserializeObject<RecipeSearchResult>(json);
				for (int i = 0; i < search.results.Count; i++)
				{
					Recipe recipe = search.results[i];
					AddRecipe(recipe.title, recipe.thumbnail);
				}
			}
		}
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

	public void AddRecipe(string title, string url)
	{
		GameObject go = Instantiate(recipeSlotPrefab, recipeGridContainer);
		RecipeSlot newSlot = go.GetComponent<RecipeSlot>();

		newSlot.SetRecipe(title, url);
	}

	public void Search()
	{
		ClearRecipeDisplay();

		string search = "http://www.recipepuppy.com/api/";
		bool hasIngredient = false;

		for (int i = 0; i < ingredientSlots.Count; i++)
		{
			//First add "?i="
			if (i == 0)
			{
				search += "?i=";
				hasIngredient = true;
			}
			//Then add "," except for the last one
			else if (i > 0 && i < ingredientSlots.Count - 1)
			{
				search += ",";
			}

			search += ingredientSlots[i].GetName();
		}

		if (recipeField.text != "")
		{
			if (hasIngredient)
			{
				search += "&";
			}

			search += "q=" + recipeField.text;
		}

		StartCoroutine(SearchForRecipes(search));
	}
}
