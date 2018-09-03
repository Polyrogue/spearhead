using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDetructor : MonoBehaviour
{
	[SerializeField]
	private float timeTillDeath;

	// Use this for initialization
	private void Start()
	{
		StartCoroutine(KillSelf());
	}

	private IEnumerator KillSelf()
	{
		yield return new WaitForSeconds(timeTillDeath);
		Destroy(gameObject);
	}
}
