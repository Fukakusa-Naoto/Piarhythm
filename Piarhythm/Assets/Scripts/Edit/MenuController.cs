//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		MenuController.cs
//!
//! @summary	メニュー操作に関するC#スクリプト
//!
//! @date		2019.11.05
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class MenuController : MonoBehaviour
{
	// <メンバ変数>
	[SerializeField]
	private Text m_wholeText = null;


	// メンバ関数の定義 =====================================================
	#region 楽曲全体の長さの変更があった時の表示の更新処理
	//-----------------------------------------------------------------
	//! @summary   楽曲全体の長さの変更があった時の表示の更新処理
	//!
	//! @parameter [wholeTime] 楽曲全体の長さ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateDisplayWholeTimeText(float wholeTime)
	{
		// 表示する文字列の作成
		string displayStr = "\t";
		displayStr += wholeTime.ToString();

		// UIの更新
		m_wholeText.text = displayStr;
	}
	#endregion
}
