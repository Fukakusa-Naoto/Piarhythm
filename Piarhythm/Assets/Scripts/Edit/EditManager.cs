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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms; //OpenFileDialog用に使う
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


// クラスの定義 =============================================================
public class EditManager : MonoBehaviour
{
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
	private Datas.BGMData m_bgmData;
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
		m_bgmData = new Datas.BGMData();
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
			m_nowTime.text = m_audioSource.time.ToString();
		}

		// レイヤーの変更
		m_layers[m_dropdown.value].SetAsLastSibling();

		// コルーチンのスタート
		if ((m_filePuth != null) && (m_audioClip == null)) StartCoroutine("Load", m_filePuth);

		if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene("Scenes/PlayScene");
	}



	//-----------------------------------------------------------------
	//! @summary   ファイルの読み込み処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OpenExistFile()
	{
		OpenFileDialog open_file_dialog = new OpenFileDialog
		{
			//ファイルが実在しない場合は警告を出す(true)、警告を出さない(false)
			CheckFileExists = false
		};

		//ダイアログを開く
		open_file_dialog.ShowDialog();

		//取得したファイル名を代入する
		m_filePuth = open_file_dialog.FileName;

		if (m_filePuth != null) m_audioClip = m_audioSource.clip = null;
	}



	//-----------------------------------------------------------------
	//! @summary   読み込んだファイルをAudioCripに変換する
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	IEnumerator Load(string file)
	{
		var www = UnityWebRequestMultimedia.GetAudioClip("file://" + file, AudioType.WAV);
		yield return www.SendWebRequest();

		// 読み込み完了
		m_audioClip = m_audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
		string[] str = file.Split('\\');
		m_bgmText.text = m_audioClip.name = str[str.Length - 1];
		m_maxTime.text = "/\t";
		m_maxTime.text += m_audioClip.length.ToString();
		Text text = m_endInputField.transform.GetChild(1).GetComponent<Text>();
		text.text = m_audioClip.length.ToString();
		m_bgmData.startTime = 0.0f;
		m_bgmData.endTime = m_audioClip.length;

		// スクロールバーにグラフィックとして表示
		m_notesEditScrollbarController.UpdateTexture();
		m_musicalScore.ChangeScoreLength(m_bgmData.endTime - m_bgmData.startTime);
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
		Datas.MusicPieceData musicPieceData = new Datas.MusicPieceData();
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
}
