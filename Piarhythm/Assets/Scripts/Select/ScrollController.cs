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

	// マネージャー
	[SerializeField]
	private SelectManager m_selectManager = null;

	// コントローラー
	[SerializeField]
	private MusicSheetController m_musicSheetController = null;


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

			// コンポーネントを取得する
			SoundTileController soundTileController = tile.GetComponent<SoundTileController>();

			// 曲名を設定する
			string musicName = Path.GetFileNameWithoutExtension(musicPiece);
			soundTileController.SetMusicName(musicName);

			// SelectManagerを設定する
			soundTileController.SetSelectManager(m_selectManager);

			// MusicSheetControllerを設定する
			soundTileController.SetMusicSheetController(m_musicSheetController);

			// 親子関係を組む
			tile.transform.SetParent(transform);
		}
	}
	#endregion
}
