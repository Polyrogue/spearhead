using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUIManager : MonoBehaviour
{
	[SerializeField]
	private Sprite soundOn;

	[SerializeField]
	private Sprite soundOff;

	[SerializeField]
	private Sprite musicOn;

	[SerializeField]
	private Sprite musicOff;

	[SerializeField]
	private GameObject musicButton;

	[SerializeField]
	private GameObject soundButton;

	private void Start()
	{
		if(FindObjectOfType<AudioManager>().playSounds)
		{
			soundButton.GetComponent<Image>().sprite = soundOn;
		}
		else
		{
			soundButton.GetComponent<Image>().sprite = soundOff;
		}
		if(FindObjectOfType<AudioManager>().playBGM)
		{
			musicButton.GetComponent<Image>().sprite = musicOn;
		}
		else
		{
			musicButton.GetComponent<Image>().sprite = musicOff;
		}
	}

	public void ToggleSounds()
	{
		if(FindObjectOfType<AudioManager>().playSounds)
		{
			soundButton.GetComponent<Image>().sprite = soundOff;
			FindObjectOfType<AudioManager>().playSounds = false;
		}
		else
		{
			soundButton.GetComponent<Image>().sprite = soundOn;
			FindObjectOfType<AudioManager>().playSounds = true;
		}
	}

	public void ToggleBGM()
	{
		Sound s = FindObjectOfType<AudioManager>().GetBGM();
		if(FindObjectOfType<AudioManager>().playBGM)
		{
			FindObjectOfType<AudioManager>().playBGM = false;
			musicButton.GetComponent<Image>().sprite = musicOff;
			s.source.volume = 0;
		}
		else
		{
			FindObjectOfType<AudioManager>().playBGM = true;
			musicButton.GetComponent<Image>().sprite = musicOn;
			s.source.volume = 0.217f;
		}
	}
}
