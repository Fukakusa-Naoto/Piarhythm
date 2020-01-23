//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		SettingSheetController.cs
//!
//! @summary	設定シートの制御に関するC#スクリプト
//!
//! @date		2019.12.12
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class SettingSheetController : MonoBehaviour
{
	// <メンバ変数>
	private PiarhythmDatas.SettingData m_settingData = null;

	// UI
	[SerializeField]
	private InputField m_noteSpeedInputField = null;
	[SerializeField]
	private Toggle m_practiceToggle = null;

	// マネージャー
	[SerializeField]
	private SelectManager m_selectManager = null;


	// メンバ関数の定義 =====================================================
	#region ノーツ速度の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツ速度の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditNoteSpeedInputField()
	{
		int noteSpeed = Mathf.Clamp(int.Parse(m_noteSpeedInputField.text), 1, 10);
		m_settingData.m_noteSpeed = noteSpeed;
		m_noteSpeedInputField.text = noteSpeed.ToString();
	}
	#endregion

	#region ノーツ速度減少のボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツ速度減少のボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickSpeedDownButton()
	{
		m_settingData.m_noteSpeed = (m_settingData.m_noteSpeed <= 1) ? 1 : m_settingData.m_noteSpeed - 1;
		DispleySettingData();
	}
	#endregion

	#region ノーツ速度上昇のボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツ速度上昇のボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickSpeedUpButton()
	{
		m_settingData.m_noteSpeed = (m_settingData.m_noteSpeed >= 10) ? 10 : m_settingData.m_noteSpeed + 1;
		DispleySettingData();
	}
	#endregion

	#region 練習モード切替のチェックボックスの入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   練習モード切替のチェックボックスの入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnValueChangedPtacticeToggle()
	{
		m_settingData.m_practiceFlag = m_practiceToggle.isOn;
	}
	#endregion

	#region 開始ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   開始ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickStartButton()
	{
		// 設定データを保存する
		m_selectManager.SaveSettingData(m_settingData);

		// プレイシーンに遷移する
		m_selectManager.LoadScene(PiarhythmDatas.ScenenID.SCENE_PLAY);
	}
	#endregion

	#region 設定データをUIへ反映させる処理
	//-----------------------------------------------------------------
	//! @summary   設定データをUIへ反映させる処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void DispleySettingData()
	{
		m_noteSpeedInputField.text = m_settingData.m_noteSpeed.ToString();
		m_practiceToggle.isOn = m_settingData.m_practiceFlag;
	}
	#endregion

	#region 設定データを取得する
	//-----------------------------------------------------------------
	//! @summary   設定データを取得する
	//!
	//! @return    設定データ
	//-----------------------------------------------------------------
	public PiarhythmDatas.SettingData GetSettingData()
	{
		return m_settingData;
	}
	#endregion

	#region 設定データを設定する
	//-----------------------------------------------------------------
	//! @summary   設定データを設定する
	//!
	//! @parameter [settingData] 設定する設定データ
	//-----------------------------------------------------------------
	public void SetSettingData(PiarhythmDatas.SettingData settingData)
	{
		m_settingData = settingData;
	}
	#endregion
}
