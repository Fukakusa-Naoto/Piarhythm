//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
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
		float nowTime = PiarhythmUtility.ConvertPositionToTime(m_transform.localPosition.y, NotesManager.NOTES_SPEED);

		// 値を返す
		return nowTime;
	}
	#endregion

	#region 指定された時間の位置に座標を移動させる
	//-----------------------------------------------------------------
	//! @summary   指定された時間の位置に座標を移動させる
	//!
	//! @parameter [time] 移動させる時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetTime(float time)
	{
		Vector3 position = m_transform.localPosition;
		position.y = PiarhythmUtility.ConvertTimeToPosition(time, NotesManager.NOTES_SPEED);
		m_transform.localPosition = position;
	}
	#endregion
}
