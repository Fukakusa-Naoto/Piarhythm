using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Windows.Forms; //OpenFileDialog用に使う
using System.IO;
using UnityEngine.Networking;


public class test : MonoBehaviour
{
	private AudioSource audioSource;
	private AudioClip m_audioClip = null;
	private string m_filePuth = null;


	// Use this for initialization
	void Start()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
				Debug.Log("name" + m_audioClip.name);
				Debug.Log("channel ID" + m_audioClip.channels);
				float[] data = new float[m_audioClip.samples * m_audioClip.channels];
				m_audioClip.GetData(data, 0);
				Debug.Log("length Samples" + data.Length);
				Debug.Log("frequency" + m_audioClip.frequency);
			}
		}

		// コルーチンのスタート
		if ((m_filePuth != null) && (m_audioClip == null)) Debug.Log(StartCoroutine("Load", m_filePuth));
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
	}


	IEnumerator Load(string file)
	{
		var www = UnityWebRequestMultimedia.GetAudioClip("file://" + file, AudioType.WAV);
		yield return www.SendWebRequest();
		m_audioClip = GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(www);
		string[] str = file.Split('\\');
		m_audioClip.name = str[str.Length - 1];
	}
}
