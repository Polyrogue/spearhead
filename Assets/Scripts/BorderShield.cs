using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderShield : MonoBehaviour
{
	[SerializeField]
	private GameObject PS;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Bubble" || col.gameObject.tag == "BubbleBig")
		{
			FindObjectOfType<AudioManager>().Play("shield");
			Instantiate(PS, col.transform.position, Quaternion.identity);
			Destroy(col.gameObject);
			Destroy(gameObject);
		}
	}
}
