using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms; //OpenFileDialog用に使う
using System.IO;
using UnityEngine.Networking;


public class PlayManager : MonoBehaviour
{
	private AudioSource m_audioSource;
	private AudioClip m_audioClip = null;

	// Start is called before the first frame update
	void Start()
	{
		m_audioSource = GetComponent<AudioSource>();

		// コルーチンのスタート
		if (m_audioClip == null) StartCoroutine("Load", UnityEngine.Application.dataPath + "/Resources/BGM/YUBIKIRI-GENMAN -special edit-.wav");
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.Space)) m_audioSource.Play();
	}

	IEnumerator Load(string file)
	{
		var www = UnityWebRequestMultimedia.GetAudioClip("file://" + file, AudioType.WAV);
		yield return www.SendWebRequest();
		m_audioClip = m_audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
		string[] str = file.Split('\\');
		m_audioClip.name = str[str.Length - 1];
	}
}
