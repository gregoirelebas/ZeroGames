using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI nameText = null;
	[SerializeField] private Button deleteBtn = null;

	public void SetIngredient(RecipeEditor editor, string name)
	{
		nameText.text = name;
		deleteBtn.onClick.AddListener(() =>
		{
			editor.RemoveIngredient(this);
			Destroy(gameObject);
		});
	}

	public string GetName()
	{
		return nameText.text;
	}
}
