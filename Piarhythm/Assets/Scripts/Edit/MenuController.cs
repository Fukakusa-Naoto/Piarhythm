﻿//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		MenuController.cs
//!
//! @summary	メニュー操作に関するC#スクリプト
//!
//! @date		2019.11.05
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// クラスの定義 =============================================================
public class MenuController : MonoBehaviour
{
	// <メンバ変数>
	// UI
	[SerializeField]
	private Text m_wholeText = null;
	[SerializeField]
	private InputField m_nowTimeInputField = null;
	[SerializeField]
	private Dropdown m_sheetDropdown = null;
	[SerializeField]
	private RectTransform[] m_sheets = null;

	// コントローラー
	[SerializeField]
	private MusicalScoreController m_musicalScoreController = null;
	[SerializeField]
	private ConnectNoteSheetController m_connectNoteSheetController = null;

	// マネージャー
	[SerializeField]
	private EditManager m_editManager = null;
	[SerializeField]
	private NotesManager m_noteManager = null;


	// メンバ関数の定義 =====================================================
	#region 楽曲全体の長さの変更があった時の表示の更新処理
	//-----------------------------------------------------------------
	//! @summary   楽曲全体の長さの変更があった時の表示の更新処理
	//!
	//! @parameter [wholeTime] 楽曲全体の長さ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateDisplayWholeTimeText(float wholeTime)
	{
		// 表示する文字列の作成
		string displayStr = "/\t";
		displayStr += wholeTime.ToString();

		// UIの更新
		m_wholeText.text = displayStr;
	}
	#endregion

	#region 戻るボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   戻るボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickRevertButton()
	{
		if(PiarhythmUtility.MessegeBoxYesOrNo("セーブをしますか？"))
		{
			// ダイアログを開いて、保存するファイルパスを決める
			string filePath = PiarhythmUtility.OpenExistFileDialog(
				PiarhythmDatas.MUSIC_PIECE_DIRECTORY_PATH,
				"JSONファイル(*.json)|*.json");

			// ファイルが選択されていなければ処理を終了する
			if (filePath == "") return;

			// 楽曲データの保存処理
			m_editManager.SaveMusicPiece(filePath);
		}

		// タイトルシーンに遷移する
		PiarhythmUtility.LoadScene(PiarhythmDatas.ScenenID.SCENE_TITLE);
	}
	#endregion

	#region 現在の時間の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   現在の時間の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditNowTimeInputField()
	{
		// 入力が無い場合、もしくはマイナスの値だった場合は現在の時間で設定する
		if ((m_nowTimeInputField.text == "") || (float.Parse(m_nowTimeInputField.text) < 0))
		{
			// 現在の時間を取得
			float time = m_musicalScoreController.GetNowTime();

			// 表示する
			m_nowTimeInputField.text
				= m_nowTimeInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text
				= time.ToString();

			// 処理を終了する
			return;
		}

		// スクロールバーを指定された時間の位置まで移動する
		m_musicalScoreController.SetNowTime(float.Parse(m_nowTimeInputField.text));
	}
	#endregion

	#region スクロールバーに変更があった時のUIへの反映処理
	//-----------------------------------------------------------------
	//! @summary   スクロールバーに変更があった時のUIへの反映処理
	//!
	//! @parameter [nowTime] 表示する時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateDisplayNowTime(float nowTime)
	{
		m_nowTimeInputField.text = nowTime.ToString();
	}
	#endregion

	#region 停止ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   停止ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickStopButton()
	{
		// 曲を停止させる
		m_editManager.Stop();
	}
	#endregion

	#region 再生ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   再生ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickPlayButton()
	{
		// 曲を再生させる
		m_editManager.Play();
	}
	#endregion

	#region 一時停止ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   一時停止ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickPauseButton()
	{
		// 曲を停止させる
		m_editManager.Pause();
	}
	#endregion

	#region シートの変更があった時の処理
	//-----------------------------------------------------------------
	//! @summary   シートの変更があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnValueChangedSheetDropdown()
	{
		// 選択されているシートをヒエラルキーの一番最後に移動させる
		switch (m_sheetDropdown.value)
		{
			case 0:
				m_sheets[m_sheetDropdown.value].SetAsLastSibling();

				// 複数選択がされている
				if (m_noteManager.GetMultipleSelectFlag()) m_connectNoteSheetController.SetAsLastSibling();
				break;
			case 1:
				m_sheets[m_sheetDropdown.value].SetAsLastSibling();
				break;
			case 2:
				m_sheets[m_sheetDropdown.value].SetAsLastSibling();
				break;
		}
	}
	#endregion

	#region 読み込みボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   読み込みボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickLoadButton()
	{
		// ダイアログを開いて、ファイルパスを取得する
		string filePath = PiarhythmUtility.OpenExistFileDialog(
			PiarhythmDatas.MUSIC_PIECE_DIRECTORY_PATH,
			"JSONファイル(*.json)|*.json");

		// ファイルが選択されていなければ処理を終了する
		if (filePath == "") return;

		// 楽曲データを読み込む
		m_editManager.LoadMusicPiece(filePath);
	}
	#endregion

	#region 保存ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   保存ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickSaveButton()
	{
		// ダイアログを開いて、保存するファイルパスを決める
		string filePath = PiarhythmUtility.OpenExistFileDialog(
			PiarhythmDatas.MUSIC_PIECE_DIRECTORY_PATH,
			"JSONファイル(*.json)|*.json");

		// ファイルが選択されていなければ処理を終了する
		if (filePath == "") return;

		// 楽曲データの保存処理
		m_editManager.SaveMusicPiece(filePath);
	}
	#endregion
}
