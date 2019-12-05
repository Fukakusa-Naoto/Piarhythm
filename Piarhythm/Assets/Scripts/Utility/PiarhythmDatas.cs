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
	// #時の色の変化率
	public static readonly float SHARP_COLOR_PERCENTAGE = 0.7f;
	// 光彩の最小サイズ
	public static readonly float MIN_GLOW_SIZE = 2.0f;
	// 光彩の最大サイズ
	public static readonly float MAX_GLOW_SIZE = 10.0f;


	// 構造体の定義 =========================================================
	[System.Serializable]
	public class NoteData : ScriptableObject
	{
		// 音階
		public string m_scale = null;
		// 開始の拍数
		public float m_startBeat = 0.0f;
		// 音符の長さ
		public int m_noteLength = 0;
		// 色
		public Color m_color;
		// 連結されている前のノーツデータ
		public NoteData m_prevNoteData = null;
		// 連結されている次のノーツデータ
		public NoteData m_nextNoteData = null;
	}


	[System.Serializable]
	public class BGMData : ScriptableObject
	{
		// ファイルパス
		public string m_path = null;
		// 開始時間
		public float m_startTime = 0.0f;
		// 終了時間
		public float m_endTime = 0.0f;
	}


	[System.Serializable]
	public class MusicPieceData : ScriptableObject
	{
		// 楽曲の設定
		public OptionData m_optionData = null;
		// BGM
		public BGMData m_bgmData = null;
		// ノーツ
		public NoteData[] m_noteDataList = null;
	}


	[System.Serializable]
	public class TempoData : ScriptableObject
	{
		// テンポの開始小節
		public int m_startMeasure = 0;
		// テンポ数
		public int m_tempo = 0;
	}


	[System.Serializable]
	public class OptionData : ScriptableObject
	{
		// テンポデータのリスト
		public TempoData[] m_tempDatas = null;
		// 全小節数
		public int m_wholeMeasure = 0;
	}

	public class PositionData
	{
		// 開始座標
		public float m_position = 0.0f;
		// 長さ
		public float m_lenght = 0.0f;
	}

	public class NoteTime
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

	[System.Serializable]
	public struct Color
	{
		public float r;
		public float g;
		public float b;
		public float a;

		public Color(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
	}
}