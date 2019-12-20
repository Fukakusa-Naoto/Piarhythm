//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		PlayManager.cs
//!
//! @summary	プレイシーンの管理に関するC#スクリプト
//!
//! @date		2019.10.04
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;


// クラスの定義 =============================================================
public class PlayManager : MonoBehaviour
{
	// <メンバ変数>
	private PiarhythmDatas.SettingData m_settingData = null;
	private bool m_loadFlag = false;
	private IEnumerator<UnityWebRequestAsyncOperation> m_coroutine = null;
	private PiarhythmDatas.MusicPieceData m_musicPieceData = null;
	private float m_elapsedTime = 0.0f;
	private bool m_playedFlag = false;
	private float m_wholeTime = 0.0f;

	// コンポーネント
	private AudioSource m_audioSource = null;

	// コントローラー
	[SerializeField]
	private MusicController m_musicController = null;


	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Start()
	{
		// 設定データを読み込む
		string jsonString = null;
		PiarhythmUtility.ReadFileText(PiarhythmDatas.SETTING_DATA_FILE_PATH, ref jsonString);

		// インスタンスを作成する
		m_settingData = JsonConvert.DeserializeObject<PiarhythmDatas.SettingData>(jsonString);

		// プレイする楽曲データのファイルパスを取得する
		string filePath = PlayerPrefs.GetString(PiarhythmDatas.PLAY_MUSIC_PIECE_FILE_PATH, null);
		if (filePath == "") filePath = PiarhythmDatas.MUSIC_PIECE_DIRECTORY_PATH + "YUBIKIRI-GENMAN -special edit-.json";

		// 楽曲データを読み込む
		PiarhythmUtility.ReadFileText(filePath, ref jsonString);

		// インスタンスを作成する
		m_musicPieceData = JsonConvert.DeserializeObject<PiarhythmDatas.MusicPieceData>(jsonString);

		// 背景を作成する
		m_musicController.CreateMusicScoreBackGround(m_musicPieceData.m_optionData);

		// 楽曲全体の時間を取得する
		m_wholeTime = m_musicController.GetWholeTime();

		// ノーツを生成する
		m_musicController.CreateNoteList(m_musicPieceData.m_noteDataList);

		// BGMを読み込む
		if (m_musicPieceData.m_bgmData.m_path != null)
		{
			// 読み込み開始フラグをたてる
			m_loadFlag = true;

			// コルーチンを設定する
			m_coroutine = PiarhythmUtility.LoadAudioFile(m_musicPieceData.m_bgmData.m_path);
		}
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
		if (m_loadFlag)
		{
			// AudioCripの読み込み
			LoadAudioCrip();
		}
		else
		{
			if (m_playedFlag)
			{
				// 再生中の更新処理
				UpdatePlay();
			}
			else
			{
				// BGMを再生する
				if (m_audioSource != null)
				{
					// 再生位置を設定する
					m_audioSource.time = m_musicPieceData.m_bgmData.m_startTime;

					// 再生させる
					m_audioSource.Play();
				}

				m_playedFlag = true;
			}
		}
	}
	#endregion

	#region AudioCripの読み込み
	//-----------------------------------------------------------------
	//! @summary   更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void LoadAudioCrip()
	{
		// ファイルを読み込む
		StartCoroutine(m_coroutine);

		// 読み込みが完了した
		if (m_coroutine.Current.isDone)
		{
			// AudioClipへ変換する
			AudioClip audioClip = PiarhythmUtility.ConvertToAudioClip(m_coroutine.Current.webRequest);

			// 読み込みフラグを倒す
			m_loadFlag = false;

			if (audioClip)
			{
				// コンポーネントの取得
				m_audioSource = GetComponent<AudioSource>();

				// AudioSourceに設定する
				m_audioSource.clip = audioClip;
			}
		}
	}
	#endregion

	#region 再生中の更新処理
	//-----------------------------------------------------------------
	//! @summary   再生中の更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void UpdatePlay()
	{
		// 経過時間を更新する
		m_elapsedTime += Time.deltaTime;

		// 譜面の更新処理
		m_musicController.UpdatePlay(m_elapsedTime, m_settingData.m_noteSpeed);

		// BGMの更新処理
		if (m_musicPieceData.m_bgmData.m_endTime <= m_elapsedTime) m_audioSource.Stop();

		// 楽曲の終了処理
		if (m_elapsedTime >= m_wholeTime)
		{
			PiarhythmUtility.LoadScene(PiarhythmDatas.ScenenID.SCENE_SELECT);
		}
	}
	#endregion
}
