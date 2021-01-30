using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
	private AudioSource MusicSource;
	private AudioSource AmbientSource;
	[SerializeField] private Sprite		LoudSprite;
	[SerializeField] private Sprite		MutedSprite;

	[SerializeField] private Image MuteImage;

	///////////////////////////////////////////////////////////////////////////

	public void Awake()
	{
		AudioSource[] sources = GetComponents<AudioSource>();
		MusicSource = sources[0];
		AmbientSource = sources[1];
	}

	///////////////////////////////////////////////////////////////////////////

	public void ToggleMuteMusic()
	{
		bool isMuted = !MusicSource.mute;
		MusicSource.mute = isMuted;
		AmbientSource.mute = isMuted;
		MuteImage.sprite = isMuted ? MutedSprite : LoudSprite;
	}

	///////////////////////////////////////////////////////////////////////////
}
