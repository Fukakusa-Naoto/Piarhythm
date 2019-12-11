//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		ScrollController.cs
//!
//! @summary	楽曲タイルのスクロールに関するC#スクリプト
//!
//! @date		2019.09.05
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;


// クラスの定義 =============================================================
public class ScrollController : MonoBehaviour
{
	// <メンバ変数>
	// 楽曲タイルのプレハブ
	[SerializeField]
	private GameObject m_soundTilePrefab = null;


	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Start()
    {
		// 楽曲データを取得する
		string[] musicPieceArray = Directory.GetFiles(PiarhythmDatas.MUSIC_PIECE_DIRECTORY_PATH, "*.json", SearchOption.AllDirectories);

		foreach(string musicPiece in musicPieceArray)
		{
			// タイルを作成する
			GameObject tile = Instantiate(m_soundTilePrefab);

			// テキストを更新する
			string musicName = Path.GetFileNameWithoutExtension(musicPiece);
			tile.GetComponent<SoundTileController>().SetMusicName(musicName);

			// 親子関係を組む
			tile.transform.SetParent(transform);
		}
	}
	#endregion


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
