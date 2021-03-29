using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterEditor : MonoBehaviour
{
	private class CharacterConfig
	{
		public int index = 0;
		public List<int> values = null;

		public CharacterConfig(int index)
		{
			this.index = index;
			values = new List<int>();
		}
	}

	[Header("External references")]
	[SerializeField] private PlayerAreaTrigger trigger = null;
	[SerializeField] private CinemachineVirtualCamera vCamera = null;
	[SerializeField] private TextMeshProUGUI interactText = null;

	[Header("Internal references")]
	[SerializeField] private GameObject editPanel = null;
	[SerializeField] private List<CharacterField> fields = null;
	[SerializeField] private GridLayoutGroup slotGrid = null;

	private Player player = null;
	private bool editMode = false;

	private CharacterConfig currentConfig = null;
	private int lastIndex = 0;
	private List<LoadSlot> loadSlots = new List<LoadSlot>();

	private void Awake()
	{
		for (int i = 0; i < slotGrid.transform.childCount; i++)
		{
			loadSlots.Add(slotGrid.transform.GetChild(i).GetComponent<LoadSlot>());
		}
	}

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
	}

	private void OnDisable()
	{
		trigger.OnPlayerEnter -= GetPlayer;
		trigger.OnPlayerExit -= RemovePlayer;
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

	public void ShowHideLoadPanel()
	{
		slotGrid.gameObject.SetActive(!slotGrid.gameObject.activeSelf);
		if (slotGrid.gameObject.activeSelf)
		{
			for (int i = 0; i < loadSlots.Count; i++)
			{
				loadSlots[i].gameObject.SetActive(false);
			}

			for (int i = 0; i < 2; i++)
			{
				loadSlots[i].gameObject.SetActive(true);
				loadSlots[i].SetSlotIndex(this, i);
			}
		}
	}

	public void LoadConfig(int index)
	{
		Debug.Log("Loading config : " + index);

		currentConfig = new CharacterConfig(index);

		for (int i = 0; i < fields.Count; i++)
		{
			fields[i].SetIndexValue(index + 1);
		}
	}

	public void SaveConfig(bool saveAsNew)
	{
		if (saveAsNew || (!saveAsNew && currentConfig == null))
		{
			currentConfig = new CharacterConfig(lastIndex);
			lastIndex++;
		}

		for (int i = 0; i < fields.Count; i++)
		{
			currentConfig.values.Add(fields[i].GetValueIndex());
		}

		string json = JsonUtility.ToJson(currentConfig);
		Debug.Log(json);

		HideEditMode();
	}
}
