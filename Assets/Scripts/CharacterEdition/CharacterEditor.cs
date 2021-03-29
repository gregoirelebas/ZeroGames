using Cinemachine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterEditor : MonoBehaviour
{
	[System.Serializable]
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

	[Header("Player interaction")]
	[SerializeField] private PlayerAreaTrigger trigger = null;
	[SerializeField] private CinemachineVirtualCamera vCamera = null;
	[SerializeField] private TextMeshProUGUI interactText = null;

	[Header("Edition")]
	[SerializeField] private GameObject editPanel = null;
	[SerializeField] private List<CharacterField> fields = null;

	[Header("Loading")]
	[SerializeField] private GridLayoutGroup slotGrid = null;

	private Player player = null;
	private bool editMode = false;

	private List<CharacterConfig> allConfigs = new List<CharacterConfig>();
	private CharacterConfig currentConfig = null;

	private List<LoadSlot> loadSlots = new List<LoadSlot>();

	private string directoryPath = "Character";
	private string filePath = "characterConfiguration.json";

	private void Awake()
	{
		for (int i = 0; i < slotGrid.transform.childCount; i++)
		{
			loadSlots.Add(slotGrid.transform.GetChild(i).GetComponent<LoadSlot>());
		}

		LoadCharacterFile();
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

	private void OnDestroy()
	{
		WriteCharacterFile();
	}

	/// <summary>
	/// Get the player to edit and set slot fields.
	/// </summary>
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

	/// <summary>
	/// Clear the player reference.
	/// </summary>
	private void RemovePlayer()
	{
		player = null;
		interactText.text = "";
	}

	/// <summary>
	/// Show the edition panel and enable the edition camera.
	/// </summary>
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

	/// <summary>
	/// Hide the edition panel and disable the edition camera.
	/// </summary>
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

	/// <summary>
	/// Write all configurations saved in a file.
	/// </summary>
	private void WriteCharacterFile()
	{
		string path = Path.Combine(Application.persistentDataPath, directoryPath);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		path = Path.Combine(path, filePath);


		string json = JsonConvert.SerializeObject(allConfigs);
		File.WriteAllText(path, json);
	}

	/// <summary>
	/// Read the file and load all configurations saved.
	/// </summary>
	private void LoadCharacterFile()
	{
		string path = Path.Combine(Application.persistentDataPath, directoryPath);
		if (Directory.Exists(path))
		{
			path = Path.Combine(path, filePath);
			if (File.Exists(path))
			{
				string json = File.ReadAllText(path);
				allConfigs = JsonConvert.DeserializeObject<List<CharacterConfig>>(json);
			}
		}
	}

	/// <summary>
	/// Show or Hide the load panel and set load slots.
	/// </summary>
	public void ShowHideLoadPanel()
	{
		slotGrid.gameObject.SetActive(!slotGrid.gameObject.activeSelf);
		if (slotGrid.gameObject.activeSelf)
		{
			for (int i = 0; i < loadSlots.Count; i++)
			{
				loadSlots[i].gameObject.SetActive(false);
			}

			for (int i = 0; i < allConfigs.Count; i++)
			{
				loadSlots[i].gameObject.SetActive(true);
				loadSlots[i].SetSlotIndex(this, i);
			}
		}
	}

	/// <summary>
	/// Load the character configuration and apply it on player.
	/// </summary>
	public void LoadConfig(int index)
	{
		if (index >= 0 && index < allConfigs.Count)
		{
			currentConfig = allConfigs[index];

			for (int i = 0; i < fields.Count; i++)
			{
				fields[i].SetIndexValue(currentConfig.values[i]);
			}
		}
	}

	/// <summary>
	/// Save the current configuration. Can be saved as new or overwrite an existing one.
	/// </summary>
	public void SaveConfig(bool save)
	{
		if (save || currentConfig == null)
		{
			currentConfig = new CharacterConfig(allConfigs.Count);
		}

		for (int i = 0; i < fields.Count; i++)
		{
			currentConfig.values.Add(fields[i].GetValueIndex());
		}

		if (save)
		{
			//Already existing => overwrite
			if (currentConfig.index < allConfigs.Count)
			{
				allConfigs[currentConfig.index] = currentConfig;
			}
			//Not existing yet, create a new one.
			else
			{
				allConfigs.Add(currentConfig);
			}
		}

		HideEditMode();
	}
}
