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

		// コルーチンのスタート
		if (m_audioClip == null)
			StartCoroutine("Load",
				UnityEngine.Application.dataPath + "/Resources/BGM/YUBIKIRI-GENMAN -special edit-.wav");
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
		if (Input.GetKey(KeyCode.Space)) m_audioSource.Play();
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
		var www = UnityWebRequestMultimedia.GetAudioClip("file://" + file, AudioType.WAV);
		yield return www.SendWebRequest();
		m_audioClip = m_audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
		string[] str = file.Split('\\');
		m_audioClip.name = str[str.Length - 1];
	}
}
