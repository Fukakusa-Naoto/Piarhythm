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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}
