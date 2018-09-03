using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpearManager : MonoBehaviour
{
	private bool drawingSpear;

	private Camera cam;

	[SerializeField]
	private GameObject spear;

	[SerializeField]
	private Image spearPUUI;

	[SerializeField]
	private GameObject spearPS;

	private GameObject s; //spear being drawn

	private SpriteRenderer spearRenderer;

	private SpriteRenderer shaftRenderer;

	private float prevDistance;

	private float shaftScale = 0.95f; //amplify the rate of scaling by some coefficient of the distance delta

	private float maxScale = 8.5f;

	private float minScale = 6f;

	[SerializeField]
	private float spearScale = 1f;

	private Transform shaft;

	[SerializeField]
	private float spearForce;

	//power up stuff
	private const float spearPUScale = 2f;

	private const float spearPUTimer = 4f;

	private bool spearPUOn = false;

	private Coroutine SPUCoroutine = null;

	//power down stuff
	private bool spearsDisabled = false;

	[SerializeField]
	private Image  bg;

	private void Start()
	{
		prevDistance = 0;
		cam = Camera.main;
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0) && !spearsDisabled)
		{
			if(Input.mousePosition.y < Screen.height * 0.4)
			{
				Vector3 pos = Input.mousePosition;
				pos.z = 50;
				pos = cam.ScreenToWorldPoint(pos);
				drawingSpear = true;
				s = Instantiate(spear, pos, Quaternion.identity);
				s.GetComponentInChildren<Transform>().localScale = new Vector3(s.GetComponentInChildren<Transform>().localScale.x, s.GetComponentInChildren<Transform>().localScale.y * spearScale, s.GetComponentInChildren<Transform>().localScale.z);
				spearRenderer = s.GetComponentInChildren<SpriteRenderer>();
				spearRenderer.color = new Color(0.5f, 0.5f, 0f);

				prevDistance = 0;
			}
		}
		if(Input.GetMouseButton(0) && drawingSpear && s)
		{
			Vector3 mousePosInWorld = Input.mousePosition;
			mousePosInWorld.z = 50;
			mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosInWorld);

			shaft = s.transform.GetChild(0).GetChild(0).transform;
			shaftRenderer = s.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
			if(shaft.localScale.y < minScale)
			{
				spearRenderer.color = new Color(0.4f, 0f, 0f);
				shaftRenderer.color = new Color(0.3f, 0f, 0f, 0.5f);
			}
			else
			{
				spearRenderer.color = new Color(0.5f, 0.5f, 0.5f);
				shaftRenderer.color = new Color(0.4f, 0.4f, 0.4f, 0.5f);
			}

			if(shaft.localScale.y > maxScale)
			{
				shaft.localScale = new Vector3(shaft.localScale.x, maxScale, shaft.localScale.z);
			}
			if(Vector3.Distance(s.transform.position, mousePosInWorld) > prevDistance && shaft.localScale.y < maxScale)
			{
				if(shaft.localScale.y > maxScale)
				{
					shaft.localScale = new Vector3(shaft.localScale.x, maxScale, shaft.localScale.z);
				}
				else
				{
					shaft.localScale += new Vector3(0f, shaftScale * (Vector3.Distance(s.transform.position, mousePosInWorld) - prevDistance), 0);
				}
			}
			if(Vector3.Distance(s.transform.position, mousePosInWorld) < prevDistance)
			{
				if(shaft.localScale.y > 0)
				{
					shaft.localScale -= new Vector3(0f, shaftScale * (prevDistance - Vector3.Distance(s.transform.position, mousePosInWorld)), 0);
				}
				if(shaft.localScale.y <= 0)
				{
					shaft.localScale = new Vector3(shaft.localScale.x, 0, shaft.localScale.z);
				}
			}
		}
		if(Input.GetMouseButtonUp(0) && drawingSpear && s && shaft)
		{
			drawingSpear = false;
			if(shaft.localScale.y < minScale)
			{
				Destroy(s);
			}
			else
			{
				s.GetComponentInChildren<SpearSkewer>().spearParticles = Instantiate(spearPS, s.transform.position, Quaternion.identity);
				s.GetComponentInChildren<SpearSkewer>().EnableFlight();
				s.GetComponent<Rigidbody2D>().AddForce(-s.transform.right * spearForce * (1 + (shaft.localScale.y) / 7));
				spearRenderer.color = new Color(1f, 1f, 1f);
				shaftRenderer = s.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
				shaftRenderer.color = new Color(1f, 1f, 1f);
			}
		}
		if(s && drawingSpear)
		{
			//look away from mouse
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 50;
			Vector3 objectPos = cam.WorldToScreenPoint(s.transform.position);
			mousePos.x = mousePos.x - objectPos.x;
			mousePos.y = mousePos.y - objectPos.y;

			float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			s.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		}

		//update previous positions and distances
		if(s)
		{
			Vector3 mousePosInWorld = Input.mousePosition;
			mousePosInWorld.z = 50;
			mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosInWorld);
			prevDistance = Vector3.Distance(s.transform.position, mousePosInWorld);
		}

		if(spearPUOn && spearPUUI.fillAmount > 0)
		{
			spearPUUI.fillAmount -= 1 / (spearPUTimer * 60);
		}
	}

	public void ActivateSpearPowerUp()
	{
		if(spearPUOn)
		{
			StopCoroutine(SPUCoroutine);
		}
		SPUCoroutine = StartCoroutine(SpearPUCoroutine());
	}

	public void DisableSpears()
	{
		StartCoroutine(DisableSpearsCoroutine());
	}

	private IEnumerator DisableSpearsCoroutine()
	{
		spearsDisabled = true;
		bg.color = new Color32(255, 79, 79, 122);
		yield return new WaitForSeconds(1.5f);
		bg.color = new Color32(186, 186, 186, 122);
		spearsDisabled = false;
	}

	private IEnumerator SpearPUCoroutine()
	{
		spearPUOn = true;
		spearScale = spearPUScale;
		spearPUUI.gameObject.SetActive(true);
		spearPUUI.fillAmount = 1f;
		yield return new WaitForSeconds(spearPUTimer);
		spearPUUI.gameObject.SetActive(false);
		spearScale = 1f;
		spearPUOn = false;
	}
}
