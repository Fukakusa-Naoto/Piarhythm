//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		BGMSheetController.cs
//!
//! @summary	BGMシートに関するC#スクリプト
//!
//! @date		2019.11.07
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


// クラスの定義 =============================================================
public class BGMSheetController : MonoBehaviour
{
	// <メンバ変数>
	private string m_filePath = null;
	private bool m_loadFlag = false;
	private AudioClip m_audioClip = null;
	private IEnumerator<UnityWebRequestAsyncOperation> m_coroutine = null;

	// UI
	[SerializeField]
	private InputField m_nameInputField = null;
	[SerializeField]
	private InputField m_startTimeInputField = null;
	[SerializeField]
	private InputField m_endTimeInputField = null;

	// コントローラー
	[SerializeField]
	private MusicalScoreController m_musicalScoreController = null;
	[SerializeField]
	private MenuController m_menuController = null;
	[SerializeField]
	private NotesEditScrollbarController m_notesEditScrollbarController = null;
	[SerializeField]
	private OptionSheetController m_optionSheetController = null;

	// マネージャー
	[SerializeField]
	private EditManager m_editManager = null;

	// BGMデータ
	private PiarhythmDatas.BGMData m_BGMData = null;


	// メンバ関数の定義 =====================================================
	#region 更新処理
	//-----------------------------------------------------------------
	//! @summary   更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Update()
	{
		if(m_loadFlag)
		{
			// ファイルを読み込む
			StartCoroutine(m_coroutine);

			// 読み込みが完了した
			if (m_coroutine.Current.isDone)
			{
				// AudioClipへ変換する
				m_audioClip = PiarhythmUtility.ConvertToAudioClip(m_coroutine.Current.webRequest);

				// 読み込みフラグを倒す
				m_loadFlag = false;

				if(m_audioClip)
				{
					// BGMデータを設定する
					if (m_BGMData == null) SetBGMData();

					// オーディオクリップを設定し、スクロールバーのテクスチャを更新する
					m_notesEditScrollbarController.SetAudioClip(m_audioClip);

					// UIへ反映させる
					DisplayBGMData();
					m_musicalScoreController.ChangeScoreLength(m_BGMData.m_endTime);
					m_menuController.UpdateDisplayWholeTimeText(m_BGMData.m_endTime);
					m_optionSheetController.SetWholeMeasure(m_BGMData.m_endTime);

					// AudioSourceに設定する
					m_editManager.SetAudioClip(m_audioClip);
				}
			}
		}
	}
	#endregion

	#region 曲選択ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   曲選択ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickSelectButton()
	{
		// ダイアログを開いて、ファイルパスを取得する
		m_filePath = PiarhythmUtility.OpenExistFileDialog(
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
			"WAVファイル(*.wav) | *.wav");

		// ファイルが選択されていなければ処理を終了する
		if (m_filePath == "") return;

		// 読み込み開始フラグをたてる
		m_loadFlag = true;

		// コルーチンを設定する
		m_coroutine = PiarhythmUtility.LoadAudioFile(m_filePath);
	}
	#endregion

	#region 曲の開始時間が入力された時の処理
	//-----------------------------------------------------------------
	//! @summary   曲の開始時間が入力された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditStartTimeInputField()
	{
		// 入力が無ければ、初期化する
		if (m_startTimeInputField.text == "") m_startTimeInputField.text = "0.0";

		// 終了時間超えていれば処理を終了する
		float startTime = float.Parse(m_startTimeInputField.text);
		if (m_BGMData.m_endTime <= startTime) return;

		// データを更新する
		PiarhythmDatas.BGMData bgmData = m_BGMData;
		bgmData.m_startTime = startTime;
		m_BGMData = bgmData;

		// スクロールバーのテクスチャの更新
		m_notesEditScrollbarController.UpdateTexture(m_BGMData, m_optionSheetController.GetWholeTime());
	}
	#endregion

