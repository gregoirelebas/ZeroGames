using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI nameText = null;
	[SerializeField] private RawImage image = null;
	[SerializeField] private Image placeholder = null;

	/// <summary>
	/// Send a request to download a texture at url and display it on image.
	/// </summary>
	private IEnumerator LoadImageFromURL(string url)
	{
		using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
		{
			yield return request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
			}
			else
			{
				image.texture = DownloadHandlerTexture.GetContent(request);
			}
		}
	}

	/// <summary>
	/// Set the recipe name and image. If url is empty, display placeholder.
	/// </summary>
	public void SetRecipe(string name, string imageURL)
	{
		nameText.text = name;
		if (imageURL != null && imageURL != "")
		{
			image.gameObject.SetActive(true);
			placeholder.gameObject.SetActive(false);

			StartCoroutine(LoadImageFromURL(imageURL));
		}
		else
		{
			image.gameObject.SetActive(false);
			placeholder.gameObject.SetActive(true);
		}
	}
}
