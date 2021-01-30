using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////

public class Footsteps : MonoBehaviour
{
 
	[SerializeField]	AudioClip[] m_Left;
	[SerializeField]	AudioClip[] m_Right;

	[SerializeField]	float m_Frequency		  = 0.5f;
	[SerializeField]	float m_VelocityThreshold = 0.1f;
						float m_LastFootstep	= 0.5f;
						float m_LastUpdate		= -100.0f;
						bool  m_LastWasLeft		= false;
	[SerializeField]	float preDelay = 0.1f;
	[SerializeField]	bool m_IsPlayer = false;

	Vector2 m_LastPosition;

	AudioSource m_AudioSource;

	private void Awake()
	{
		m_LastPosition = new Vector2(transform.position.x, transform.position.z);
		m_AudioSource = GetComponent<AudioSource>();

		m_LastFootstep = Time.realtimeSinceStartup + 1.0f + Random.Range(0.0f, m_Frequency);
	}

	private void Update()
	{
		Vector2 curPosition = new Vector2(transform.position.x, transform.position.z);
		float movementPerSecond = (curPosition - m_LastPosition).magnitude / Time.deltaTime;

		m_LastPosition = curPosition;

		if (movementPerSecond < m_VelocityThreshold)
		{
			return;
		}

		
		float random = (Mathf.Abs(GetInstanceID()) % 97.0f) / 97.0f;
		float botFactor = m_IsPlayer ? 1.0f : Mathf.Lerp(0.8f, 1.1f, random);

		int lastHalfSecond = (int) ((m_LastUpdate - preDelay) * botFactor * 2.0f);
		int curHalfSecond  = (int) ((MusicManager.s_Instance.GetMusicTime() - preDelay) * botFactor * 2.0f);

		m_LastUpdate = MusicManager.s_Instance.GetMusicTime();

		if (lastHalfSecond == curHalfSecond)
		{
			return;
		}

		/*
		float timeSinceLastFoostep = Time.realtimeSinceStartup - m_LastFootstep;
		if (timeSinceLastFoostep < m_Frequency)
		{
			return;
		}*/

		float musicTime = MusicManager.s_Instance.GetMusicTime();
		

		if (Time.deltaTime <= 0)
		{
			return;
		}


		m_LastWasLeft = !m_LastWasLeft;

		bool isLeft = lastHalfSecond % 2 == 1;

		AudioClip[] clips = isLeft ? m_Left : m_Right;
		
		AudioClip clip = clips[Random.Range(0, clips.Length - 1)];

		m_AudioSource.Stop();
		m_AudioSource.clip = clip;
		m_AudioSource.Play();

		m_LastFootstep = Time.realtimeSinceStartup;
	}
}

///////////////////////////////////////////////////////////////////////////