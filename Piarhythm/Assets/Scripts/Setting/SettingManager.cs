﻿//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
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


// クラスの定義 =============================================================
[System.Serializable]
public struct SystemData
{
	// ノーツの流れる速度
	public float speed { get; set; }
	// 鍵盤の数
	public int keyNumber { get; set; }
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
		string dataFilePass = UnityEngine.Application.dataPath + "/Resources/Data/System/SystemData.json";
		m_systemData = new SystemData();

		// ファイルの有無を調べる
		if (File.Exists(dataFilePass))
		{
			// ファイルを読み込む
			string json = File.ReadAllText(dataFilePass);
			JsonUtility.FromJsonOverwrite(json, m_systemData);
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
	//! @summary   ノーツ速度の上昇
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpSpeed()
	{
		// スピードを上げる
		m_systemData.speed += 0.1f;
		// 最大最小を超えないように制限をかける
		m_systemData.speed = Mathf.Clamp(m_systemData.speed, 1.0f, 10.0f);
		// UIに反映する
		m_speedText.text = m_systemData.speed.ToString("F1");
	}



}