using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	private AudioSource audio;

	[SerializeField]
	private AudioClip[] introSounds;

	private bool SplashDone = false;

	private void Awake()
	{
		StartCoroutine(Splash());
	}

	private void Start()
	{
		audio = GetComponent<AudioSource>();
		AudioClip intro = introSounds[Random.Range(0,introSounds.Length)];
		audio.PlayOneShot(intro);
	}

	private void Update()
	{ 
		if(Input.GetMouseButtonDown(0) && SplashDone)
		{
			SceneManager.LoadScene("Scenes/main");
		}
	}

	private IEnumerator Splash()
	{
		yield return new WaitForSeconds(0.2f);
		SplashDone = true;
	}
}
