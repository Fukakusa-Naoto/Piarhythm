//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		MusicController.cs
//!
//! @summary	ノーツスクロールに関するC#スクリプト
//!
//! @date		2019.10.04
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class MusicController : MonoBehaviour
{
	// <メンバ変数>
	private PiarhythmDatas.TempoData[] m_tempoDataList = null;
	// キーボード情報
	private Dictionary<string, RectTransform> m_keyDictionary = null;
	private float m_wholeTime = 0.0f;
	private PiarhythmDatas.SettingData m_settingData = null;

	// コンポーネント
	private RectTransform m_transform = null;

	public GameObject m_musicScoreBackGroundPrefab = null;
	public GameObject m_notePrefab = null;

	[SerializeField]
	private GameObject m_keyboard = null;



	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Awake()
	{
		// コンポーネントの取得
		m_transform = GetComponent<RectTransform>();

		// キーボード情報を登録する
		m_keyDictionary = new Dictionary<string, RectTransform>();
		RectTransform rectTransform = m_keyboard.GetComponent<RectTransform>();
		for (int i = 0; i < rectTransform.childCount; ++i)
		{
			RectTransform keyTransform = rectTransform.GetChild(i).GetComponent<RectTransform>();
			m_keyDictionary[keyTransform.name] = keyTransform;
		}
	}
	#endregion

	#region 再生中の更新処理
	//-----------------------------------------------------------------
	//! @summary   再生中の更新処理
	//!
	//! @parameter [elapsedTime] 経過時間
	//! @parameter [noteSpeed] ノーツ速度
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdatePlay(float elapsedTime, float noteSpeed)
	{
		Vector3 position = m_transform.localPosition;
		position.y = -PiarhythmUtility.ConvertTimeToPosition(elapsedTime, noteSpeed * 20.0f);
		m_transform.anchoredPosition = position;
	}
	#endregion

	#region 背景の生成処理
	//-----------------------------------------------------------------
	//! @summary   背景の生成処理
	//!
	//! @parameter [startTime] 小節の開始時間
	//! @parameter [endTime] 小節の終了時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void CreateMusicScoreBackGround(float startTime, float endTime)
	{
		// オブジェクトを生成する
		GameObject backGround = Instantiate(m_musicScoreBackGroundPrefab);

		// 親子関係を組ませる
		RectTransform rectTransform = backGround.GetComponent<RectTransform>();
		if (m_transform) rectTransform.SetParent(m_transform);

		// 親子関係を組んだことで変化した値を修正する
		rectTransform.localScale = Vector3.one;
		rectTransform.anchoredPosition = new Vector3(0.0f, rectTransform.anchoredPosition.y);
		rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0.0f);

		// 開始時間と終了時間を座標に変換する
		Vector2 offsetMin = rectTransform.offsetMin;
		Vector2 offsetMax = rectTransform.offsetMax;
		offsetMin.y = PiarhythmUtility.ConvertTimeToPosition(startTime, m_settingData.m_noteSpeed * 20.0f);
		offsetMax.y = PiarhythmUtility.ConvertTimeToPosition(endTime, m_settingData.m_noteSpeed * 20.0f);

		// 設定する
		rectTransform.offsetMin = offsetMin;
		rectTransform.offsetMax = offsetMax;
	}
	#endregion

	#region 設定データから背景を作成する
	//-----------------------------------------------------------------
	//! @summary   設定データから背景を作成する
	//!
	//! @parameter [optionData] 設定データ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void CreateMusicScoreBackGround(PiarhythmDatas.OptionData optionData)
	{
		// テンポデータの保存
		m_tempoDataList = optionData.m_tempDatas;

		// 時間を初期化
		m_wholeTime = 0.0f;

		// テンポデータ分計算する
		PiarhythmDatas.TempoData prevTempData = ScriptableObject.CreateInstance<PiarhythmDatas.TempoData>();
		prevTempData.m_tempo = 0;
		foreach (PiarhythmDatas.TempoData tempoData in optionData.m_tempDatas)
		{
			if (prevTempData.m_tempo != 0)
			{
				// 一拍当たりの時間を求める
				float beatPerTempo = 60.0f / prevTempData.m_tempo;

				for (int i = prevTempData.m_startMeasure; i < tempoData.m_startMeasure; ++i)
				{
					// 開始時間を保存する
					float startTime = m_wholeTime;

					// 1小節分加算する
					m_wholeTime += beatPerTempo * 4.0f;

					// 背景を生成する
					CreateMusicScoreBackGround(startTime, m_wholeTime);
				}
			}

			prevTempData = tempoData;
		}

		// 最後に残りの小節分加算する
		// 一拍当たりの時間を求める
		{
			float beatPerTempo = 60.0f / prevTempData.m_tempo;

			for (int i = prevTempData.m_startMeasure; i < optionData.m_wholeMeasure; ++i)
			{
				// 開始時間を保存する
				float startTime = m_wholeTime;

				// 1小節分加算する
				m_wholeTime += beatPerTempo * 4.0f;

				// 背景を生成する
				CreateMusicScoreBackGround(startTime, m_wholeTime);
			}
		}
	}
	#endregion

	#region ノーツの生成処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの生成処理
	//!
	//! @parameter [noteList] 生成するノーツデータのリスト
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void CreateNoteList(PiarhythmDatas.NoteData[] noteList)
	{
		foreach(PiarhythmDatas.NoteData noteData in noteList)
		{
			// インスタンスの作成
			GameObject note = Instantiate(m_notePrefab);

			// コンポーネントの取得
			NoteController noteController = note.GetComponent<NoteController>();

			// データを設定する
			noteController.SetNoteData(noteData);
			noteController.SetMusicController(this);
			noteController.SetKeyDictionary(m_keyDictionary);

			// 親子関係を組む
			note.GetComponent<RectTransform>().SetParent(m_transform);
		}
	}
	#endregion

	#region ノーツの開始拍と音符から開始座標と長さに変換する
	//-----------------------------------------------------------------
	//! @summary   ノーツの開始拍と音符から開始座標と長さに変換する
	//!
	//! @parameter [startBeat] 開始の拍数
	//! @parameter [noteLength] 音符の長さ
	//!
	//! @return    変換された座標
	//-----------------------------------------------------------------
	public PiarhythmDatas.PositionData ConvertToPositionData(float startBeat, int noteLenght)
	{
		PiarhythmDatas.PositionData positionData = new PiarhythmDatas.PositionData();
		float elapsedBeat = 0.0f;
		float elapsedPosition = 0.0f;

		// 所属しているテンポデータを調べる
		PiarhythmDatas.TempoData tempoData = m_tempoDataList[0];
		for (int i = 1; i < m_tempoDataList.Length; ++i)
		{
			if (startBeat >= m_tempoDataList[i].m_startMeasure * 4)
			{
				// 一拍当たりの時間を求める
				float beatPerTempo = 60.0f / tempoData.m_tempo;
				// 時間を座標に変換する
				float beatPosition = PiarhythmUtility.ConvertTimeToPosition(beatPerTempo, m_settingData.m_noteSpeed * 20.0f);

				// テンポデータの終了座標を求める
				elapsedPosition += beatPosition * ((m_tempoDataList[i].m_startMeasure - tempoData.m_startMeasure) * 4);

				// 経過拍数を増やす
				elapsedBeat += (m_tempoDataList[i].m_startMeasure - tempoData.m_startMeasure) * 4;

				tempoData = m_tempoDataList[i];
			}
			else
			{
				break;
			}
		}

		{
			// 一拍当たりの時間を求める
			float beatPerTempo = 60.0f / tempoData.m_tempo;
			// 時間を座標に変換する
			float beatPosition = PiarhythmUtility.ConvertTimeToPosition(beatPerTempo, m_settingData.m_noteSpeed * 20.0f);

			// テンポデータを元に座標と長さを決める
			positionData.m_position = elapsedPosition + (startBeat - elapsedBeat) * beatPosition;

			switch (noteLenght)
			{
				case 0:
					positionData.m_lenght = beatPosition * PiarhythmDatas.NoteTime.WHOLE_NOTE_SEMIBREVE;
					break;
				case 1:
					positionData.m_lenght = beatPosition * PiarhythmDatas.NoteTime.HALF_NOTE_MININ;
					break;
				case 2:
					positionData.m_lenght = beatPosition * PiarhythmDatas.NoteTime.QUARTER_NOTE_CROCHET;
					break;
				case 3:
					positionData.m_lenght = beatPosition * PiarhythmDatas.NoteTime.EIGHTH_NOTE_QUAVER;
					break;
				case 4:
					positionData.m_lenght = beatPosition * PiarhythmDatas.NoteTime.SIXTEENTH_NOTE_SEMIQUAVER;
					break;
				case 5:
					positionData.m_lenght = beatPosition * PiarhythmDatas.NoteTime.WHOLE_DOTTED_NOTE_SEMIBREVE;
					break;
				case 6:
					positionData.m_lenght = beatPosition * PiarhythmDatas.NoteTime.HALF_DOTTED_NOTE_MININ;
					break;
				case 7:
					positionData.m_lenght = beatPosition * PiarhythmDatas.NoteTime.QUARTER_DOTTED_NOTE_CROCHET;
					break;
				case 8:
					positionData.m_lenght = beatPosition * PiarhythmDatas.NoteTime.EIGHTH_DOTTED_NOTE_QUAVER;
					break;
			}
		}

		return positionData;
	}
	#endregion

	#region 全体の時間を取得する
	//-----------------------------------------------------------------
	//! @summary   全体の時間を取得する
	//!
	//! @return    全体の時間
	//-----------------------------------------------------------------
	public float GetWholeTime()
	{
		return m_wholeTime;
	}
	#endregion

	#region 設定データを設定する
	//-----------------------------------------------------------------
	//! @summary   設定データを設定する
	//!
	//! @parameter [settingData] 設定する設定データ
	//-----------------------------------------------------------------
	public void SetSettingData(PiarhythmDatas.SettingData settingData)
	{
		m_settingData = settingData;
	}
	#endregion
}
