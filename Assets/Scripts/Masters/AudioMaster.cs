using UnityEngine;
using System;

[Serializable]
public class Sound
{
	public string name;
	public AudioClip clip;

	public bool loop;

	[Range(0f, 1f)]
	public float volume = 1f;
	[Range(0.5f, 1.5f)]
	public float pitch = 1f;

	private AudioSource source;

	public void SetSource(AudioSource newSource)
	{
		source = newSource;
		source.clip = clip;
		source.loop = loop;
	}

	public void Play()
	{
		source.volume = volume;
		source.pitch = pitch;
		source.Play();
	}

	public void Stop()
	{
		source.Stop();
	}
}

public class AudioMaster : MonoBehaviour
{
	public static AudioMaster ins;

	[SerializeField]
	private Sound[] sounds;

	private void Awake()
	{
		if(!ins)
		{
			ins = this;
			DontDestroyOnLoad(ins);
		}
		else
		{
			Destroy(gameObject);
		}
		LoadSounds();
	}

	private void LoadSounds()
	{
		for(int i = 0; i < sounds.Length; i++)
		{
			GameObject newGameObject = new GameObject("Sound " + i + ":" + sounds[i].name);
			newGameObject.transform.SetParent(transform);

			AudioSource newAudioSource = newGameObject.AddComponent<AudioSource>();
			sounds[i].SetSource(newAudioSource);
		}
	}

	public void PlaySound(string name)
	{
		for(int i = 0; i < sounds.Length; i++)
		{
			if(sounds[i].name == name)
			{
				sounds[i].Play();
				return;
			}
		}

		Debug.LogWarning("PlaySound: Sound with name '" + name + "' has not been found!");
	}

	public void StopSound(string name)
	{
		for(int i = 0; i < sounds.Length; i++)
		{
			if(sounds[i].name == name)
			{
				sounds[i].Stop();
				return;
			}
		}

		Debug.LogWarning("StopSound: Sound with name '" + name + "' has not been found!");
	}
}
