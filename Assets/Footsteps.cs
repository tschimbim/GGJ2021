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
						bool  m_LastWasLeft		= false;

	Vector2 m_LastPosition;

	AudioSource m_AudioSource;

	private void Awake()
	{
		m_LastPosition = new Vector2(transform.position.x, transform.position.z);
		m_AudioSource = GetComponent<AudioSource>();

		m_LastFootstep = Time.time + 1.0f + Random.Range(0.0f, m_Frequency);
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

		float timeSinceLastFoostep = Time.time - m_LastFootstep;
		if (timeSinceLastFoostep < m_Frequency)
		{
			return;
		}

		if (Time.deltaTime <= 0)
		{
			return;
		}


		m_LastWasLeft = !m_LastWasLeft;

		bool isLeft = m_LastWasLeft;

		AudioClip[] clips = isLeft ? m_Left : m_Right;
		
		AudioClip clip = clips[Random.Range(0, clips.Length - 1)];

		m_AudioSource.Stop();
		m_AudioSource.clip = clip;
		m_AudioSource.Play();

		m_LastFootstep = Time.time;
	}
}

///////////////////////////////////////////////////////////////////////////