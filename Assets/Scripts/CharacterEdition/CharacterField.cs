using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterField : MonoBehaviour
{
	[SerializeField] private PlayerParts part = PlayerParts.Hair;
	[SerializeField] private TextMeshProUGUI fieldText = null;

	[Header("Meshes")]
	[SerializeField] private List<Mesh> meshes = null;
	[SerializeField] private bool setMesh = false;

	[Header("Materials")]
	[SerializeField] private List<Material> materials = null;
	[SerializeField] private bool setMaterial = false;

	private GameObject target = null;
	private MeshFilter meshFilter = null;
	private MeshRenderer meshRenderer = null;

	private int valueIndex = 0;
	private int maxValue = 0;

	private void Awake()
	{
		fieldText.text = part.ToString();

		//If we are not setting mesh, we are setting material
		maxValue = setMesh ? meshes.Count : materials.Count;
	}

	/// <summary>
	/// Updated mesh and/or material based on current index.
	/// </summary>
	private void SetValueToTarget()
	{
		if (setMesh)
		{
			meshFilter.mesh = meshes[valueIndex];
		}

		if (setMaterial)
		{
			meshRenderer.material = materials[valueIndex];
		}
	}

	/// <summary>
	/// Return the player part assigned to this slot.
	/// </summary>
	public PlayerParts GetPart()
	{
		return part;
	}

	/// <summary>
	/// Return the current value index.
	/// </summary>
	public int GetValueIndex()
	{
		return valueIndex;
	}

	/// <summary>
	/// Set the target to edit and try to get MeshRenderer and MeshFilter.
	/// </summary>
	public void SetTarget(GameObject target)
	{
		this.target = target;

		if (setMesh)
		{
			meshFilter = target.GetComponent<MeshFilter>();
		}

		if (setMaterial)
		{
			meshRenderer = target.GetComponent<MeshRenderer>();
		}
	}

	/// <summary>
	/// Decrease value index by 1 and apply modifications.
	/// </summary>
	public void SetPreviousValue()
	{
		if (target != null)
		{
			valueIndex--;
			if (valueIndex < 0)
			{				
				valueIndex = maxValue - 1;
			}

			SetValueToTarget();
		}
	}

	/// <summary>
	/// Increase value index by 1 and apply modifications.
	/// </summary>
	public void SetNextValue()
	{
		if (target != null)
		{
			valueIndex++;

			if (valueIndex >= maxValue)
			{
				valueIndex = 0;
			}

			SetValueToTarget();
		}
	}

	/// <summary>
	/// Set the value index and apply modifications.
	/// </summary>
	public void SetIndexValue(int value)
	{
		if (value >= 0 && value < maxValue)
		{
			valueIndex = value;
			SetValueToTarget();
		}
	}
}
