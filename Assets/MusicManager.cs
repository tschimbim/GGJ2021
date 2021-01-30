using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Emote
{
	YellowTree,
	Fence,

	What
}

[System.Serializable]
class EmoteDefinition
{
	public Emote		Emote;
	public AudioClip[]	Clips;
}

public struct EmoteRequest
{
	public Emote	Emote;
	public bool		OnOffbeat;
	public int		PlayerID;
}

public class MusicManager : MonoBehaviour
{
	private AudioSource MusicSource;
	private AudioSource AmbientSource;

	[SerializeField] private Sprite		LoudSprite;
	[SerializeField] private Sprite		MutedSprite;

	[SerializeField] private GameObject	TestObject;

	[SerializeField] private Image MuteImage;

	[SerializeField] List<EmoteDefinition> EmoteDefinitions;

	///////////////////////////////////////////////////////////////////////////
	
	Dictionary<int, float> m_SuppressPlayerInputUntil	= new Dictionary<int, float>();
	Dictionary<int, EmoteRequest> m_PlayerQueueing		= new Dictionary<int, EmoteRequest>();

	///////////////////////////////////////////////////////////////////////////

	public bool TryPlayEmote(EmoteRequest request)
	{
		AudioClip[] clips = null;

		foreach (EmoteDefinition def in EmoteDefinitions)
		{
			if (def.Emote != request.Emote)
			{
				continue;
			}

			clips = def.Clips;
			break;
		}

		if (clips == null)
		{ 
			Debug.Log("No defintiion for emote " + request.Emote.ToString() + " found");
			return true;
		}

		float suppressUntil = - 100.0f;
		if (m_SuppressPlayerInputUntil.TryGetValue(request.PlayerID, out suppressUntil))
		{
			if (Time.realtimeSinceStartup <= suppressUntil)
			{
				m_PlayerQueueing[request.PlayerID] = request;
				return false;
			}
		}

		m_SuppressPlayerInputUntil[request.PlayerID] = Time.realtimeSinceStartup + 0.99f;

		float preDelay = 1 / 4.0f;

		float timeSinceStart = GetMusicTime();

		float	halfSecondsSinceStart	= (timeSinceStart * 2.0f);
		int		curHalfSecond			= (int) (halfSecondsSinceStart);
		int		nextHalfSecond			= (int) Mathf.Ceil(halfSecondsSinceStart);
		int		earliestHitHalfSecond	= (int) Mathf.Ceil(halfSecondsSinceStart + preDelay);

		float playAt = earliestHitHalfSecond * 0.5f;

		bool EHHIsOnBeat	= (earliestHitHalfSecond % 2 == 0);
		bool tryToBeOnBeat	= !request.OnOffbeat;

		if (EHHIsOnBeat != tryToBeOnBeat)
		{
			playAt += 0.5f;
		}

		playAt -= preDelay;

		float delay = playAt - timeSinceStart;

		PlaySound(clips, delay);

		if (m_PlayerQueueing.ContainsKey(request.PlayerID))
		{
			m_PlayerQueueing.Remove(request.PlayerID);
		}
		return true;
	}

	///////////////////////////////////////////////////////////////////////////

	float GetMusicTime()
	{
		return MusicSource.time;
	}

	///////////////////////////////////////////////////////////////////////////

	float lastTime = -100;

	void DoTest()
	{

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			TryPlayEmote(new EmoteRequest(){ Emote = Emote.YellowTree,  OnOffbeat = false, PlayerID = 0 } );
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			TryPlayEmote(new EmoteRequest(){ Emote = Emote.Fence,  OnOffbeat = false, PlayerID = 0 } );
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			TryPlayEmote(new EmoteRequest(){ Emote = Emote.What,  OnOffbeat = false, PlayerID = 1 } );
		}

		if (!TestObject)
		{
			return;
		}

		float timeSinceStart = GetMusicTime();
		
		int lastHalfSeconds = (int) lastTime;
		int halfSecond		= (int) (timeSinceStart);

		if (lastHalfSeconds != halfSecond)
		{
			TestObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		}
		else
		{
			TestObject.transform.localScale = TestObject.transform.localScale + new Vector3(0.2f, 0.2f, 0.2f);
		}

		lastTime = timeSinceStart;
	}

	///////////////////////////////////////////////////////////////////////////

	private void Update()
	{
		DoTest();

		int[] keys = new int[m_PlayerQueueing.Count];
		m_PlayerQueueing.Keys.CopyTo(keys, 0);

		foreach (var key in keys)
		{
			TryPlayEmote(m_PlayerQueueing[key]);
		}
	}

	///////////////////////////////////////////////////////////////////////////

	public AudioSource PlaySound(AudioClip[] audioClips, float delay = 0.0f, float volume = 1.0f, float amount3D = 0.0f, Vector3? position3D = null)
	{
		if (audioClips == null || audioClips.Length == 0)
		{
			return null;
		}

		int rndIndex = Random.Range(0, audioClips.Length);
		return PlaySound(audioClips[rndIndex], delay, volume, amount3D, position3D);
	}

	///////////////////////////////////////////////////////////////////////////

	public AudioSource PlaySound(AudioClip audioClip, float delay = 0.0f, float volume = 1.0f, float amount3D = 0.0f, Vector3? position3D = null)
	{
		if (audioClip == null)
		{
			return null;
		}

		GameObject audioObject = new GameObject("OneShot " + audioClip.name);
		audioObject.transform.SetParent(transform);

		bool force2DSound = false;
		if (position3D.HasValue)
		{
			audioObject.transform.position = position3D.Value;
		}
		else
		{
			gameObject.transform.position = Vector3.zero;
			force2DSound = true;
		}

		float finalVolume = volume;

		AudioSource audioSource				= audioObject.AddComponent<AudioSource>();
		audioSource.clip					= audioClip;
		audioSource.volume					= finalVolume;
		audioSource.dopplerLevel			= 0.0f;
		audioSource.spatialBlend			= force2DSound ? 0.0f : amount3D;

		audioSource.PlayDelayed(delay);

		Destroy(audioObject, audioClip.length + delay);

		return audioSource;
	}

	///////////////////////////////////////////////////////////////////////////

	public void Awake()
	{
		AudioSource[] sources = GetComponents<AudioSource>();
		MusicSource		= sources[0];
		AmbientSource	= sources[1];
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
