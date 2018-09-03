using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : MonoBehaviour
{
	private void OnBecameInvisible()
	{
		Destroy(transform.parent.parent.parent.gameObject);
	}
}
