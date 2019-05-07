using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms; //OpenFileDialog用に使う
using UnityEngine.Networking;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
	private AudioSource m_audioSource;
	private AudioClip m_audioClip = null;
	private string m_filePuth = null;
	public Text m_maxTime;
	public Text m_nowTime;


	// Use this for initialization
	void Start()
	{
		m_audioSource = gameObject.GetComponent<AudioSource>();
	}

	void Update()
	{
		// 再生時間の更新
		if(m_audioClip) m_nowTime.text = m_audioSource.time.ToString();

		// コルーチンのスタート
		if ((m_filePuth != null) && (m_audioClip == null)) StartCoroutine("Load", m_filePuth);
	}

	public void OpenExistFile()
	{

		OpenFileDialog open_file_dialog = new OpenFileDialog();

		//ファイルが実在しない場合は警告を出す(true)、警告を出さない(false)
		open_file_dialog.CheckFileExists = false;

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
	}


	public void OnPauseButton()
	{
		m_audioSource.Pause();
	}



	public void OnStopButton()
	{
		if (m_audioSource.isPlaying) m_audioSource.Stop();
	}


	public void OnSaveButton()
	{

	}


	public void OnLoadButton()
	{

	}
}
