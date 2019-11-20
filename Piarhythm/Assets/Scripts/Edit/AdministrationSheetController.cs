
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		AdministrationSheetController.cs
//!
//! @summary	BGM管理シートに関するC#スクリプト
//!
//! @date		2019.11.20
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class AdministrationSheetController : MonoBehaviour
{
	// <メンバ変数>
	// UI
	[SerializeField]
	private Text m_fpsText = null;


	// メンバ関数の定義 =====================================================
	#region 更新処理
	//-----------------------------------------------------------------
	//! @summary   更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Update()
    {
		int fps = (int)(1.0f / Time.deltaTime);
		m_fpsText.text = "FPS　：　" + fps.ToString();
	}
	#endregion
}
