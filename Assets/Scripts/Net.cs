using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Net : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Bubble" || col.gameObject.tag == "BubbleBig")
		{
			Lose();
			Destroy(col.gameObject);
		}
		else if(col.gameObject.tag == "BadBrick")
		{
			Destroy(col.gameObject);
		}
	}

	private void Lose()
	{
		int finalScore = GameObject.FindGameObjectWithTag("GM").GetComponent<ScoreManager>().score;
		PlayerPrefs.SetInt("finalScore", finalScore);
		if(finalScore > PlayerPrefs.GetInt("highScore", 0))
		{
			PlayerPrefs.SetInt("newBest", 1);
			PlayerPrefs.SetInt("highScore", finalScore);
		}
		SceneManager.LoadScene("Scenes/lose");
	}
}
