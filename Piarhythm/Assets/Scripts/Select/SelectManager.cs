//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		SelectManager.cs
//!
//! @summary	楽曲選択シーンの管理に関するC#スクリプト
//!
//! @date		2019.08.29
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// クラスの定義 =============================================================
public class SelectManager : MonoBehaviour
{
	// <メンバ変数>
	private string[] m_musicPieceArray;


	// メンバ関数の定義 =====================================================
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Start()
    {
		// フォルダ内の全てのjsonファイルを取得する
		m_musicPieceArray= System.IO.Directory.GetFiles(UnityEngine.Application.dataPath + "/Resources/Data/MusicPiece", "*.json", System.IO.SearchOption.AllDirectories);

		foreach (var n in m_musicPieceArray) Debug.Log(n);
	}



	//-----------------------------------------------------------------
	//! @summary   更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Update()
    {

    }
}
