using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	// <メンバ変数>
	[SerializeField]
	private int m_speed = 5;
	private RectTransform m_transform;

    // Start is called before the first frame update
    void Start()
    {
		m_transform = GetComponent<RectTransform>();

		// ノーツの生成
		Create();
    }

    // Update is called once per frame
    void Update()
    {
		// 譜面を流す
		m_transform.localPosition += new Vector3(0, -m_speed, 0);
    }


	// ノーツの生成
	private void Create()
	{
		// To Do
	}
}
