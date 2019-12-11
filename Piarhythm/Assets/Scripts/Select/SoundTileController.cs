//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		SoundTileController.cs
//!
//! @summary	楽曲タイルの制御に関するC#スクリプト
//!
//! @date		2019.12.10
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class SoundTileController : MonoBehaviour
{
	// <メンバ変数>
	// 曲名
	private string m_musicName = null;

	// コンポーネント
	private RectTransform m_transform = null;
	private Text m_text = null;


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
		m_text = transform.GetChild(0).GetComponent<Text>();
		m_transform = GetComponent<RectTransform>();

		// テキストを更新
		m_text.text = m_musicName;

		// 親子関係を組んだ時のずれを修正する
		m_transform.localScale = Vector3.one;
		Vector3 position = m_transform.localPosition;
		position.z = 0.0f;
		m_transform.localPosition = position;
    }
	#endregion

	#region 曲名の設定する
	//-----------------------------------------------------------------
	//! @summary   曲名の設定する
	//!
	//! @parameter [musicName] 設定する曲名
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetMusicName(string musicName)
	{
		m_musicName = musicName;
	}
	#endregion
}
