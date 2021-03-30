using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI nameText = null;
	[SerializeField] private Button deleteBtn = null;

	/// <summary>
	/// Set the ingredient name and add listener to the delete button.
	/// </summary>
	public void SetIngredient(RecipeEditor editor, string name)
	{
		nameText.text = name;
		deleteBtn.onClick.AddListener(() =>
		{
			editor.RemoveIngredient(this);
			Destroy(gameObject);
		});
	}

	/// <summary>
	/// Return the ingredient name.
	/// </summary>
	public string GetName()
	{
		return nameText.text;
	}
}
