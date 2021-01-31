using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum Emote
{
	// Nature
	FlowerP,
	FlowerR,
	FlowerW,
	FlowerY,
	YellowTree,
	Mushroom,
	Rock,

	// Props
	Barrel, 
	Campfire,
	Well,
	Fence,
	Pole,
	Signpost,
	Tent,
	Toilet,

	// Buildings
	Church,
	House,

	// Emotions
	EmoteConfused,
	EmoteAngry,
	EmoteIdea,
	EmoteLol
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
	public static MusicManager s_Instance;

	private AudioSource MusicSource;
	private AudioSource AmbientSource;
	private AudioSource IntroSource;
	private AudioSource MusicSourceHelper;

	public AudioMixerGroup mixerGroup;

	[SerializeField] private float		MusicVolumeDuringIntro = 0.6f;
	private float MusicVolumeDefault;

	[SerializeField] private Sprite		LoudSprite;
	[SerializeField] private Sprite		MutedSprite;

	[SerializeField] private GameObject	TestObject;

	[SerializeField] private Image MuteImage;
	[SerializeField] private Image TutorialImage;

	[SerializeField] List<EmoteDefinition> EmoteDefinitions;

	[SerializeField] private AudioClip IntroAmor;
	[SerializeField] private AudioClip IntroSearcher;

	///////////////////////////////////////////////////////////////////////////
	
	Dictionary<int, float> m_SuppressPlayerInputUntil	= new Dictionary<int, float>();
	Dictionary<int, EmoteRequest> m_PlayerQueueing		= new Dictionary<int, EmoteRequest>();

	///////////////////////////////////////////////////////////////////////////

	void StartIntro(bool amor)
	{
		IntroSource.clip = amor ? IntroAmor : IntroSearcher;
		IntroSource.Stop();
		IntroSource.Play();
		IntroSource.loop = false;

		float musicTime = GetMusicTime();

		float neededHeadroom = 1.0f;
		float preDelayTotal = 4.0f;

		// 
		// |-------------------------------|									(pre delay total)
		//						       |---|									(headroom)
		// X               X               O               X			   X    (next two seconds)
		//                  cur--v     
		// |o o o o|o o o o|o o o o|o o o o|o o o o|o o o o|o o o o|o o o o|o o o o|
		// ^-------^
		//    1s

		float twoSeconds = 4.0f;

		int currentTwoSeconds			= (int) (musicTime / twoSeconds);
		float currentTwoSecondTime		= currentTwoSeconds * twoSeconds;
		float currentToSecondOffset		= musicTime - currentTwoSecondTime;

		int earlieastTwoSecondsToStart	= (int) ((musicTime + neededHeadroom) / twoSeconds) + 1;
		float startOnBeatAbs				= earlieastTwoSecondsToStart * twoSeconds;
		float startTrack					= startOnBeatAbs - preDelayTotal;
		
		float introTrackOffset = musicTime - startTrack;

		float mainTrackOnBeatAfterSeconds		= 16.0f;
		float mainTrackOffsetForFullPreDelay	=  mainTrackOnBeatAfterSeconds - preDelayTotal;
		float mainTrackOffset					= mainTrackOffsetForFullPreDelay + introTrackOffset;

		IntroSource.time		= introTrackOffset;
		MusicSource.time		= musicTime;

		MusicSourceHelper.Play();
		MusicSourceHelper.loop = true;
		MusicSourceHelper.time	= mainTrackOffset;
		MusicSourceHelper.mute = true;
	}

	void UpdateIntroTracks()
	{
		if (IntroSource.isPlaying && MusicSourceHelper.mute && MusicSourceHelper.isPlaying)
		{
			float switchTrackAfter = 4.0f - 0.75f;
			switchTrackAfter += Time.deltaTime;

			if (IntroSource.time > switchTrackAfter)
			{
				MusicSourceHelper.mute = false;
				MusicSource.mute = true;
				MusicSource.Stop();

				AudioSource tmp = MusicSource;
				MusicSource = MusicSourceHelper;
				MusicSourceHelper = tmp;
			}
		}

		float musicSourceTargetVolume = IntroSource.isPlaying ? MusicVolumeDuringIntro : MusicVolumeDefault;

		
		MusicSource.volume			= Mathf.Lerp(MusicSource.volume, musicSourceTargetVolume, 0.025f);
		MusicSourceHelper.volume	= MusicSource.volume;
	}

	///////////////////////////////////////////////////////////////////////////

	void Stopintro()
	{
		IntroSource.Stop();
	}

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

	public float GetMusicTime()
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
			TryPlayEmote(new EmoteRequest(){ Emote = Emote.EmoteConfused,  OnOffbeat = false, PlayerID = 1 } );
		}

		if (Input.GetKey(KeyCode.Alpha4))
		{
			StartIntro(true);
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

		TutorialImage.color = IntroSource.isPlaying ? Color.yellow : Color.white;

		UpdateIntroTracks();
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
		audioSource.outputAudioMixerGroup	= MusicSource.outputAudioMixerGroup;

		audioSource.PlayDelayed(delay);

		Destroy(audioObject, audioClip.length + delay);

		return audioSource;
	}

	///////////////////////////////////////////////////////////////////////////

	public void Awake()
	{
		AudioSource[] sources = GetComponents<AudioSource>();
		MusicSource			= sources[0];
		AmbientSource		= sources[1];
		IntroSource			= sources[2];
		MusicSourceHelper	= sources[3];

		MusicSourceHelper.volume	= MusicSource.volume;
		MusicSourceHelper.clip		= MusicSource.clip;
		MusicSourceHelper.mute		= true;

		MusicVolumeDefault = MusicSource.volume;

		mixerGroup = MusicSource.outputAudioMixerGroup;

		IntroSource.loop = false;

		s_Instance = this;
	}

	private void OnEnable()
	{
		s_Instance = this;
	}

	///////////////////////////////////////////////////////////////////////////

	bool IsMuted()
	{
		float volume;
		mixerGroup.audioMixer.GetFloat("Volume", out volume);

		bool isMuted = volume < -40.0f;
		return isMuted;
	}

	///////////////////////////////////////////////////////////////////////////

	public void ToggleMuteMusic()
	{
		bool isMuted = IsMuted();
		isMuted = !isMuted;

		MuteImage.sprite = isMuted ? MutedSprite : LoudSprite;

		mixerGroup.audioMixer.SetFloat("Volume", isMuted ? -80.0f : -0.03f);
	}

	///////////////////////////////////////////////////////////////////////////

	public void ToggleTutorial()
	{
		bool isPlaying = IntroSource.isPlaying;

		if (isPlaying)
		{
			Stopintro();
		}
		else
		{
			bool isAmor = GameManager.instance ? GameManager.instance.localIsGhost : true;
			StartIntro(isAmor);

			if (IsMuted())
			{
				ToggleMuteMusic();
			}
		}
	}

	///////////////////////////////////////////////////////////////////////////
}
