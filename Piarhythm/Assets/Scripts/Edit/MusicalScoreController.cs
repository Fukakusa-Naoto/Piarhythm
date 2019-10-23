using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalScoreController : MonoBehaviour
{
	private bool m_isPlaying;
	private Vector3 m_startPosition;
	private float m_time;
	public EditManager m_editManager;


    // Start is called before the first frame update
    void Start()
    {
		m_startPosition = GetComponent<RectTransform>().localPosition;
		m_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
		if(m_isPlaying)
		{
			GetComponent<RectTransform>().localPosition += new Vector3(0.0f, -m_editManager.GetNotesSpeed(), 0.0f);
		}
		m_time = GetComponent<RectTransform>().localPosition.y - m_startPosition.y;
	}

	public void Play()
	{
		m_isPlaying = true;
	}

	public void Pause()
	{
		m_isPlaying = false;
	}

	public void Stop()
	{
		m_isPlaying = false;
		GetComponent<RectTransform>().localPosition = m_startPosition;
	}

	public float GetTime()
	{
		return m_time;
	}


	public void SetMusicalScoreSize(float height)
	{
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height * 10.0f);
	}
}
