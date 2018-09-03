using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	[SerializeField]
	private float maxRadius;

	//amount of time between loops
	[SerializeField]
	private float explosionRate;

	//amount size increases every loop
	[SerializeField]
	private float explosionForce;

	[SerializeField]
	private GameObject bubblePU;

	// Use this for initialization
	private void Start()
	{
		StartCoroutine(Explode());
	}

	private void Update()
	{
		if(transform.localScale.x > maxRadius)
		{
			Destroy(gameObject);
		}
	}

	private IEnumerator Explode()
	{
		while(true)
		{
			yield return new WaitForSeconds(explosionRate);
			transform.localScale = new Vector3(transform.localScale.x + explosionForce, transform.localScale.y + explosionForce, transform.localScale.z);
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Bubble" || col.gameObject.tag == "BubbleBig" || col.gameObject.tag == "BubblePUSpear")
		{
			Instantiate(bubblePU, col.transform.position, Quaternion.identity);
			Destroy(col.gameObject);
		}
	}
}
