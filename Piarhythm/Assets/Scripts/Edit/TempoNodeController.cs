//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		TempoNodeController.cs
//!
//! @summary	テンポの設定をするノードの制御に関するC#スクリプト
//!
//! @date		2019.11.21
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class TempoNodeController : MonoBehaviour
{
	// <メンバ変数>
	// テンポデータ
	PiarhythmDatas.TempoData m_tempoData = null;
	int m_index = -1;

	// UI
	[SerializeField]
	private InputField m_startMeasureInputField = null;
	[SerializeField]
	private InputField m_tempoInputField = null;

	// コントローラー
	[SerializeField]
	private OptionSheetController m_optionSheetController = null;


	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Awake()
	{
		m_tempoData = ScriptableObject.CreateInstance<PiarhythmDatas.TempoData>();
		m_tempoData.m_startMeasure = 0;
		m_tempoData.m_tempo = 60;
	}
	#endregion

	#region 開始の小節が入力された時の処理
	//-----------------------------------------------------------------
	//! @summary   開始の小節が入力された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditStartMeasure()
	{
		// 入力された情報を数値に変換する
		int startMeasure = int.Parse(m_startMeasureInputField.text);

		// マイナスの値がとられていないかチェックする
		if(startMeasure < 0)
		{
			// 前回のデータを使用する
			m_startMeasureInputField.text = m_tempoData.m_startMeasure.ToString();

			// 処理を終了する
			return;
		}

		// 情報を更新する
		m_tempoData.m_startMeasure = startMeasure;

		// リスト内のデータを更新する
		m_optionSheetController.UpdateTempoData(m_tempoData, m_index);
	}
	#endregion

	#region テンポ数が入力された時の処理
	//-----------------------------------------------------------------
	//! @summary   テンポ数が入力された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditTempo()
	{
		// 入力された情報を数値に変換する
		int tempo = int.Parse(m_tempoInputField.text);

		// 0以下の値がとられていないかチェックする
		if (tempo <= 0)
		{
			// 前回のデータを使用する
			m_tempoInputField.text = m_tempoData.m_tempo.ToString();

			// 処理を終了する
			return;
		}

		// 情報を更新する
		m_tempoData.m_tempo = tempo;

		// リスト内のデータを更新する
		m_optionSheetController.UpdateTempoData(m_tempoData, m_index);
	}
	#endregion

	#region 削除ボタンがクリックされた時の処理
	//-----------------------------------------------------------------
	//! @summary   削除ボタンがクリックされた時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickDestroyButton()
	{
		// データをリストから削除する
		m_optionSheetController.RemoveTempoData(m_tempoData);

		// 自身を削除する
		Destroy(gameObject);
	}
	#endregion

	#region リストの要素番号を設定する
	//-----------------------------------------------------------------
	//! @summary   リストの要素番号を設定する
	//!
	//! @parameter [index] 設定する要素番号
	//-----------------------------------------------------------------
	public void SetIndex(int index)
	{
		m_index = index;
	}
	#endregion

	#region OptionSheetControllerを設定する
	//-----------------------------------------------------------------
	//! @summary   OptionSheetControllerを設定する
	//!
	//! @parameter [optionSheetController] 設定するOptionSheetController
	//-----------------------------------------------------------------
	public void SetOptionSheetController(OptionSheetController optionSheetController)
	{
		m_optionSheetController = optionSheetController;
	}
	#endregion

	#region テンポデータを設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツデータを設定する
	//!
	//! @parameter [tempoData] 設定するテンポデータ
	//-----------------------------------------------------------------
	public void SetTempoData(PiarhythmDatas.TempoData tempData)
	{
		// データを設定する
		m_tempoData = tempData;

		// UIへ反映させる
		m_startMeasureInputField.text = m_tempoData.m_startMeasure.ToString();
		m_tempoInputField.text = m_tempoData.m_tempo.ToString();
	}
	#endregion

	#region テンポデータを取得する
	//-----------------------------------------------------------------
	//! @summary   テンポデータを取得する
	//!
	//! @return    テンポデータ
	//-----------------------------------------------------------------
	public PiarhythmDatas.TempoData GetTempoData()
	{
		return m_tempoData;
	}
	#endregion
}
