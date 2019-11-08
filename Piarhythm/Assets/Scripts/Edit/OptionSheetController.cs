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
	[SerializeField]
	private InputField m_wholeTimeInputField = null;

	[SerializeField]
	private MusicalScoreController m_musicalScoreController = null;
	[SerializeField]
	private MenuController m_menuController = null;


	// メンバ関数の定義 =====================================================
	#region 楽曲全体の時間の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   楽曲全体の時間の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditWholeTimeInputField()
	{
		// 入力が無ければ初期化する
		if (m_wholeTimeInputField.text == "") m_wholeTimeInputField.text = "0.0";

		// 変更を報告する
		m_musicalScoreController.ChangeScoreLength(float.Parse(m_wholeTimeInputField.text));
		m_menuController.UpdateDisplayWholeTimeText(float.Parse(m_wholeTimeInputField.text));
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
		m_wholeTimeInputField.text = time.ToString();
	}
	#endregion
}
