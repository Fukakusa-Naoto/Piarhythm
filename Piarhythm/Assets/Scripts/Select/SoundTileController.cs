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

	// コントロール
	private SelectManager m_selectManager = null;
	private MusicSheetController m_musicSheetController = null;


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

	#region クリックされた時の処理
	//-----------------------------------------------------------------
	//! @summary   クリックされた時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnPointerClickSoundTile()
	{
		// UIの表示を更新する
		m_musicSheetController.DisplaySelectMusicName(m_musicName);

		// 選択されている楽曲を更新する
		m_selectManager.SetSelectMusic(m_musicName);
	}
	#endregion

	#region SelectManagerを設定する
	//-----------------------------------------------------------------
	//! @summary   SelectManagerを設定する
	//!
	//! @parameter [selectManager] 設定するSelectManager
	//-----------------------------------------------------------------
	public void SetSelectManager(SelectManager selectManager)
	{
		m_selectManager = selectManager;
	}
	#endregion

	#region MusicSheetControllerを設定する
	//-----------------------------------------------------------------
	//! @summary   MusicSheetControllerを設定する
	//!
	//! @parameter [musicSheetController] 設定するMusicSheetController
	//-----------------------------------------------------------------
	public void SetMusicSheetController(MusicSheetController musicSheetController)
	{
		m_musicSheetController = musicSheetController;
	}
	#endregion
}
