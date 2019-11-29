//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		ConnectNoteSheetController.cs
//!
//! @summary	連結ノーツの制御に関するC#スクリプト
//!
//! @date		2019.11.29
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class ConnectNoteSheetController : MonoBehaviour
{
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

	#region シートを手前に持ってくる処理
	//-----------------------------------------------------------------
	//! @summary   シートを手前に持ってくる処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetAsLastSibling()
	{
		m_transform.SetAsLastSibling();
	}
	#endregion

	#region シートを奥に持っていく処理
	//-----------------------------------------------------------------
	//! @summary   シートを奥に持っていく処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetAsFirstSibling()
	{
		m_transform.SetAsFirstSibling();
	}
	#endregion
}