	#region 曲の終了時間が入力された時の処理
	//-----------------------------------------------------------------
	//! @summary   曲の終了時間が入力された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditEndTimeInputField()
	{
		// 入力が無ければ、初期化する
		if (m_endTimeInputField.text == "") m_endTimeInputField.text = "0.0";

		// 開始時間超えていれば処理を終了する
		float endTime = float.Parse(m_endTimeInputField.text);
		if (m_BGMData.m_startTime >= endTime) return;

		// データを更新する
		PiarhythmDatas.BGMData bgmData = m_BGMData;
		bgmData.m_endTime = endTime;
		m_BGMData = bgmData;

		// スクロールバーのテクスチャの更新
		m_notesEditScrollbarController.UpdateTexture(m_BGMData, m_optionSheetController.GetWholeTime());
	}
	#endregion

	#region 曲を外すボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   曲を外すボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnOnClickRemoveButton()
	{
		// パスを空にする
		m_filePath = null;

		// オーディオクリップを削除する
		if(m_audioClip)
		{
			Destroy(m_audioClip);
			m_audioClip = null;
		}

		// オーディオソースの設定を外す
		m_editManager.SetAudioClip(m_audioClip);

		// BGMデータを初期化する
		m_BGMData = null;

		// オーディオクリップを設定し、スクロールバーのテクスチャを更新する
		m_notesEditScrollbarController.SetAudioClip(m_audioClip);
		m_notesEditScrollbarController.UpdateTexture(m_BGMData, m_BGMData.m_endTime);

		// UIを初期化する
		m_nameInputField.text = "None";
		m_startTimeInputField.text = "0.0";
		m_endTimeInputField.text = "0.0";
	}
	#endregion

	#region BGMデータをUIへ反映させる
	//-----------------------------------------------------------------
	//! @summary   BGMデータをUIへ反映させる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void DisplayBGMData()
	{
		// 曲名を取得する
		string musicName = Path.GetFileNameWithoutExtension(m_BGMData.m_path);

		// UIへ反映する
		m_nameInputField.text = musicName;
		m_startTimeInputField.text = m_BGMData.m_startTime.ToString();
		m_endTimeInputField.text = m_BGMData.m_endTime.ToString();
	}
	#endregion

	#region BGMデータを設定する
	//-----------------------------------------------------------------
	//! @summary   BGMデータを設定する
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void SetBGMData()
	{
		m_BGMData = ScriptableObject.CreateInstance<PiarhythmDatas.BGMData>();

		// ファイル名を切り取る
		string fileName = Path.GetFileName(m_filePath);

		m_BGMData.m_path = PiarhythmDatas.BGM_DIRECTORY_PATH + fileName;
		m_BGMData.m_startTime = 0.0f;
		m_BGMData.m_endTime = m_audioClip.length;
	}
	#endregion

	#region BGMデータを設定する(設定引数あり)
	//-----------------------------------------------------------------
	//! @summary   BGMデータを設定する(設定引数あり)
	//!
	//! @parameter [BGMData] 設定するBGMData
	//-----------------------------------------------------------------
	public void SetBGMData(PiarhythmDatas.BGMData BGMData)
	{
		// 設定するデータがない場合、処理を終了する
		if (BGMData == null) return;

		// 設定する
		m_BGMData = BGMData;

		// 読み込み開始フラグをたてる
		m_loadFlag = true;

		// コルーチンを設定する
		m_coroutine = PiarhythmUtility.LoadAudioFile(m_BGMData.m_path);
	}
	#endregion

	#region BGMデータを取得する
	//-----------------------------------------------------------------
	//! @summary   BGMデータを取得する
	//!
	//! @return    BGMデータ
	//-----------------------------------------------------------------
	public PiarhythmDatas.BGMData GetBGMData()
	{
		if (m_BGMData == null) return ScriptableObject.CreateInstance<PiarhythmDatas.BGMData>();
		else return m_BGMData;
	}
	#endregion

	#region 音楽ファイルのパスを取得する
	//-----------------------------------------------------------------
	//! @summary   音楽ファイルのパスを取得する
	//!
	//! @return    音楽ファイルのパス
	//-----------------------------------------------------------------
	public string GetAudioFilePath()
	{
		return m_filePath;
	}
	#endregion
}
