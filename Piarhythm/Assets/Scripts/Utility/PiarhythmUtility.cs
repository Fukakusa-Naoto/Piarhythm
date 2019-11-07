//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		PiarhythmUtility.cs
//!
//! @summary	ゲームで使用する共通関数に関するC#スクリプト
//!
//! @date		2019.11.06
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


// クラスの定義 ============================================================
public class PiarhythmUtility
{
	// 関数の定義 ==========================================================
	#region 座標から時間に変換する
	//-----------------------------------------------------------------
	//! @summary   座標から時間に変換する
	//!
	//! @parameter [positionY] 変換する座標
	//! @parameter [speed] 速度
	//!
	//! @return    変換された時間
	//-----------------------------------------------------------------
	public static float ConvertPositionToTime(float positionY, float speed)
	{
		return positionY / speed;
	}
	#endregion

	#region 時間から座標に変換する
	//-----------------------------------------------------------------
	//! @summary   時間から座標に変換する
	//!
	//! @parameter [time] 変換する時間
	//! @parameter [speed] 速度
	//!
	//! @return    変換された座標
	//-----------------------------------------------------------------
	public static float ConvertTimeToPosition(float time, float speed)
	{
		return time * speed;
	}
	#endregion

	#region ダイアログを開いてファイルをファイルを指定する
	//-----------------------------------------------------------------
	//! @summary   ダイアログを開いてファイルをファイルを指定する
	//!
	//! @parameter [void] なし
	//!
	//! @return    選択されたファイル名
	//-----------------------------------------------------------------
	public static string OpenExistFileDialog()
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			//ファイルが実在しない場合は警告を出す(true)、警告を出さない(false)
			CheckFileExists = false
		};

		//ダイアログを開く
		openFileDialog.ShowDialog();

		//取得したファイル名を代入する
		string filePuth = openFileDialog.FileName;

		return filePuth;
	}
	#endregion

	#region 音楽ファイルを読み込む
	//-----------------------------------------------------------------
	//! @summary   音楽ファイルを読み込む
	//!
	//! @parameter [fileName] 読み込むファイル名
	//!
	//! @return    読み込み結果
	//-----------------------------------------------------------------
	public static IEnumerator<UnityWebRequestAsyncOperation> LoadAudioFile(string fileName)
	{
		// 拡張子を調べる
		string extension = Path.GetExtension(fileName);

		// 拡張子に合わせて設定を変更する
		AudioType audioType = AudioType.UNKNOWN;
		switch(extension)
		{
			case ".wav":
				audioType = AudioType.WAV;
				break;
			case ".mp3":
				audioType = AudioType.MPEG;
				break;
			case ".aif":
				audioType = AudioType.AIFF;
				break;
		}

		// ファイルを読み込む
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + fileName, audioType);
		yield return www.SendWebRequest();
	}
	#endregion

	#region 音楽ファイルをAudioClipに変換する
	//-----------------------------------------------------------------
	//! @summary   音楽ファイルをAudioClipに変換する
	//!
	//! @parameter [webRequest] 読み込んだ音楽データ
	//!
	//! @return    変換されたAudioClip
	//-----------------------------------------------------------------
	public static AudioClip ConvertToAudioClip(UnityWebRequest webRequest)
	{
		// AudioClipへ変換
		return DownloadHandlerAudioClip.GetContent(webRequest);
	}
	#endregion
}
