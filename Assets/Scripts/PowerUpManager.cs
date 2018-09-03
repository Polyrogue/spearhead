using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
	[SerializeField]
	private Image slowTimerUI;

	private float timeSlowFactor = 0.5f;

	private float timeSlowDuration = 1.3f;

	private bool timeSlowing = false;

	private Coroutine slowCoroutine = null;

	public void ActivateTimeSlow()
	{
		if(timeSlowing)
		{
			StopCoroutine(slowCoroutine);
		}
		slowCoroutine = StartCoroutine(SlowTime());
	}

	private void Update()
	{
		if(timeSlowing && slowTimerUI.fillAmount > 0)
		{
			slowTimerUI.fillAmount -= 1 / (timeSlowDuration * 120);
		}
	}

	private IEnumerator SlowTime()
	{
		Time.timeScale = timeSlowFactor;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		timeSlowing = true;
		slowTimerUI.gameObject.SetActive(true);
		slowTimerUI.fillAmount = 1f;
		yield return new WaitForSeconds(timeSlowDuration);
		slowTimerUI.gameObject.SetActive(false);
		timeSlowing = false;
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
}
