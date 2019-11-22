//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		OptionSheetController.cs
//!
//! @summary	設定シートに関するC#スクリプト
//!
//! @date		2019.10.29
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class OptionSheetController : MonoBehaviour
{
	// <メンバ変数>
	private float m_wholeTime = 0.0f;
	private List<PiarhythmDatas.TempData> m_tempDataList;
	private int m_wholeMeasure = 0;

	// データ
	private PiarhythmDatas.OptionData m_optionData;

	// UI
	[SerializeField]
	private InputField m_wholeMeasureInputField = null;
	[SerializeField]
	private RectTransform m_tempoNodeContent;

	// コントローラー
	[SerializeField]
	private MusicalScoreController m_musicalScoreController = null;
	[SerializeField]
	private MenuController m_menuController = null;
	[SerializeField]
	private BGMSheetController m_bgmSheetController = null;
	[SerializeField]
	private NotesEditScrollbarController m_notesEditScrollbarController = null;

	// プレハブ
	[SerializeField]
	private GameObject tempoNodePrefab = null;


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
		m_tempDataList = new List<PiarhythmDatas.TempData>();
		m_optionData = new PiarhythmDatas.OptionData();

		// 最初のテンポデータを登録する
		GameObject tempoNode = GameObject.FindGameObjectWithTag("TempoNode");
		m_tempDataList.Add(tempoNode.GetComponent<TempoNodeController>().GetTempoData());

		// 全小節数を取得する
		m_wholeMeasure = int.Parse(m_wholeMeasureInputField.text);

		// 全体時間を計算する
		CalculateWholeTime();
	}
	#endregion

	#region 全体時間を計算する
	//-----------------------------------------------------------------
	//! @summary   全体時間を計算する
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void CalculateWholeTime()
	{
		// 時間を初期化
		m_wholeTime = 0.0f;

		// テンポデータ分計算する
		PiarhythmDatas.TempData prevTempData = new PiarhythmDatas.TempData();
		prevTempData.tempo = 0;
		foreach (PiarhythmDatas.TempData tempData in m_tempDataList)
		{
			if (prevTempData.tempo != 0)
			{
				// 一拍当たりの時間を求める
				float beatPerTempo = 60.0f / prevTempData.tempo;

				for (int i = prevTempData.startMeasure; i < tempData.startMeasure; ++i)
				{
					// 1小節分加算する
					m_wholeTime += beatPerTempo * 4.0f;
				}
			}

			prevTempData = tempData;
		}

		// 最後に残りの小節分加算する
		// 一拍当たりの時間を求める
		{
			float beatPerTempo = 60.0f / prevTempData.tempo;

			for (int i = prevTempData.startMeasure; i < m_wholeMeasure; ++i)
			{
				// 1小節分加算する
				m_wholeTime += beatPerTempo * 4.0f;
			}
		}

		// 各UIへ反映させる
		m_musicalScoreController.ChangeScoreLength(m_wholeTime);
		m_menuController.UpdateDisplayWholeTimeText(m_wholeTime);

		// スクロールバーのテクスチャを更新する
		m_notesEditScrollbarController.UpdateTexture(m_bgmSheetController.GetBGMData(), m_wholeTime);
	}
	#endregion

	#region テンポの追加ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   テンポの追加ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickAddTempoButton()
	{
		// テンポノードを作成する

		// コンテナに登録する

		// データを追加する

		//　全体の時間を更新する

	}
	#endregion

	#region 楽曲全体の時間の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   楽曲全体の時間の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditWholeMeasureInputField()
	{
		//// 入力が無ければ初期化する
		//if (m_wholeMeasureInputField.text == "") m_wholeTimeInputField.text = "0.0";

		//// 変更を報告する
		//m_wholeTime = float.Parse(m_wholeTimeInputField.text);
		//m_musicalScoreController.ChangeScoreLength(m_wholeTime);
		//m_menuController.UpdateDisplayWholeTimeText(m_wholeTime);

		//// スクロールバーのテクスチャを更新する
		//m_notesEditScrollbarController.UpdateTexture(m_bgmSheetController.GetBGMData(), m_wholeTime);
	}
	#endregion

	#region BGMの読み込みがあった時の楽曲全体の長さの更新
	//-----------------------------------------------------------------
	//! @summary   BGMの読み込みがあった時の楽曲全体の長さの更新
	//!
	//! @parameter [time] BGMの時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetWholeTime(float time)
	{
		//m_wholeTime = time;
		//m_wholeTimeInputField.text = m_wholeTime.ToString();
	}
	#endregion

	#region 楽曲全体の時間の取得
	//-----------------------------------------------------------------
	//! @summary   楽曲全体の時間の取得
	//!
	//! @return    楽曲全体の時間
	//-----------------------------------------------------------------
	public float GetWholeTime()
	{
		return m_wholeTime;
	}
	#endregion
}
