//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		NotesSheetController.cs
//!
//! @summary	ノーツシートに関するC#スクリプト
//!
//! @date		2019.11.07
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
public class NotesSheetController : MonoBehaviour
{
	// <メンバ変数>
	private string[] m_keyList = null;

	// マネージャー
	[SerializeField]
	private NotesManager m_notesManager = null;

	// UI
	[SerializeField]
	private InputField m_musicalScaleInputField = null;
	[SerializeField]
	private InputField m_startBeatInputField = null;
	[SerializeField]
	private Dropdown m_noteLengthDropdown = null;
	[SerializeField]
	private Dropdown m_colorDropdown = null;
	[SerializeField]
	private RectTransform m_keyboard = null;


	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary	初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Start()
	{
		m_keyList = new string[m_keyboard.childCount];

		for (int i = 0; i < m_keyboard.childCount; ++i)
		{
			m_keyList[i] = m_keyboard.GetChild(i).name;
		}
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
		// コンポーネントの取得
		InputField inputField = m_musicalScaleInputField.GetComponent<InputField>();

		// 何も入力がされていなければ処理を終了する
		if (inputField.text == "") return;

		// 文字列を大文字、小文字の区別なくチェックする
		foreach (string n in m_keyList)
		{
			if (inputField.text.Equals(n, StringComparison.OrdinalIgnoreCase))
			{
				// 文字列を大文字にする
				string scale = inputField.text.ToUpper();
				// 選択されているノーツに設定する
				m_notesManager.SetSelectNotesScale(scale);

				// 処理を終了する
				return;
			}
		}
	}
	#endregion

	#region ノーツの開始時間の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの開始時間の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditNotesStartTimeInputField()
	{
		// 入力が無ければ、処理を終了する
		if (m_startBeatInputField.text == "") return;

		// 選択されているノーツに設定する
		m_notesManager.SetSelectNotesStartTime(float.Parse(m_startBeatInputField.text));
	}
	#endregion

	#region ノーツの長さの入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの長さの入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditNotesLengthTimeInputField()
	{
		// 選択されているノーツに設定する
		m_notesManager.SetSelectNotesLengthTime(m_noteLengthDropdown.value);
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
		// コンポーネントの取得
		Dropdown colorDropdown = m_colorDropdown.GetComponent<Dropdown>();

		// 入力値の応じて色を設定する
		switch (colorDropdown.value)
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

	#region 作成ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   作成ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickNotesCreateButton()
	{
		// ノーツの作成
		m_notesManager.CreateNotes();
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
	public void OnClickNotesDestroyButton()
	{
		// 選択されているノーツの削除
		m_notesManager.DestroyNotes();

	}
	#endregion

	#region UIへノーツ情報を反映させる
	//-----------------------------------------------------------------
	//! @summary   UIへノーツ情報を反映させる
	//!
	//! @parameter [displayNotes] 表示するノーツ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void DisplayNotes(EditNotesController displayNotes)
	{
		// UIへ情報を反映させる
		if (displayNotes != null)
		{
			// ノーツデータの取得
			PiarhythmDatas.NoteData notesData = displayNotes.GetNotesData();

			// 音階の更新
			m_musicalScaleInputField.text = notesData.m_scale;

			// 開始の拍数を更新
			m_startBeatInputField.text = notesData.m_startBeat.ToString();

			// 音符の長さを更新
			m_noteLengthDropdown.value = notesData.m_noteLength;

			// 色の更新
			if (notesData.m_color == Color.red) m_colorDropdown.value = 0;
			else if (notesData.m_color == Color.green) m_colorDropdown.value = 1;
			else if (notesData.m_color == Color.blue) m_colorDropdown.value = 2;
		}
	}
	#endregion

	#region 音階の変更があった時のUIへの反映
	//-----------------------------------------------------------------
	//! @summary   音階の変更があった時のUIへの反映
	//!
	//! @parameter [scale] 音階
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateMusicalScale(string scale)
	{
		// コンポーネントの取得
		InputField inputField = m_musicalScaleInputField.GetComponent<InputField>();

		// UIへ反映する
		inputField.text = scale;
	}
	#endregion
}
