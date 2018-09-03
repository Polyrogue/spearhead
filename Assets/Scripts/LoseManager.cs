using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoseManager : MonoBehaviour
{
	private TextMeshProUGUI fSLabel;

	[SerializeField]
	private TextMeshProUGUI hsLabel;

	[SerializeField]
	private GameObject newBest;

	private bool cooldown;

	// Use this for initialization
	private void Start()
	{
		fSLabel = GetComponent<TextMeshProUGUI>();
		fSLabel.SetText("Final Score\n" + PlayerPrefs.GetInt("finalScore").ToString());

		hsLabel.SetText("Best\n" + PlayerPrefs.GetInt("highScore").ToString());

		if(PlayerPrefs.GetInt("newBest", 0) == 1)
		{
			PlayerPrefs.SetInt("newBest", 0);
			newBest.SetActive(true);
		}

		StartCoroutine(loseCooldown());
		FindObjectOfType<AudioManager>().Stop("bgm");
		FindObjectOfType<AudioManager>().Play("lose");
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0) && !cooldown)
		{
			SceneManager.LoadScene("Scenes/main");
		}
	}

	private IEnumerator loseCooldown()
	{
		cooldown = true;
		yield return new WaitForSeconds(1f);
		cooldown = false;
	}
}
