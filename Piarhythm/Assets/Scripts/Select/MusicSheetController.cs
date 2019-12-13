//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		MusicSheetController.cs
//!
//! @summary	楽曲シートの制御に関するC#スクリプト
//!
//! @date		2019.12.11
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class MusicSheetController : MonoBehaviour
{
	// <メンバ変数>
	// UI
	[SerializeField]
	private Text m_selectMusicText = null;


	// メンバ関数の定義 =====================================================
	#region 選択されている楽曲名のテキストUIを更新する
	//-----------------------------------------------------------------
	//! @summary   選択されている楽曲名のテキストUIを更新する
	//!
	//! @parameter [musicName] 表示する曲名
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void DisplaySelectMusicName(string musicName)
	{
		m_selectMusicText.text = musicName;
	}
	#endregion
}
