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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

			// 楽曲が終了した
			if (m_elapsedTime >= m_optionSheetController.GetWholeTime())
			{
				FinishedMusic();
			}
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

		// 再生位置を調節する
		m_audioSource.time = m_elapsedTime;

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
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SaveMusicPiece()
	{
		// 音楽ファイルのパスを取得する
		string audioFilePath = m_bgmSheetController.GetAudioFilePath();

		// BGMデータを取得する
		PiarhythmDatas.BGMData bgmData = m_bgmSheetController.GetBGMData();

		// BGMをコピーする
		PiarhythmUtility.CopyFile(audioFilePath, bgmData.path);

		// ノーツデータを取得する
		PiarhythmDatas.NotesData[] notesDatas = m_notesManager.GetNotesDatas();

		// 楽曲データを作成する
		PiarhythmDatas.MusicPieceData musicPieceData = new PiarhythmDatas.MusicPieceData();
		musicPieceData.bgmData = bgmData;
		musicPieceData.notesDataList = notesDatas;

		// json文字列に変換する
		string jsonString = JsonUtility.ToJson(musicPieceData);

		// 保存先のパスを作成する
		string path = PiarhythmDatas.MUSIC_PIECE_DIRECTORY_PATH + Path.GetFileNameWithoutExtension(audioFilePath) + ".json";

		// ファイルに書き込んで保存する
		PiarhythmUtility.WriteFileText(path, jsonString);
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



#if false
	// <メンバ変数>
	private AudioSource m_audioSource;
	private AudioClip m_audioClip = null;
	private string m_filePuth = null;
	public Text m_maxTime;
	public Text m_nowTime;
	public Dropdown m_dropdown;
	public Transform[] m_layers;
	public MusicalScoreController m_musicalScore;
	private SystemData m_systemData;
	public GameObject m_startInputField;
	public GameObject m_endInputField;
	public Text m_bgmText;
	private PiarhythmDatas.BGMData m_bgmData;
	public NotesManager m_notesManager;
	public NotesEditScrollbarController m_notesEditScrollbarController;

	// メンバ関数の定義 =====================================================
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Start()
	{
		m_bgmData = new PiarhythmDatas.BGMData();
		m_audioSource = gameObject.GetComponent<AudioSource>();

		string dataFilePath = UnityEngine.Application.dataPath + "/StreamingAssets/Data/System/SystemData.json";
		m_systemData = new SystemData();

		// ファイルの有無を調べる
		if (File.Exists(dataFilePath))
		{
			// ファイルを読み込む
			string json = File.ReadAllText(dataFilePath);
			m_systemData = JsonUtility.FromJson<SystemData>(json); ;
		}
		else
		{
			// 初期化
			m_systemData.speed = 1.0f;
			m_systemData.keyNumber = 88;
		}
	}



	//-----------------------------------------------------------------
	//! @summary   更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Update()
	{
		// 再生時間の更新
		if (m_audioClip)
		{
			// 再生終了
			if (m_audioSource.time >= m_bgmData.endTime)
			{
				m_audioSource.Stop();
				//m_musicalScore.Stop();
			}
			//m_nowTime.text = m_audioSource.time.ToString();
		}

		// レイヤーの変更
		m_layers[m_dropdown.value].SetAsLastSibling();

		// コルーチンのスタート
		if ((m_filePuth != null) && (m_audioClip == null)) StartCoroutine("Load", m_filePuth);

		if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene("Scenes/PlayScene");
	}



	//-----------------------------------------------------------------
	//! @summary   再生ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnPlayButton()
	{
		m_audioSource.time = m_bgmData.startTime;
		if (!m_audioSource.isPlaying) m_audioSource.Play();
		//m_musicalScore.Play();
	}



	//-----------------------------------------------------------------
	//! @summary   一時停止ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnPauseButton()
	{
		m_audioSource.Pause();
		//m_musicalScore.Pause();
	}



	//-----------------------------------------------------------------
	//! @summary   停止ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnStopButton()
	{
		if (m_audioSource.isPlaying) m_audioSource.Stop();
		//m_musicalScore.Stop();
	}



	//-----------------------------------------------------------------
	//! @summary   セーブボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnSaveButton()
	{
		// BGMファイルをコピーする
		if (!File.Exists(UnityEngine.Application.dataPath + "/StreamingAssets/BGM/" + m_audioClip.name))
			File.Copy(m_filePuth, UnityEngine.Application.dataPath + "/StreamingAssets/BGM/" + m_audioClip.name);

		// 楽曲データを構築する
		PiarhythmDatas.MusicPieceData musicPieceData = new PiarhythmDatas.MusicPieceData();
		musicPieceData.bgmData = m_bgmData;
		//musicPieceData.notesDataList = m_notesManager.GetNotesDatas();

		// json文字列に変換する
		string json = JsonUtility.ToJson(musicPieceData);
		// ファイルに書き出し
		string dataFilePath = UnityEngine.Application.dataPath + "/StreamingAssets/Data/MusicPiece/" + Path.GetFileNameWithoutExtension(m_audioClip.name) + ".json";
		File.WriteAllText(dataFilePath, json);
	}



	//-----------------------------------------------------------------
	//! @summary   ロードボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnLoadButton()
	{

	}



	//-----------------------------------------------------------------
	//! @summary   設定シーンを閉じる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnRevertButton()
	{
		// タイトルシーンに遷移する
		SceneManager.LoadScene((int)ScenenID.SCENE_TITLE);
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの移動速度の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    ノーツの移動速度
	//-----------------------------------------------------------------
	public float GetNotesSpeed()
	{
		return m_systemData.speed;
	}



	//-----------------------------------------------------------------
	//! @summary   開始時間の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnStartTimeInputField()
	{
		// 文字列を数値に変換してデータ構造体に保管する
		InputField inputField = m_startInputField.GetComponent<InputField>();
		m_bgmData.startTime = float.Parse(inputField.text);
	}



	//-----------------------------------------------------------------
	//! @summary   終了時間の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndTimeInputField()
	{
		// 文字列を数値に変換してデータ構造体に保管する
		InputField inputField = m_endInputField.GetComponent<InputField>();
		m_bgmData.endTime = float.Parse(inputField.text);
	}


	//-----------------------------------------------------------------
	//! @summary   音楽データの取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    読み込んだ音楽データ
	//-----------------------------------------------------------------
	public float[] GetAudioData()
	{
		if (!m_audioClip) return null;
		float[] samples = new float[m_audioClip.samples];
		m_audioSource.clip.GetData(samples, m_audioSource.timeSamples);

		return samples;
	}
#endif
}
