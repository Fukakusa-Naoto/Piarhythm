﻿//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		MusicalScoreController.cs
//!
//! @summary	楽譜に関するC#スクリプト
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
public class MusicalScoreController : MonoBehaviour
{
	// <メンバ定数>
	private static readonly float MIN_HEIGHT = 368.9f;

	// <メンバ変数>
	// コンポーネント
	private RectTransform m_transform = null;
	[SerializeField]
	private GameObject m_musicScoreBackGroundPrefab = null;


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
		// コンポーネントの取得
		m_transform = GetComponent<RectTransform>();
	}
	#endregion

	#region 楽曲の長さが変更された時の処理
	//-----------------------------------------------------------------
	//! @summary   楽曲の長さが変更された時の処理
	//!
	//! @parameter [length] 楽曲の長さ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void ChangeScoreLength(float length)
	{
		// 時間を座標に変換
		float height = (length * NotesManager.NOTES_SPEED);

		if (height < MIN_HEIGHT) height = MIN_HEIGHT;
		else height += MIN_HEIGHT;

		m_transform.sizeDelta = new Vector2(m_transform.sizeDelta.x, height);
	}
	#endregion

	#region 現在時間の取得
	//-----------------------------------------------------------------
	//! @summary   現在時間の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    現在時間
	//-----------------------------------------------------------------
	public float GetNowTime()
	{
		// 座標を時間に変換する
		float position = -m_transform.offsetMin.y;
		float nowTime = PiarhythmUtility.ConvertPositionToTime(position, NotesManager.NOTES_SPEED);

		// 値を返す
		return nowTime;
	}
	#endregion

	#region 指定された時間の位置に座標を移動させる
	//-----------------------------------------------------------------
	//! @summary   指定された時間の位置に座標を移動させる
	//!
	//! @parameter [nowTime] 移動させる時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetNowTime(float nowTime)
	{
		Vector3 position = m_transform.localPosition;
		position.y = m_transform.sizeDelta.y - PiarhythmUtility.ConvertTimeToPosition(nowTime, NotesManager.NOTES_SPEED);
		m_transform.anchoredPosition = position;
	}
	#endregion

	#region 背景の生成
	//-----------------------------------------------------------------
	//! @summary   背景の生成
	//!
	//! @parameter [startTime] 小節の開始時間
	//! @parameter [endTime] 小節の終了時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void CreateMusicScoreBackGround(float startTime, float endTime)
	{
		// オブジェクトを生成する
		GameObject backGround = Instantiate(m_musicScoreBackGroundPrefab);

		// 親子関係を組ませる
		RectTransform rectTransform = backGround.GetComponent<RectTransform>();
		if (m_transform) rectTransform.SetParent(m_transform);

		// 生成した背景を常にヒエラルキーの上に設定する
		rectTransform.SetSiblingIndex(1);

		// 親子関係を組んだことで変化した値を修正する
		rectTransform.localScale = Vector3.one;
		rectTransform.anchoredPosition = new Vector3(0.0f, rectTransform.anchoredPosition.y);
		rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0.0f);

		// 開始時間と終了時間を座標に変換する
		Vector2 offsetMin = rectTransform.offsetMin;
		Vector2 offsetMax = rectTransform.offsetMax;
		offsetMin.y = PiarhythmUtility.ConvertTimeToPosition(startTime, NotesManager.NOTES_SPEED);
		offsetMax.y = PiarhythmUtility.ConvertTimeToPosition(endTime, NotesManager.NOTES_SPEED);

		// 設定する
		rectTransform.offsetMin = offsetMin;
		rectTransform.offsetMax = offsetMax;
	}
	#endregion

	#region 背景をリセットする
	//-----------------------------------------------------------------
	//! @summary   背景をリセットする
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void ResetMusicScoreBackGround()
	{
		for (int i = 1; i < m_transform.childCount; ++i)
		{
			if (m_transform.GetChild(i).tag != "Notes")
			{
				Destroy(m_transform.GetChild(i).gameObject);
			}
		}
	}
	#endregion
}
