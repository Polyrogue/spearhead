using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleVisibility : MonoBehaviour
{
	private bool visible = false;

	private void OnBecameVisible()
	{
		visible = true;
	}

	public bool CheckVisibility()
	{
		return visible;
	}
}
