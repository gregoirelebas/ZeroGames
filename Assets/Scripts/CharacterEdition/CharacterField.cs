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

	private void Awake()
	{
		fieldText.text = part.ToString();
	}

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

	public PlayerParts GetPart()
	{
		return part;
	}

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

	public void SetPreviousValue()
	{
		if (target != null)
		{
			valueIndex--;
			if (valueIndex < 0)
			{
				//If we are not setting mesh, we are setting material
				valueIndex = (setMesh ? meshes.Count : materials.Count) - 1;
			}

			SetValueToTarget();
		}
	}

	public void SetNextValue()
	{
		if (target != null)
		{
			valueIndex++;

			//If we are not setting mesh, we are setting material
			if (valueIndex >= (setMesh ? meshes.Count : materials.Count))
			{
				valueIndex = 0;
			}

			SetValueToTarget();
		}
	}
}
