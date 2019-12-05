//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		EditManager.cs
//!
//! @summary	エディットシーンの管理に関するC#スクリプト
//!
//! @date		2019.08.08
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


// クラスの定義 =============================================================
public class EditManager : MonoBehaviour
{
	// <メンバ変数>
	private bool m_playFlag = false;
	// 再生し始めてからの経過時間
	private float m_elapsedTime = 0.0f;

	// コンポーネント
	private AudioSource m_audioSource = null;

	// コントローラー
	[SerializeField]
	private MusicalScoreController m_musicalScoreController = null;
	[SerializeField]
	private MenuController m_menuController = null;
	[SerializeField]
	private OptionSheetController m_optionSheetController = null;
	[SerializeField]
	private BGMSheetController m_bgmSheetController = null;

	// マネージャー
	[SerializeField]
	private NotesManager m_notesManager = null;


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
		m_audioSource = GetComponent<AudioSource>();
	}
	#endregion

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
		// 再生中
		if(m_playFlag)
		{
			// 時間を更新
			m_elapsedTime += Time.deltaTime;

			// 譜面をスクロールさせる
			m_musicalScoreController.SetNowTime(m_elapsedTime);

			// UIへ反映させる
			m_menuController.UpdateDisplayNowTime(m_elapsedTime);

			// 全てのノーツの更新処理をする
			m_notesManager.UpdateAllEditNotes(m_elapsedTime);

			// BGMを止める
			PiarhythmDatas.BGMData bgmData = m_bgmSheetController.GetBGMData();
			if (m_audioSource.time >= bgmData.m_endTime) m_audioSource.Stop();

			// 楽曲が終了した
			if (m_elapsedTime >= m_optionSheetController.GetWholeTime()) FinishedMusic();
		}
	}
	#endregion

	#region 楽曲を再生させる
	//-----------------------------------------------------------------
	//! @summary   楽曲を再生させる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void Play()
	{
		// すでに再生中だった場合、処理を終了する
		if (m_playFlag) return;

		// フラグを立てる
		m_playFlag = true;

		// 現在時間の取得
		m_elapsedTime = m_musicalScoreController.GetNowTime();

		// BGMデータの取得
		PiarhythmDatas.BGMData bgmData = m_bgmSheetController.GetBGMData();

		// 再生位置を調節する
		m_audioSource.time = m_elapsedTime + bgmData.m_startTime;

		// BGMを再生させる
		if (m_audioSource.clip) m_audioSource.Play();

		// 再生前にノーツの初期化をする
		m_notesManager.PlayMomentEditNotes(m_elapsedTime);
	}
	#endregion

	#region 楽曲を停止させる
	//-----------------------------------------------------------------
	//! @summary   楽曲を停止させる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void Stop()
	{
		m_playFlag = false;

		// BGMを停止させる
		m_audioSource.Stop();

		// 全てのノーツを元に戻す
		m_notesManager.StopMomentEditNotes();

		// 初期位置に戻す
		m_musicalScoreController.SetNowTime(0.0f);
	}
	#endregion

	#region 楽曲を一時停止させる
	//-----------------------------------------------------------------
	//! @summary   楽曲を一時停止させる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void Pause()
	{
		// 再生フラグを倒す
		m_playFlag = false;

		// BGMを止める
		m_audioSource.Pause();

		// 全てのノーツを元に戻す
		m_notesManager.StopMomentEditNotes();
	}
	#endregion

	#region 楽曲が再生され切った時の処理
	//-----------------------------------------------------------------
	//! @summary   楽曲が再生され切った時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void FinishedMusic()
	{
		// 楽曲を停止させる
		Stop();
	}
	#endregion

	#region 楽曲データの保存処理
	//-----------------------------------------------------------------
	//! @summary   楽曲データの保存処理
	//!
	//! @parameter [filePath] 保存するファイルパス
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SaveMusicPiece(string filePath)
	{
		// 音楽ファイルのパスを取得する
		string audioFilePath = m_bgmSheetController.GetAudioFilePath();

		// BGMデータを取得する
		PiarhythmDatas.BGMData bgmData = m_bgmSheetController.GetBGMData();

		if (audioFilePath == null) audioFilePath = bgmData.m_path;

		// BGMをコピーする
		PiarhythmUtility.CopyFile(audioFilePath, bgmData.m_path);

		// ノーツデータを取得する
		PiarhythmDatas.NoteData[] notesDatas = m_notesManager.GetNotesDatas();

		// 設定データを取得する
		PiarhythmDatas.OptionData optionData = m_optionSheetController.GetOptionData();

		// 楽曲データを作成する
		PiarhythmDatas.MusicPieceData musicPieceData = ScriptableObject.CreateInstance<PiarhythmDatas.MusicPieceData>();
		musicPieceData.m_bgmData = bgmData;
		musicPieceData.m_noteDataList = notesDatas;
		musicPieceData.m_optionData = optionData;

		// json文字列に変換する
		string jsonString = JsonConvert.SerializeObject(musicPieceData);

		// 拡張子があるか調べる
		if (Path.GetExtension(filePath) != ".json") filePath += ".json";

		// ファイルに書き込んで保存する
		PiarhythmUtility.WriteFileText(filePath, jsonString);
	}
	#endregion

	#region 楽曲データの読み込み処理
	//-----------------------------------------------------------------
	//! @summary   楽曲データの読み込み処理
	//!
	//! @parameter [filePath] 読み込むファイルパス
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void LoadMusicPiece(string filePath)
	{
		// ファイルを読み込む
		string jsonString = null;
		PiarhythmUtility.ReadFileText(filePath, ref jsonString);

		// オブジェクトに変換する
		PiarhythmDatas.MusicPieceData musicPieceData = JsonConvert.DeserializeObject<PiarhythmDatas.MusicPieceData>(jsonString);

		// 設定データの設定と初期化
		m_optionSheetController.Start(musicPieceData.m_optionData);

		// BGMデータの設定
		if (musicPieceData.m_bgmData.m_path == "") m_bgmSheetController.SetBGMData(null);
		else m_bgmSheetController.SetBGMData(musicPieceData.m_bgmData);

		// 通常ノーツの生成
		foreach (PiarhythmDatas.NoteData noteData in musicPieceData.m_noteDataList)
		{
			// ノーツの生成
			if (noteData.m_nextNoteData == null) m_notesManager.CreateNotes(noteData);
			else m_notesManager.CreateConnectNote(noteData);
		}
	}
	#endregion

	#region AudioClipの設定処理
	//-----------------------------------------------------------------
	//! @summary   AudioClipの設定処理
	//!
	//! @parameter [audioClip] 設定するAudioClip
	//-----------------------------------------------------------------
	public void SetAudioClip(AudioClip audioClip)
	{
		m_audioSource.clip = audioClip;
	}
	#endregion

	#region BGMの音量を設定する
	//-----------------------------------------------------------------
	//! @summary   BGMの音量を設定する
	//!
	//! @parameter [volume] 設定する音量
	//-----------------------------------------------------------------
	public void SetAudioVolume(float volume)
	{
		m_audioSource.volume = volume;
	}
	#endregion
}
