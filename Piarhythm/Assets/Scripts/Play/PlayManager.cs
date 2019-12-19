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
		// コンポーネントの取得
		m_audioSource = GetComponent<AudioSource>();

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
		// AudioCripの読み込み
		LoadAudioCrip();
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
		if (m_loadFlag)
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
					// AudioSourceに設定する
					m_audioSource.clip = audioClip;
				}
			}
		}
	}
	#endregion
}
