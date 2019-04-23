using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Windows.Forms; //OpenFileDialog用に使う
using System.IO;

public class test : MonoBehaviour
{
	public string input_field_path_;
	AudioSource audio;
	public AudioClipMaker m_ClipMaker;
	private WavFileInfo m_WavInfo = new WavFileInfo();
	public AudioClip m_audioClip;


	// Use this for initialization
	void Start()
	{
		audio = gameObject.AddComponent<AudioSource>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) if (!audio.isPlaying) audio.Play();
	}

	public void OpenExistFile()
	{

		OpenFileDialog open_file_dialog = new OpenFileDialog();

		//ファイルが実在しない場合は警告を出す(true)、警告を出さない(false)
		open_file_dialog.CheckFileExists = false;

		//ダイアログを開く
		open_file_dialog.ShowDialog();

		//取得したファイル名をInputFieldに代入する
		input_field_path_ = open_file_dialog.FileName;

		Debug.Log(input_field_path_);

		Copy(input_field_path_);

		Load(input_field_path_);

	}

	private static void Copy(string file)
	{
		string[] arr = file.Split('\\');
		File.Copy(file, UnityEngine.Application.dataPath + "\\Resources\\Audio\\" + arr[arr.Length - 1], true);
	}

	private void Load(string file)
	{
		string[] arr = file.Split('\\');
		string path = UnityEngine.Application.dataPath + "\\Resources\\Audio\\" + arr[arr.Length - 1];
		byte[] buf = File.ReadAllBytes(path.Replace('\\', '/'));

		// analyze wav file
		m_WavInfo.Analyze(buf);

		// create audio clip
		audio.clip = m_ClipMaker.Create(
			arr[arr.Length - 1],
			buf,
			m_WavInfo.TrueWavBufIdx,
			m_WavInfo.BitPerSample,
			m_WavInfo.TrueSamples,
			m_WavInfo.Channels,
			m_WavInfo.Frequency,
			false,
			true
		);

	}
}
