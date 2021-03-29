using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	[SerializeField] private Transform target = null;

	private Vector3 offset = Vector3.zero;

	private void Start()
	{
		offset = target.position - transform.position;
	}

	private void LateUpdate()
	{
		transform.LookAt(target);
		transform.position = target.position - offset;
	}
}
