//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		EditNotesController.cs
//!
//! @summary	エディター用ノーツに関するC#スクリプト
//!
//! @date		2019.10.25
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class EditNotesController : MonoBehaviour
{
	// <メンバ変数>
	// コンポーネント
	private RectTransform m_transform;
	private Image m_image;

	// マネージャー
	private NotesManager m_notesManager;

	// ノーツ情報
	private Datas.NotesData m_notesData;


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
		m_image = GetComponent<Image>();

		// データの初期化
		m_notesData = new Datas.NotesData();

		// 音階の設定
		m_notesData.scale = "C4";

		// 座標の初期化


		// スケールの初期化
		m_transform.localScale = Vector3.one;

		// 色の初期化
		m_notesData.color = m_image.color = Color.green;
	}
	#endregion

	#region ドラッグ時の移動処理
	//-----------------------------------------------------------------
	//! @summary   ドラッグ時の移動処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnMouseDrag()
	{
		// ノーツの移動
		Debug.Log("Drag");
	}
	#endregion

	#region ノーツが選択された時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツが選択された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnPointerDown()
	{
		// 選択されたことをNotesManagerに伝える
		m_notesManager.SetSelectNotes(gameObject);
	}
	#endregion

	#region ノーツ情報の取得
	//-----------------------------------------------------------------
	//! @summary   ノーツ情報の取得
	//!
	//! @return    ノーツ情報
	//-----------------------------------------------------------------
	public Datas.NotesData GetNotesData()
	{
		return m_notesData;
	}
	#endregion

	#region ノーツの音階を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの音階を設定する
	//!
	//! @parameter [scale] 設定する音階
	//-----------------------------------------------------------------
	public void SetNotesScale(string scale)
	{
		m_notesData.scale = scale;

		// 座標を設定された音階の位置に移動させる

	}
	#endregion

	#region ノーツの開始時間を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの開始時間を設定する
	//!
	//! @parameter [startTime] 設定する開始時間
	//-----------------------------------------------------------------
	public void SetNotesStartTime(float startTime)
	{
		m_notesData.startTime = startTime;
	}
	#endregion

	#region ノーツの終了時間を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの終了時間を設定する
	//!
	//! @parameter [endTime] 設定する終了時間
	//-----------------------------------------------------------------
	public void SetNotesEndTime(float endTime)
	{
		m_notesData.endTime = endTime;
	}
	#endregion

	#region ノーツの色を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの色を設定する
	//!
	//! @parameter [color] 設定する色
	//-----------------------------------------------------------------
	public void SetNotesColor(Color color)
	{
		m_notesData.color = color;
	}
	#endregion

	#region NotesManagerを設定する
	//-----------------------------------------------------------------
	//! @summary   NotesManagerを設定する
	//!
	//! @parameter [notesManager] 設定するNotesManager
	//-----------------------------------------------------------------
	public void SetNotesManager(NotesManager notesManager)
	{
		m_notesManager = notesManager;
	}
	#endregion
}
