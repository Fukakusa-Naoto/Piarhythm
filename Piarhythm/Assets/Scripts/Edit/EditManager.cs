using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms; //OpenFileDialog用に使う
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class EditManager : MonoBehaviour
{
	private AudioSource m_audioSource;
	private AudioClip m_audioClip = null;
	private string m_filePuth = null;
	public Text m_maxTime;
	public Text m_nowTime;
	public Dropdown m_dropdown;
	public Transform[] m_layers;
	public MusicalScoreController m_musicalScore;


	// Use this for initialization
	void Start()
	{
		m_audioSource = gameObject.GetComponent<AudioSource>();
	}

	void Update()
	{
		// 再生時間の更新
		if (m_audioClip)
		{
			// 再生終了
			if (Mathf.Approximately(m_audioSource.time, m_audioClip.length)) m_musicalScore.Stop();
			m_nowTime.text = m_audioSource.time.ToString();
		}

		// レイヤーの変更
		m_layers[m_dropdown.value].SetAsLastSibling();

		// コルーチンのスタート
		if ((m_filePuth != null) && (m_audioClip == null)) StartCoroutine("Load", m_filePuth);

		if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene("Scenes/PlayScene");
	}

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


	IEnumerator Load(string file)
	{
		var www = UnityWebRequestMultimedia.GetAudioClip("file://" + file, AudioType.WAV);
		yield return www.SendWebRequest();
		m_audioClip = m_audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
		string[] str = file.Split('\\');
		m_audioClip.name = str[str.Length - 1];
		m_maxTime.text = "/\t";
		m_maxTime.text += m_audioClip.length.ToString();
	}


	public void OnPlayButton()
	{
		if (!m_audioSource.isPlaying) m_audioSource.Play();
		m_musicalScore.Play();
	}


	public void OnPauseButton()
	{
		m_audioSource.Pause();
		m_musicalScore.Pause();
	}



	public void OnStopButton()
	{
		if (m_audioSource.isPlaying) m_audioSource.Stop();
		m_musicalScore.Stop();
	}


	public void OnSaveButton()
	{
		File.Copy(m_filePuth, UnityEngine.Application.dataPath + "/Resources/BGM/" + m_audioClip.name);
	}


	public void OnLoadButton()
	{

	}
}
