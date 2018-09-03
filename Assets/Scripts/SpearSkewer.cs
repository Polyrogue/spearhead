using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using TMPro;

public class SpearSkewer : MonoBehaviour
{
	[SerializeField]
	private GameObject bubbleParticleExplosion;

	[SerializeField]
	private GameObject bigBubbleParticleExplosion;

	[SerializeField]
	private GameObject smallBubbleParticleExplosion;

	[SerializeField]
	private GameObject SpearPUParticleExplosion;

	[SerializeField]
	private GameObject timePUParticleExplosion;

	[SerializeField]
	private GameObject bombParticleExplosion;

	[SerializeField]
	private GameObject canvas;

	[SerializeField]
	private GameObject scoreMultiplierLabel;

	public GameObject spearParticles;


	private bool flying = false;

	private bool skewerable = true;

	private int scoreMultiplier;

	private void Start()
	{
		canvas = GameObject.FindGameObjectWithTag("Canvas");
		scoreMultiplier = 0;
	}

	private void Update()
	{
		if(spearParticles)
		{
			spearParticles.transform.position = transform.parent.transform.position;
			spearParticles.transform.GetChild(0).transform.position = transform.parent.position;
		}
	}

	public void EnableFlight()
	{
		FindObjectOfType<AudioManager>().PlayRandomPitch("launch", 1f, 1f);
		flying = true;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(flying && skewerable && col.gameObject.GetComponent<BubbleVisibility>())
		{
			ParticleSystem.MainModule main = spearParticles.GetComponentInChildren<ParticleSystem>().main;
			main.loop = false;
			if(col.gameObject.GetComponent<BubbleVisibility>().CheckVisibility())
			{
				//TODO convert this to a switch statement
				scoreMultiplier++;
				if(col.gameObject.tag == "Bubble")
				{
					FindObjectOfType<AudioManager>().PlayRandomPitch("pop", 0.9f, 1.1f);
					CameraShaker.Instance.ShakeOnce(1f, 1.5f, 0.1f, 0.75f);
					GameObject.FindGameObjectWithTag("GM").GetComponent<ScoreManager>().score += scoreMultiplier;
					ShowSMLabel(col.gameObject.transform);
					Instantiate(bubbleParticleExplosion, col.transform.position, Quaternion.identity);
					Destroy(col.gameObject);
				}
				if(col.gameObject.tag == "BubbleBig")
				{
					FindObjectOfType<AudioManager>().PlayRandomPitch("pop", 0.8f, 0.9f);
					if(col.gameObject.GetComponent<BubbleBig>().hp <= 0)
					{
						CameraShaker.Instance.ShakeOnce(2f, 2f, 0.1f, 0.75f);
						GameObject.FindGameObjectWithTag("GM").GetComponent<ScoreManager>().score += 2 * scoreMultiplier;
						ShowSMLabel(col.gameObject.transform);
						Instantiate(bigBubbleParticleExplosion, col.transform.position, Quaternion.identity);
						Destroy(col.gameObject);
					}
					else
					{
						CameraShaker.Instance.ShakeOnce(1f, 2f, 0.1f, 0.75f);
						ShowSMLabel(col.gameObject.transform);
						col.gameObject.GetComponent<BubbleBig>().hp--;
						col.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 100, 200, 200); //TODO make this not hardcoded
						col.transform.localScale = new Vector3(col.transform.localScale.x * 0.7f, col.transform.localScale.y * 0.7f, 1f);
						Destroy(gameObject);
					}
				}
				if(col.gameObject.tag == "BubblePUSpear")
				{
					//destroy bubble, activate power up
					FindObjectOfType<AudioManager>().Play("spearPU");
					FindObjectOfType<AudioManager>().PlayRandomPUSound();
					GameObject.FindGameObjectWithTag("GM").GetComponent<ScoreManager>().score += scoreMultiplier;
					ShowSMLabel(col.gameObject.transform);
					CameraShaker.Instance.ShakeOnce(1f, 1.5f, 0.1f, 0.75f);
					GameObject.FindGameObjectWithTag("GM").GetComponent<SpearManager>().ActivateSpearPowerUp();
					Instantiate(SpearPUParticleExplosion, col.transform.position, Quaternion.identity);
					Destroy(col.gameObject);
				}
				if(col.gameObject.tag == "BubbleBomb")
				{
					FindObjectOfType<AudioManager>().Play("explosion");
					GameObject.FindGameObjectWithTag("GM").GetComponent<ScoreManager>().score += scoreMultiplier;
					ShowSMLabel(col.gameObject.transform);
					CameraShaker.Instance.ShakeOnce(5f, 3f, 0.1f, 1.5f);
					Instantiate(bombParticleExplosion, col.transform.position, Quaternion.identity);
					col.gameObject.GetComponent<CreateExplosion>().Explode();
					Destroy(gameObject);
				}
				if(col.gameObject.tag == "BubblePUTime")
				{
					FindObjectOfType<AudioManager>().Play("slowdown");
					FindObjectOfType<AudioManager>().PlayRandomTSSound();
					GameObject.FindGameObjectWithTag("GM").GetComponent<ScoreManager>().score += scoreMultiplier;
					ShowSMLabel(col.gameObject.transform);
					CameraShaker.Instance.ShakeOnce(1f, 1.5f, 0.1f, 0.75f);
					Instantiate(timePUParticleExplosion, col.transform.position, Quaternion.identity);
					GameObject.FindGameObjectWithTag("GM").GetComponent<PowerUpManager>().ActivateTimeSlow();
					Destroy(col.gameObject);
				}
				if(col.gameObject.tag == "BadBrick")
				{
					FindObjectOfType<AudioManager>().Play("emp");
					scoreMultiplier--;
					CameraShaker.Instance.ShakeOnce(1f, 2f, 0.1f, 0.75f);
					GameObject.FindGameObjectWithTag("GM").GetComponent<SpearManager>().DisableSpears();
					Destroy(gameObject);
				}
				if(scoreMultiplier > 1)
				{
					if(!FindObjectOfType<AudioManager>().IsNarratorPlaying())
					{
						int comboSoundIndex = Random.Range(1, 6);
						FindObjectOfType<AudioManager>().Play("combo" + comboSoundIndex.ToString());
					}
				}
			}
		}
	}

	private void OnBecameInvisible()
	{
		skewerable = false;
	}

	private void ShowSMLabel(Transform bubbleTransform)
	{
		if(scoreMultiplier > 1)
		{
			GameObject a = Instantiate(scoreMultiplierLabel, bubbleTransform.position, Quaternion.identity);
			a.transform.SetParent(canvas.transform, false);
			a.transform.localScale = new Vector3(2f, 2f, 2f);
			a.transform.position = bubbleTransform.position;
			a.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("x" + scoreMultiplier);
		}
	}

	private IEnumerator EmitParticles(ParticleSystem ps)
	{
		yield return new WaitForSeconds(0.1f);
		ps.Emit(40);
		yield return new WaitForSeconds(0.1f);
		ps.Emit(30);
		yield return new WaitForSeconds(0.1f);
		ps.Emit(20);
	}
}
