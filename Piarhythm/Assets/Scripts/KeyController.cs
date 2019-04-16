using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
	private Image m_image;
	private Color m_startColor;

    // Start is called before the first frame update
    void Start()
    {
		m_image = GetComponent<Image>();
		m_startColor = m_image.color;
    }

    // Update is called once per frame
    void Update()
    {

    }


	public void Press()
	{
		m_image.color = Color.red;
	}

	public void Release()
	{
		m_image.color = m_startColor;
	}
}
