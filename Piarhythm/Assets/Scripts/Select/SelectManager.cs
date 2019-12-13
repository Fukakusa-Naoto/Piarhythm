//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		SelectManager.cs
//!
//! @summary	楽曲選択シーンの管理に関するC#スクリプト
//!
//! @date		2019.08.29
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;


// クラスの定義 =============================================================
public class SelectManager : MonoBehaviour
{
	// <メンバ変数>
	// 選択されている楽曲名
	private string m_selectMusic = null;
	private Dictionary<string, string> m_musicPathDictionary = null;

	// UI
	[SerializeField]
	private SettingSheetController m_settingSheetController = null;

	// コントローラー
	[SerializeField]
	private ScrollController m_scrollController = null;


	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Start()
	{
		// 設定データを読み込む
		string jsonString = null;
		PiarhythmUtility.ReadFileText(PiarhythmDatas.SETTING_DATA_FILE_PATH, ref jsonString);

		// オブジェクトに変換する
		PiarhythmDatas.SettingData settingData = JsonConvert.DeserializeObject<PiarhythmDatas.SettingData>(jsonString);

		// 設定シートの値を渡す
		m_settingSheetController.SetSettingData(settingData);

		// UIに反映させる
		m_settingSheetController.DispleySettingData();



		// 楽曲データのファイルパスを取得する
		m_musicPathDictionary = new Dictionary<string, string>();
		string[] musicPieceArray = Directory.GetFiles(PiarhythmDatas.MUSIC_PIECE_DIRECTORY_PATH, "*.json", SearchOption.AllDirectories);
		foreach(string musicPath in musicPieceArray)
		{
			// ファイル名を取得する
			string fileName = Path.GetFileNameWithoutExtension(musicPath);

			// 登録する
			m_musicPathDictionary[fileName] = musicPath;

			// タイルを作成する
			m_scrollController.CreateSoundTile(fileName);
		}
	}
	#endregion

	#region 設定データを保存する
	//-----------------------------------------------------------------
	//! @summary   設定データを保存する
	//!
	//! @parameter [settingData] 保存する設定データ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SaveSettingData(PiarhythmDatas.SettingData settingData)
	{
		// json文字列に変換する
		string jsonString = JsonConvert.SerializeObject(settingData);

		// ファイルに書き込んで保存する
		PiarhythmUtility.WriteFileText(PiarhythmDatas.SETTING_DATA_FILE_PATH, jsonString, false);
	}
	#endregion

	#region シーンを遷移させる
	//-----------------------------------------------------------------
	//! @summary   シーンを遷移させる
	//!
	//! @parameter [sceneID] 遷移させるシーンID
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void LoadScene(PiarhythmDatas.ScenenID scenenID)
	{
		if (scenenID == PiarhythmDatas.ScenenID.SCENE_PLAY)
		{
			// 選択されている曲のファイルパスを保存する
			PlayerPrefs.SetString("selectMusicPath", m_musicPathDictionary[m_selectMusic]);
		}

		// シーンを遷移する
		PiarhythmUtility.LoadScene(scenenID);
	}
	#endregion

	#region 選択されている楽曲を設定する
	//-----------------------------------------------------------------
	//! @summary   選択されている楽曲を設定する
	//!
	//! @parameter [selectMusic] 設定する曲名
	//-----------------------------------------------------------------
	public void SetSelectMusic(string selectMusic)
	{
		m_selectMusic = selectMusic;
	}
	#endregion

	#region 戻るボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   戻るボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickRevertButton()
	{
		// シーンを遷移する
		LoadScene(PiarhythmDatas.ScenenID.SCENE_TITLE);
	}
	#endregion
}
