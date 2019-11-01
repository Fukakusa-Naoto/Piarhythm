//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		MusicalScoreController.cs
//!
//! @summary	楽譜に関するC#スクリプト
//!
//! @date		2019.10.29
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class MusicalScoreController : MonoBehaviour
{
	// <メンバ定数>
	private static readonly float MIN_HEIGHT = 368.9f;

	// <メンバ変数>
	// コンポーネント
	private RectTransform m_transform = null;


	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Start()
	{
		// コンポーネントの取得
		m_transform = GetComponent<RectTransform>();
	}
	#endregion

	#region 楽曲の長さが変更された時の処理
	//-----------------------------------------------------------------
	//! @summary   楽曲の長さが変更された時の処理
	//!
	//! @parameter [length] 楽曲の長さ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void ChangeScoreLength(float length)
	{
		// 時間を座標に変換
		float height = (length * NotesManager.NOTES_SPEED);

		if (height < MIN_HEIGHT) height = MIN_HEIGHT;

		m_transform.sizeDelta = new Vector2(m_transform.sizeDelta.x, height);
	}
	#endregion


#if false
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
#endif
}
