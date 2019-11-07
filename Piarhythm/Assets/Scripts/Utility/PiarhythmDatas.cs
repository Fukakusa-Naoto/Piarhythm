//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		PiarhythmDatae.cs
//!
//! @summary	ゲームで使用する全ての構造体の管理に関するC#スクリプト
//!
//! @date		2019.10.25
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 名前空間の定義 ===========================================================
namespace PiarhythmDatas
{
	// クラスの定義 =========================================================
	[System.Serializable]
	public struct NotesData
	{
		// 音階
		public string scale;
		// 開始時間
		public float startTime;
		// 長さ
		public float length;
		// 色
		public Color color;
	}


	[System.Serializable]
	public struct BGMData
	{
		// ファイルパス
		public string path;
		// 開始時間
		public float startTime;
		// 終了時間
		public float endTime;
	}



	[System.Serializable]
	public struct MusicPieceData
	{
		// BGM
		public BGMData bgmData;
		// ノーツ
		public NotesData[] notesDataList;
	}
}