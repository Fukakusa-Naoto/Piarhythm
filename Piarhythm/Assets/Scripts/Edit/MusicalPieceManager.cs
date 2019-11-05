//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		MusicalPieceManager.cs
//!
//! @summary	楽曲設定に関するC#スクリプト
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
public class MusicalPieceManager : MonoBehaviour
{
	// <メンバ変数>
	[SerializeField]
	private GameObject m_wholeTimeInputField = null;

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
		// コンポーネントの取得
		InputField inputField = m_wholeTimeInputField.GetComponent<InputField>();
		if (inputField.text == "") inputField.text = "0.0";

		// 変更を報告する
		m_musicalScoreController.ChangeScoreLength(float.Parse(inputField.text));
		m_menuController.UpdateDisplayWholeTimeText(float.Parse(inputField.text));
	}
	#endregion
}
