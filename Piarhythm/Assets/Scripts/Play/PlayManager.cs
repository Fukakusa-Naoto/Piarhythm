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
using System.Windows.Forms; //OpenFileDialog用に使う
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


// クラスの定義 =============================================================
public class PlayManager : MonoBehaviour
{
	// <メンバ変数>
	private AudioSource m_audioSource;
	private AudioClip m_audioClip = null;
	private Datas.MusicPieceData m_musicPieceData;
	private float m_notesEndTime;
	// 経過時間
	private float m_elapsedTime = 0.0f;
	private SystemData m_systemData;


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
		m_audioSource = GetComponent<AudioSource>();

		// セーブデータから読み込む楽曲ファイルパスを取得する
		string filePath = PlayerPrefs.GetString("Path", "");

		// 楽曲データを読み込む
		string jsonStr = "";
		StreamReader reader;
		reader = new StreamReader(filePath);
		jsonStr = reader.ReadToEnd();
		reader.Close();
		m_musicPieceData = JsonUtility.FromJson<Datas.MusicPieceData>(jsonStr);

		// BGMの読み込みコルーチンのスタート
		if (m_audioClip == null)
			StartCoroutine(LoadToAudioClip(m_musicPieceData.bgmData.path));


		// ノーツの終了時間の取得
		m_notesEndTime = 0.0f;
		foreach (Datas.NotesData n in m_musicPieceData.notesDataList)
		{
			if (m_notesEndTime < n.endTime)
			{
				m_notesEndTime = n.endTime;
			}
		}


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
		// 時間のカウント
		m_elapsedTime += Time.deltaTime;

		// BGMの再生
		if ((m_audioClip != null) && (!m_audioSource.isPlaying)) m_audioSource.Play();

		// BGMの停止
		if (m_audioSource.time >= m_musicPieceData.bgmData.endTime) m_audioSource.Stop();

		// 楽曲の終了
		if ((!m_audioSource.isPlaying) && (m_notesEndTime <= m_elapsedTime)) SceneManager.LoadScene(2);
	}



	//-----------------------------------------------------------------
	//! @summary   読み込み処理
	//!
	//! @parameter [path] ファイルパス
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public IEnumerator LoadToAudioClip(string path)
	{
		if (m_audioSource == null || string.IsNullOrEmpty(path))
			yield break;

		if (!File.Exists(path))
		{
			//ここにファイルが見つからない処理
			Debug.Log("File not found.");
			yield break;
		}

		using (WWW www = new WWW("file://" + path))
		{
			while (!www.isDone)
				yield return null;

			AudioClip audioClip = www.GetAudioClip(false, true);
			if (audioClip.loadState != AudioDataLoadState.Loaded)
			{
				//ここにロード失敗処理
				Debug.Log("Failed to load AudioClip.");
				yield break;
			}

			//ここにロード成功処理
			m_audioClip = m_audioSource.clip = audioClip;
			string[] str = path.Split('\\');
			m_audioClip.name = str[str.Length - 1];
			m_audioSource.time = m_musicPieceData.bgmData.startTime;
		}
	}


	//-----------------------------------------------------------------
	//! @summary   経過時間の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    楽曲が再生されてからの経過時間
	//-----------------------------------------------------------------
	public float GetElapsedTime()
	{
		return m_elapsedTime;
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    ノーツの配列
	//-----------------------------------------------------------------
	public Datas.NotesData[] GetNoteDatas()
	{
		return m_musicPieceData.notesDataList;
	}



	//-----------------------------------------------------------------
	//! @summary   再生速度の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    再生速度
	//-----------------------------------------------------------------
	public float GetPlaySpeed()
	{
		return m_systemData.speed;
	}
}
