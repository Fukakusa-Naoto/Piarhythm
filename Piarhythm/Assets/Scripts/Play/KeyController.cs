using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
	private Image m_image;
	private Color m_startColor;
	private AudioSource m_audioSource;
	private bool m_isPress;

    // Start is called before the first frame update
    void Start()
    {
		m_image = GetComponent<Image>();
		m_startColor = m_image.color;
		m_audioSource = GetComponent<AudioSource>();
		m_isPress = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


	public void Press()
	{
		m_image.color = Color.red;
		if (!m_isPress) m_isPress = true;
		else return;
		if (m_audioSource.clip) m_audioSource.PlayOneShot(m_audioSource.clip);
	}

	public void Release()
	{
		m_isPress = false;
		m_image.color = m_startColor;
	}
}
