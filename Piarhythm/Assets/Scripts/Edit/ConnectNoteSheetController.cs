﻿//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		ConnectNoteSheetController.cs
//!
//! @summary	連結ノーツの制御に関するC#スクリプト
//!
//! @date		2019.11.29
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class ConnectNoteSheetController : MonoBehaviour
{
	// <メンバ変数>
	private string[] m_keyList = null;

	// コンポーネント
	private RectTransform m_transform = null;

	// マネージャー
	[SerializeField]
	private NotesManager m_notesManager = null;

	// UI
	[SerializeField]
	private InputField m_musicalScaleInputField = null;
	[SerializeField]
	private InputField m_startBeatInputField = null;
	[SerializeField]
	private Dropdown m_colorDropdown = null;

	// 参照するデータ
	[SerializeField]
	private RectTransform m_keyboard = null;


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

		m_keyList = new string[m_keyboard.childCount];

		for (int i = 0; i < m_keyboard.childCount; ++i)
		{
			m_keyList[i] = m_keyboard.GetChild(i).name;
		}
	}
	#endregion

	#region シートを手前に持ってくる処理
	//-----------------------------------------------------------------
	//! @summary   シートを手前に持ってくる処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetAsLastSibling()
	{
		m_transform.SetAsLastSibling();
	}
	#endregion

	#region シートを奥に持っていく処理
	//-----------------------------------------------------------------
	//! @summary   シートを奥に持っていく処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetAsFirstSibling()
	{
		m_transform.SetAsFirstSibling();
	}
	#endregion

	#region 音階の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   音階の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditMusicalScaleInputField()
	{
		// 何も入力がされていなければ処理を終了する
		if (m_musicalScaleInputField.text == "") return;

		// 文字列を大文字、小文字の区別なくチェックする
		foreach (string n in m_keyList)
		{
			if (m_musicalScaleInputField.text.Equals(n, StringComparison.OrdinalIgnoreCase))
			{
				// 文字列を大文字にする
				string scale = m_musicalScaleInputField.text.ToUpper();
				// 選択されているノーツに設定する
				m_notesManager.SetSelectNotesScale(scale);

				// 処理を終了する
				return;
			}
		}
	}
	#endregion

	#region ノーツの開始の拍数の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの開始の拍数の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditStartBeatInputField()
	{
		// 入力が無ければ、処理を終了する
		if (m_startBeatInputField.text == "") return;

		// 選択されているノーツに設定する
		m_notesManager.SetSelectNotesStartTime(float.Parse(m_startBeatInputField.text));
	}
	#endregion

	#region 色の選択がされた時の処理
	//-----------------------------------------------------------------
	//! @summary   色の選択がされた時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnValueChangedColorDropdown()
	{
		// 入力値の応じて色を設定する
		switch (m_colorDropdown.value)
		{
			case 0:     // 赤
				m_notesManager.SetSelectNotesColor(Color.magenta);
				break;
			case 1:     // 緑
				m_notesManager.SetSelectNotesColor(Color.green);
				break;
			case 2:     // 青
				m_notesManager.SetSelectNotesColor(Color.cyan);
				break;
		}
	}
	#endregion

	#region 連結ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   連結ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickConnectButton()
	{
		// 連結ノーツの作成
		m_notesManager.CreateConnectNote();
	}
	#endregion

	#region 解除ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   解除ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickCuttingButton()
	{
		// ノーツの連結を解除
		m_notesManager.CuttingNote();
	}
	#endregion

	#region 削除ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   削除ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickDestroyButton()
	{
		// 選択されているノーツの削除
		m_notesManager.DestroyNotes();
	}
	#endregion
}
