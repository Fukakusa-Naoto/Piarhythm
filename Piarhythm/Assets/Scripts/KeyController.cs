using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
	private Image m_image;

    // Start is called before the first frame update
    void Start()
    {
		m_image = GetComponent<Image>();
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
		m_image.color = Color.white;
	}
}
