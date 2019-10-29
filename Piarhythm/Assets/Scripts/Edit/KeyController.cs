//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		KeyController.cs
//!
//! @summary	MIDIキーボードのキー入力に関するC#スクリプト
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
public class KeyController : MonoBehaviour
{
	// <メンバ変数>
	// コンポーネント
	private Image m_image = null;
	private AudioSource m_audioSource = null;

	private Color m_startColor;
	private bool m_isPress;


	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Start()
    {
		// コンポーネントの取得
		m_image = GetComponent<Image>();
		m_audioSource = GetComponent<AudioSource>();

		m_startColor = m_image.color;
		m_isPress = false;
    }
	#endregion

	#region マウスでキーが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   マウスでキーが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnPointerClick()
    {
    }
	#endregion

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
