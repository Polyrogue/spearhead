using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI scoreLabel;

	[SerializeField]
	private TextMeshProUGUI highScoreLabel;

	public int score = 0;

	//PLACEHOLDER
	[SerializeField]
	private GameObject explosion;

	private void Start()
	{	
		FindObjectOfType<AudioManager>().Play("bgm");
		int index = Random.Range(1, 4);
		FindObjectOfType<AudioManager>().Play("begin" + index);
		highScoreLabel.SetText("Best: " + PlayerPrefs.GetInt("highScore").ToString());
	}

	// Update is called once per frame
	private void Update()
	{
		scoreLabel.SetText(score.ToString());

		if(Input.GetMouseButtonDown(1))
		{
			Time.timeScale = 0.5f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
		if(Input.GetMouseButtonUp(1))
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
	}
}
