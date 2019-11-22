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
	private GameObject m_tempoNodePrefab = null;


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
		TempoNodeController tempoNodeController = tempoNode.GetComponent<TempoNodeController>();
		m_tempDataList.Add(tempoNodeController.GetTempoData());
		tempoNodeController.SetIndex(m_tempDataList.Count - 1);

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
		GameObject tempoNode = Instantiate(m_tempoNodePrefab);

		// コンテナに登録する
		RectTransform rectTransform = tempoNode.GetComponent<RectTransform>();
		rectTransform.parent = m_tempoNodeContent;

		// 親子関係を組んだことで変化した値を修正する
		rectTransform.localScale = Vector3.one;
		rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0.0f);

		// コントローラーを設定する
		TempoNodeController tempoNodeController = tempoNode.GetComponent<TempoNodeController>();
		tempoNodeController.SetOptionSheetController(this);

		// データを追加する
		m_tempDataList.Add(tempoNodeController.GetTempoData());

		// 要素番号を設定する
		tempoNodeController.SetIndex(m_tempDataList.Count - 1);

		//　全体の時間を更新する
		CalculateWholeTime();
	}
	#endregion

	#region 全小節数の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   全小節数の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditWholeMeasureInputField()
	{
		// 入力値を取得する
		int wholeMeasure = int.Parse(m_wholeMeasureInputField.text);

		// 入力された値が0以下だった場合
		if (wholeMeasure <= 0)
		{
			// 前回の値を使用する
			m_wholeMeasureInputField.text = m_wholeMeasure.ToString();

			// 処理を終了する
			return;
		}

		// 更新する
		m_wholeMeasure = wholeMeasure;

		// 全体の時間を更新する
		CalculateWholeTime();
	}
	#endregion

	#region リスト内にあるテンポデータを更新する
	//-----------------------------------------------------------------
	//! @summary   リスト内にあるテンポデータを更新する
	//!
	//! @parameter [tempoData] 更新するテンポデータ
	//! @parameter [index] 要素番号
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateTempoData(PiarhythmDatas.TempData tempData, int index)
	{
		// データがリストの範囲外だった場合、処理を終了する
		if ((index < 0) || (index > m_tempDataList.Count)) return;

		// データを更新する
		m_tempDataList[index] = tempData;

		// 全体の時間の更新
		CalculateWholeTime();
	}
	#endregion

	#region リスト内から指定したテンポデータを削除する
	//-----------------------------------------------------------------
	//! @summary   リスト内から指定したテンポデータを削除する
	//!
	//! @parameter [tempoData] 削除するテンポデータ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void RemoveTempoData(PiarhythmDatas.TempData tempData)
	{
		// データを探す
		int index = m_tempDataList.IndexOf(tempData);

		// 見つからなかった場合は処理を終了する
		if (index < 0) return;

		// リストから削除する
		m_tempDataList.Remove(tempData);

		// 全体の時間を更新する
		CalculateWholeTime();
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
