//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		SettingManager.cs
//!
//! @summary	設定シーンの管理に関するC#スクリプト
//!
//! @date		2019.08.06
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// クラスの定義 =============================================================
[System.Serializable]
public struct SystemData
{
	// ノーツの流れる速度
	public float speed;
	// 鍵盤の数
	public int keyNumber;
}


public class SettingManager : MonoBehaviour
{
	// <メンバ変数>
	private SystemData m_systemData;
	public Text m_speedText;
	public Text m_keyText;


	// メンバ関数の定義 =====================================================
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Start()
	{
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

		// UIのテキストに反映
		m_speedText.text = m_systemData.speed.ToString("F1");
		m_keyText.text = m_systemData.keyNumber.ToString() + "鍵盤";
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツ速度を上げる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnUpSpeedButton()
	{
		// スピードを上げる
		m_systemData.speed += 0.1f;
		// 最大最小を超えないように制限をかける
		m_systemData.speed = Mathf.Clamp(m_systemData.speed, 1.0f, 10.0f);
		// UIに反映する
		m_speedText.text = m_systemData.speed.ToString("F1");
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツ速度を下げる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnDownSpeedButton()
	{
		// スピードを下げる
		m_systemData.speed -= 0.1f;
		// 最大最小を超えないように制限をかける
		m_systemData.speed = Mathf.Clamp(m_systemData.speed, 1.0f, 10.0f);
		// UIに反映する
		m_speedText.text = m_systemData.speed.ToString("F1");
	}



	//-----------------------------------------------------------------
	//! @summary   鍵盤数を上げる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnUpKeyNumberButton()
	{
		// 鍵盤数を上げる
		switch(m_systemData.keyNumber)
		{
			case 88: break;
			case 76: m_systemData.keyNumber = 88; break;
			case 61: m_systemData.keyNumber = 76; break;
			case 44: m_systemData.keyNumber = 61; break;
			default: m_systemData.keyNumber = 61; break;
		}

		// テキストに反映させる
		m_keyText.text = m_systemData.keyNumber.ToString() + "鍵盤";
	}



	//-----------------------------------------------------------------
	//! @summary   鍵盤数を下げる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnDownKeyNumberButton()
	{
		// 鍵盤数を下げる
		switch(m_systemData.keyNumber)
		{
			case 88: m_systemData.keyNumber = 76; break;
			case 76: m_systemData.keyNumber = 61; break;
			case 61: m_systemData.keyNumber = 44; break;
			case 44: break;
			default: m_systemData.keyNumber = 44; break;
		}

		// テキストに反映させる
		m_keyText.text = m_systemData.keyNumber.ToString() + "鍵盤";
	}



	//-----------------------------------------------------------------
	//! @summary   設定シーンを閉じる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnRevertButton()
	{
		// json文字列に変換する
		string json = JsonUtility.ToJson(m_systemData);

		// ファイルに書き出し
		string dataFilePath = UnityEngine.Application.dataPath + "/StreamingAssets/Data/System/SystemData.json";
		File.WriteAllText(dataFilePath, json);

		// 前のシーンに戻る
		SceneManager.UnloadSceneAsync((int)ScenenID.SCENE_SETTING);
	}
}
