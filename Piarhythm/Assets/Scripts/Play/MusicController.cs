//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		MusicController.cs
//!
//! @summary	ノーツスクロールに関するC#スクリプト
//!
//! @date		2019.10.04
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// クラスの定義 =============================================================
public class MusicController : MonoBehaviour
{
	// <メンバ変数>
	[SerializeField]
	private int m_speed = 5;
	private RectTransform m_transform;
	// 経過時間
	private float m_elapsedTime;


	// メンバ関数の定義 =====================================================
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Start()
    {
		m_transform = GetComponent<RectTransform>();

		m_elapsedTime = 0.0f;

		// ノーツの生成
		Create();
    }



	//-----------------------------------------------------------------
	//! @summary   更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Update()
    {
		// 時間のカウント
		m_elapsedTime += Time.deltaTime;

		// 譜面を流す
		float pos = (m_speed * -100.0f) * m_elapsedTime;
		m_transform.localPosition = new Vector3(0, pos, 0);
    }



	//-----------------------------------------------------------------
	//! @summary   ノーツの作成
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Create()
	{
		// To Do
	}
}
