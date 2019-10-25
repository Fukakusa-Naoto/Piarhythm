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
	private RectTransform m_transform;
	private Image m_image;

	// ノーツ情報
	private Datas.NotesData m_notesData;


	// メンバ関数の定義 =====================================================
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

		// スケールの初期化
		m_transform.localScale = Vector3.one;

		// 色の初期化
		m_notesData.color = m_image.color = Color.green;
	}


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
		Debug.Log("選択された");
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツ情報の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    ノーツ情報
	//-----------------------------------------------------------------
	public Datas.NotesData GetNotesData()
	{
		return m_notesData;
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの音階を設定する
	//!
	//! @parameter [scale] 設定する音階
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetNotesScale(string scale)
	{
		m_notesData.scale = scale;
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの開始時間を設定する
	//!
	//! @parameter [startTime] 設定する開始時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetNotesStartTime(float startTime)
	{
		m_notesData.startTime = startTime;
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの終了時間を設定する
	//!
	//! @parameter [endTime] 設定する終了時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetNotesEndTime(float endTime)
	{
		m_notesData.endTime = endTime;
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの色を設定する
	//!
	//! @parameter [color] 設定する色
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetNotesColor(Color color)
	{
		m_notesData.color = color;
	}
}
