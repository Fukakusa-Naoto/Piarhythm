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


// クラスの定義 =============================================================
public class PlayManager : MonoBehaviour
{
	// <メンバ変数>
	private AudioSource m_audioSource;
	private AudioClip m_audioClip = null;
	private MusicPieceData m_musicPieceData;


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
		m_musicPieceData = JsonUtility.FromJson<MusicPieceData>(jsonStr);

		// BGMの読み込みコルーチンのスタート
		if (m_audioClip == null)
			//StartCoroutine("Load", m_musicPieceData.bgmData.path);
			StartCoroutine(LoadToAudioClip(m_musicPieceData.bgmData.path));
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
		if ((m_audioClip != null) && (!m_audioSource.isPlaying)) m_audioSource.Play();
	}



	//-----------------------------------------------------------------
	//! @summary   読み込み処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	IEnumerator Load(string file)
	{
		//AudioType.MPEG
		using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + file, AudioType.WAV))
		{
			yield return www.SendWebRequest();

			if (www.isNetworkError)
			{
				Debug.Log(www.error);
			}
			else
			{
				m_audioClip = m_audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
				string[] str = file.Split('\\');
				m_audioClip.name = str[str.Length - 1];
				m_audioSource.time = m_musicPieceData.bgmData.startTime;
			}
		}
	}



	IEnumerator LoadToAudioClip(string path)
	{
		if (m_audioSource == null || string.IsNullOrEmpty(path))
			yield break;

		if (!File.Exists(path))
		{
			//ここにファイルが見つからない処理
			Debug.Log("File not found.");
			yield break;
		}

		using (WWW www = new WWW("file://" + path))  //※あくまでローカルファイルとする
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
}
