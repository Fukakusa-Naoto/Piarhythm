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
using System.IO;


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
		m_musicPieceArray = System.IO.Directory.GetFiles(
			UnityEngine.Application.dataPath + "/Resources/Data/MusicPiece",
			"*.json",
			System.IO.SearchOption.AllDirectories);
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



	//-----------------------------------------------------------------
	//! @summary   楽曲名リストの取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public string[] GetMusicPieceList()
	{
		string[] musicPieceList = new string[m_musicPieceArray.Length];

		for (int i = 0; i < m_musicPieceArray.Length; ++i)
		{
			// 文字列を分割する
			string[] str = m_musicPieceArray[i].Split('\\');

			// 拡張子を調べる
			string extension = Path.GetExtension(str[str.Length - 1]);

			// 拡張子が無し
			if (string.IsNullOrEmpty(extension))
			{
				// そのまま代入
				musicPieceList[i] = str[str.Length - 1];
			}

			// 曲名だけ取得する
			musicPieceList[i] = str[str.Length - 1].Replace(extension, string.Empty);
		}

		return musicPieceList;
	}
}
