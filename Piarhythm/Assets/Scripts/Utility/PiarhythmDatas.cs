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


// クラスの定義 =============================================================
public class PiarhythmDatas
{
	// 定数の定義 ===========================================================
	// 全てのデータを保存しているフォルダの階層パス
	public static readonly string DATA_DIRECTORY_PATH = Application.dataPath + "/StreamingAssets/";
	// BGMデータを保存しているフォルダの階層パス
	public static readonly string BGM_DIRECTORY_PATH = DATA_DIRECTORY_PATH + "BGM/";
	// 楽曲データを保存しているフォルダの階層パス
	public static readonly string MUSIC_PIECE_DIRECTORY_PATH = DATA_DIRECTORY_PATH + "Data/MusicPiece/";


	// 構造体の定義 =========================================================
	[System.Serializable]
	public struct NotesData
	{
		// 音階
		public string scale;
		// 開始の拍数
		public float startBeat;
		// 音符の長さ
		public int noteLength;
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
		// 楽曲の設定
		public OptionData optionData;
		// BGM
		public BGMData bgmData;
		// ノーツ
		public NotesData[] notesDataList;
	}


	[System.Serializable]
	public struct TempoData
	{
		// テンポの開始小節
		public int startMeasure;
		// テンポ数
		public int tempo;
	}


	[System.Serializable]
	public struct OptionData
	{
		// テンポデータのリスト
		public TempoData[] tempDatas;
		// 全小節数
		public int wholeMeasure;
	}

	public struct PositionData
	{
		// 開始座標
		public float position;
		// 長さ
		public float lenght;
	}

	public struct NoteTime
	{
		// 全音符
		public static float WHOLE_NOTE_SEMIBREVE = 4.0f;
		// 2分音符
		public static float HALF_NOTE_MININ = 2.0f;
		// 4分音符
		public static float QUARTER_NOTE_CROCHET = 1.0f;
		// 8分音符
		public static float EIGHTH_NOTE_QUAVER = 0.5f;
		// 16分音符
		public static float SIXTEENTH_NOTE_SEMIQUAVER = 0.25f;
		// 付点全音符
		public static float WHOLE_DOTTED_NOTE_SEMIBREVE = 6.0f;
		// 付点2分音符
		public static float HALF_DOTTED_NOTE_MININ = 3.0f;
		// 付点4分音符
		public static float QUARTER_DOTTED_NOTE_CROCHET = 1.5f;
		// 付点8分音符
		public static float EIGHTH_DOTTED_NOTE_QUAVER = 0.75f;
	}
}