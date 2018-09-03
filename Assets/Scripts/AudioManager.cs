using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;
	public static AudioManager instance;

	public bool playSounds = true;

	public bool playBGM = true;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			//return;
		}
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
		foreach(Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}
	}

	public void Play(string name)
	{
		if(playSounds || (name == "bgm" && playBGM))
		{
			Sound s = Array.Find(sounds, sound => sound.name == name);
			if(s == null)
			{
				return;
			}
			s.source.PlayOneShot(s.source.clip);
		}
	}

	public void PlayRandomPUSound()
	{
		if(!IsNarratorPlaying())
		{
			string name = "pu";
			int index = UnityEngine.Random.Range(1, 4);
			name += index.ToString();
			Play(name);
		}
	}

	public void PlayRandomTSSound()
	{
		if(!IsNarratorPlaying() && playSounds)
		{
			string name = "ts";
			int index = UnityEngine.Random.Range(1, 4);
			name += index.ToString();
			Play(name);
		}
	}

	public void PlayRandomPitch(string name, float min, float max)
	{
		if(playSounds)
		{
			Sound s = Array.Find(sounds, sound => sound.name == name);
			if(s == null)
			{
				return;
			}
			s.source.pitch = UnityEngine.Random.Range(min, max);
			s.source.PlayOneShot(s.source.clip);
		}
		
	}
	
	public void Stop(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			return;
		}
		if(s.source.isPlaying)
		{
			s.source.Stop();
		}
	}

	public bool IsPlaying(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			return false;
		}
		if(s.source.isPlaying)
		{
			return true;
		}
		return false;
	}

	public bool IsNarratorPlaying()
	{
		bool playing = false;
		for(int i = 1; i < 6; i++)
		{
			Sound s = Array.Find(sounds, sound => sound.name == "combo" + i.ToString());
			if(s != null)
			{
				if(s.source.isPlaying)
				{
					playing = true;
				}
			}
		}
		for(int i = 1; i < 4; i++)
		{
			Sound s = Array.Find(sounds, sound => sound.name == "pu" + i.ToString());
			if(s != null)
			{
				if(s.source.isPlaying)
				{
					playing = true;
				}
			}
		}
		for(int i = 1; i < 4; i++)
		{
			Sound s = Array.Find(sounds, sound => sound.name == "ts" + i.ToString());
			if(s != null)
			{
				if(s.source.isPlaying)
				{
					playing = true;
				}
			}
		}
		return playing;
	}

	public void SetVolume(string name, float volume)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			return;
		}
		s.source.volume = volume;
	}

	public Sound GetBGM()
	{
		//sorry for how bad this is, future me
		Sound s = Array.Find(sounds, sound => sound.name == "bgm");
		if(s == null)
		{
			return null;
		}
		return s;
	}
}
