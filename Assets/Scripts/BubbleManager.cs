using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleManager : MonoBehaviour
{
	[SerializeField]
	private GameObject[] bubbles;// 0 normal 1 big 2 little 3 PUspear 4 PUtime 5 bomb

	[SerializeField]
	private float spawnRate;

	private Camera cam;

	private int spawnsWithoutPowerup;

	private int spawnsTillNextPowerup;

	//Brick stuff
	private int spawnsTillNextBrick;

	private int spawnsWithoutBrick;

	[SerializeField]
	private GameObject brick;

	//spawn scaling stuff
	private float spawnRateLimit;

	private bool decked;

	//[SerializeField]
	//private TextMeshProUGUI debugText;

	private void Start()
	{
		spawnRateLimit = 1.1f;
		decked = false;
		cam = Camera.main;
		StartCoroutine("Spawn");
		spawnsTillNextPowerup = Random.Range(5, 10);
		spawnsTillNextBrick = Random.Range(18, 25);
	}

	private void Update()
	{
		//debugText.text = spawnRate.ToString();
		//Debug.Log(spawnRate);
	}

	private IEnumerator Spawn()
	{
		while(true)
		{
			// only spawns a powerup every spawnsTill bubbles
			yield return new WaitForSeconds(spawnRate);
			if(spawnRate > spawnRateLimit)
			{
				//difficulty scaling here
				if(decked)
				{
					decked = false;
				}
				else
				{
					decked = true;
					spawnRate -= 0.05f;
				}
			}
			Vector3 pos = cam.ScreenToWorldPoint(new Vector3(Random.Range(60, Screen.width - 60), Screen.height + 200, 50));
			int index = 0;
			//first determine if it should be a powerup or normal bubble
			if(spawnsWithoutPowerup < spawnsTillNextPowerup)
			{
				//spawn normal bad bubble
				spawnsWithoutPowerup++;
				index = Random.Range(0, 3);
			}
			else
			{
				//spawn powerup bubble
				spawnsWithoutPowerup = 0;
				spawnsTillNextPowerup = Random.Range(5, 10);
				index = Random.Range(3, 6);
			}
			Instantiate(bubbles[index], pos, Quaternion.identity);

			//Badbrick stuff
			if(spawnsWithoutBrick > spawnsTillNextBrick)
			{
				spawnsWithoutBrick = 0;
				spawnsTillNextBrick = Random.Range(8, 10);
				Vector3 brickPos = cam.ScreenToWorldPoint(new Vector3(Random.Range(60, Screen.width - 60), Screen.height + 200, 50));
				Instantiate(brick, brickPos, Quaternion.identity);
			}
			else
			{
				spawnsWithoutBrick++;
			}
		}
	}
}
