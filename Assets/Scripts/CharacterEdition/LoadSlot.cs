using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slotText = null;

	private Button slotBtn = null;

	private void Awake()
	{
		slotBtn = GetComponent<Button>();
	}

	private void OnDisable()
	{
		slotBtn.onClick.RemoveAllListeners();
	}

	public void SetSlotIndex(CharacterEditor editor, int index)
	{
		slotBtn.onClick.AddListener(() => editor.LoadConfig(index));

		slotText.text = (index + 1).ToString();
	}
}
