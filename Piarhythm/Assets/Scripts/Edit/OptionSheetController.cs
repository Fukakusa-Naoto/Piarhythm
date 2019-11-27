//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		OptionSheetController.cs
//!
//! @summary	設定シートに関するC#スクリプト
//!
//! @date		2019.10.29
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class OptionSheetController : MonoBehaviour
{
	// <メンバ変数>
	private float m_wholeTime = 0.0f;
	private List<PiarhythmDatas.TempoData> m_tempoDataList;
	private int m_wholeMeasure = 0;

	// データ
	private PiarhythmDatas.OptionData m_optionData;

	// UI
	[SerializeField]
	private InputField m_wholeMeasureInputField = null;
	[SerializeField]
	private RectTransform m_tempoNodeContent = null;

	// コントローラー
	[SerializeField]
	private MusicalScoreController m_musicalScoreController = null;
	[SerializeField]
	private MenuController m_menuController = null;
	[SerializeField]
	private BGMSheetController m_bgmSheetController = null;
	[SerializeField]
	private NotesEditScrollbarController m_notesEditScrollbarController = null;

	// プレハブ
	[SerializeField]
	private GameObject m_tempoNodePrefab = null;


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
		m_tempoDataList = new List<PiarhythmDatas.TempoData>();
		m_optionData = new PiarhythmDatas.OptionData();

		// 最初のテンポデータを登録する
		GameObject tempoNode = GameObject.FindGameObjectWithTag("TempoNode");
		TempoNodeController tempoNodeController = tempoNode.GetComponent<TempoNodeController>();
		m_tempoDataList.Add(tempoNodeController.GetTempoData());
		tempoNodeController.SetIndex(m_tempoDataList.Count - 1);

		// 全小節数を取得する
		m_wholeMeasure = int.Parse(m_wholeMeasureInputField.text);

		// 全体時間を計算する
		CalculateWholeTime();
	}
	#endregion

	#region 全体時間を計算する
	//-----------------------------------------------------------------
	//! @summary   全体時間を計算する
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void CalculateWholeTime()
	{
		// 背景をリセットする
		m_musicalScoreController.ResetMusicScoreBackGround();

		// 時間を初期化
		m_wholeTime = 0.0f;

		// テンポデータ分計算する
		PiarhythmDatas.TempoData prevTempData = new PiarhythmDatas.TempoData();
		prevTempData.tempo = 0;
		foreach (PiarhythmDatas.TempoData tempData in m_tempoDataList)
		{
			if (prevTempData.tempo != 0)
			{
				// 一拍当たりの時間を求める
				float beatPerTempo = 60.0f / prevTempData.tempo;

				for (int i = prevTempData.startMeasure; i < tempData.startMeasure; ++i)
				{
					// 開始時間を保存する
					float startTime = m_wholeTime;

					// 1小節分加算する
					m_wholeTime += beatPerTempo * 4.0f;

					// 背景を生成する
					m_musicalScoreController.CreateMusicScoreBackGround(startTime, m_wholeTime);
				}
			}

			prevTempData = tempData;
		}

		// 最後に残りの小節分加算する
		// 一拍当たりの時間を求める
		{
			float beatPerTempo = 60.0f / prevTempData.tempo;

			for (int i = prevTempData.startMeasure; i < m_wholeMeasure; ++i)
			{
				// 開始時間を保存する
				float startTime = m_wholeTime;

				// 1小節分加算する
				m_wholeTime += beatPerTempo * 4.0f;

				// 背景を生成する
				m_musicalScoreController.CreateMusicScoreBackGround(startTime, m_wholeTime);
			}
		}

		// 各UIへ反映させる
		m_musicalScoreController.ChangeScoreLength(m_wholeTime);
		m_menuController.UpdateDisplayWholeTimeText(m_wholeTime);

		// スクロールバーのテクスチャを更新する
		m_notesEditScrollbarController.UpdateTexture(m_bgmSheetController.GetBGMData(), m_wholeTime);
	}
	#endregion

	#region テンポの追加ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   テンポの追加ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickAddTempoButton()
	{
		// テンポノードを作成する
		GameObject tempoNode = Instantiate(m_tempoNodePrefab);

		// コンテナに登録する
		RectTransform rectTransform = tempoNode.GetComponent<RectTransform>();
		rectTransform.SetParent(m_tempoNodeContent);

		// 親子関係を組んだことで変化した値を修正する
		rectTransform.localScale = Vector3.one;
		rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0.0f);

		// コントローラーを設定する
		TempoNodeController tempoNodeController = tempoNode.GetComponent<TempoNodeController>();
		tempoNodeController.SetOptionSheetController(this);

		// データを追加する
		m_tempoDataList.Add(tempoNodeController.GetTempoData());

		// 要素番号を設定する
		tempoNodeController.SetIndex(m_tempoDataList.Count - 1);

		//　全体の時間を更新する
		CalculateWholeTime();
	}
	#endregion

	#region 全小節数の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   全小節数の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditWholeMeasureInputField()
	{
		// 入力値を取得する
		int wholeMeasure = int.Parse(m_wholeMeasureInputField.text);

		// 入力された値が0以下だった場合
		if (wholeMeasure <= 0)
		{
			// 前回の値を使用する
			m_wholeMeasureInputField.text = m_wholeMeasure.ToString();

			// 処理を終了する
			return;
		}

		// 更新する
		m_wholeMeasure = wholeMeasure;

		// 全体の時間を更新する
		CalculateWholeTime();
	}
	#endregion

	#region リスト内にあるテンポデータを更新する
	//-----------------------------------------------------------------
	//! @summary   リスト内にあるテンポデータを更新する
	//!
	//! @parameter [tempoData] 更新するテンポデータ
	//! @parameter [index] 要素番号
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateTempoData(PiarhythmDatas.TempoData tempData, int index)
	{
		// データがリストの範囲外だった場合、処理を終了する
		if ((index < 0) || (index > m_tempoDataList.Count)) return;

		// データを更新する
		m_tempoDataList[index] = tempData;

		// 全体の時間の更新
		CalculateWholeTime();
	}
	#endregion

	#region リスト内から指定したテンポデータを削除する
	//-----------------------------------------------------------------
	//! @summary   リスト内から指定したテンポデータを削除する
	//!
	//! @parameter [tempoData] 削除するテンポデータ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void RemoveTempoData(PiarhythmDatas.TempoData tempData)
	{
		// データを探す
		int index = m_tempoDataList.IndexOf(tempData);

		// 見つからなかった場合は処理を終了する
		if (index < 0) return;

		// リストから削除する
		m_tempoDataList.Remove(tempData);

		// 全体の時間を更新する
		CalculateWholeTime();
	}
	#endregion

	#region BGMの読み込みがあった時の楽曲全体の長さの更新
	//-----------------------------------------------------------------
	//! @summary   BGMの読み込みがあった時の楽曲全体の長さの更新
	//!
	//! @parameter [time] BGMの時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetWholeTime(float time)
	{
		//m_wholeTime = time;
		//m_wholeTimeInputField.text = m_wholeTime.ToString();
	}
	#endregion

	#region 楽曲全体の時間の取得
	//-----------------------------------------------------------------
	//! @summary   楽曲全体の時間の取得
	//!
	//! @return    楽曲全体の時間
	//-----------------------------------------------------------------
	public float GetWholeTime()
	{
		return m_wholeTime;
	}
	#endregion

	#region 時間から全ての小節数を設定する
	//-----------------------------------------------------------------
	//! @summary   時間から全ての小節数を設定する
	//!
	//! @parameter [time] 時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetWholeMeasure(float time)
	{
		// 背景をリセットする
		m_musicalScoreController.ResetMusicScoreBackGround();

		// 時間を初期化
		m_wholeTime = 0.0f;

		// 全小節数の初期化
		m_wholeMeasure = 0;

		// テンポデータ分計算する
		PiarhythmDatas.TempoData prevTempData = new PiarhythmDatas.TempoData();
		prevTempData.tempo = 0;

		foreach (PiarhythmDatas.TempoData tempData in m_tempoDataList)
		{
			if (prevTempData.tempo != 0)
			{
				// 一拍当たりの時間を求める
				float beatPerTempo = 60.0f / prevTempData.tempo;

				for (int i = prevTempData.startMeasure; i < tempData.startMeasure; ++i)
				{
					// 開始時間を保存する
					float startTime = m_wholeTime;

					// 1小節分加算する
					m_wholeTime += beatPerTempo * 4.0f;
					m_wholeMeasure++;

					// 背景を生成する
					m_musicalScoreController.CreateMusicScoreBackGround(startTime, m_wholeTime);
				}
			}

			prevTempData = tempData;
		}

		// 最後に残りの小節分加算する
		// 一拍当たりの時間を求める
		{
			float beatPerTempo = 60.0f / prevTempData.tempo;

			while (m_wholeTime < time)
			{
				// 開始時間を保存する
				float startTime = m_wholeTime;

				// 1小節分加算する
				m_wholeTime += beatPerTempo * 4.0f;
				m_wholeMeasure++;

				// 背景を生成する
				m_musicalScoreController.CreateMusicScoreBackGround(startTime, m_wholeTime);
			}
		}

		// 各UIへ反映させる
		m_musicalScoreController.ChangeScoreLength(m_wholeTime);
		m_menuController.UpdateDisplayWholeTimeText(m_wholeTime);

		// スクロールバーのテクスチャを更新する
		m_notesEditScrollbarController.UpdateTexture(m_bgmSheetController.GetBGMData(), m_wholeTime);
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
		for (int i = 1; i < m_tempoDataList.Count; ++i)
		{
			if (startBeat >= m_tempoDataList[i].startMeasure * 4)
			{
				// 一拍当たりの時間を求める
				float beatPerTempo = 60.0f / tempoData.tempo;
				// 時間を座標に変換する
				float beatPosition = PiarhythmUtility.ConvertTimeToPosition(beatPerTempo, NotesManager.NOTES_SPEED);

				// テンポデータの終了座標を求める
				elapsedPosition += beatPosition * ((m_tempoDataList[i].startMeasure - tempoData.startMeasure) * 4);

				// 経過拍数を増やす
				elapsedBeat += (m_tempoDataList[i].startMeasure - tempoData.startMeasure) * 4;

				tempoData = m_tempoDataList[i];
			}
			else
			{
				break;
			}
		}

		{
			// 一拍当たりの時間を求める
			float beatPerTempo = 60.0f / tempoData.tempo;
			// 時間を座標に変換する
			float beatPosition = PiarhythmUtility.ConvertTimeToPosition(beatPerTempo, NotesManager.NOTES_SPEED);

			// テンポデータを元に座標と長さを決める
			positionData.position = elapsedPosition + (startBeat - elapsedBeat) * beatPosition;

			switch (noteLenght)
			{
				case 0:
					positionData.lenght = beatPosition * PiarhythmDatas.NoteTime.WHOLE_NOTE_SEMIBREVE;
					break;
				case 1:
					positionData.lenght = beatPosition * PiarhythmDatas.NoteTime.HALF_NOTE_MININ;
					break;
				case 2:
					positionData.lenght = beatPosition * PiarhythmDatas.NoteTime.QUARTER_NOTE_CROCHET;
					break;
				case 3:
					positionData.lenght = beatPosition * PiarhythmDatas.NoteTime.EIGHTH_NOTE_QUAVER;
					break;
				case 4:
					positionData.lenght = beatPosition * PiarhythmDatas.NoteTime.SIXTEENTH_NOTE_SEMIQUAVER;
					break;
				case 5:
					positionData.lenght = beatPosition * PiarhythmDatas.NoteTime.WHOLE_DOTTED_NOTE_SEMIBREVE;
					break;
				case 6:
					positionData.lenght = beatPosition * PiarhythmDatas.NoteTime.HALF_DOTTED_NOTE_MININ;
					break;
				case 7:
					positionData.lenght = beatPosition * PiarhythmDatas.NoteTime.QUARTER_DOTTED_NOTE_CROCHET;
					break;
				case 8:
					positionData.lenght = beatPosition * PiarhythmDatas.NoteTime.EIGHTH_DOTTED_NOTE_QUAVER;
					break;
			}
		}

		return positionData;
	}
	#endregion

	#region ノーツの開始座標と長さから開始拍と音符に変換する
	//-----------------------------------------------------------------
	//! @summary   ノーツの開始座標と長さから開始拍と音符に変換する
	//!
	//! @parameter [positionData] 座標データ
	//!
	//! @return    ノーツデータ
	//-----------------------------------------------------------------
	public PiarhythmDatas.NotesData ConvertToNotesData(PiarhythmDatas.PositionData positionData)
	{
		PiarhythmDatas.NotesData notesData = new PiarhythmDatas.NotesData();
		float elapsedBeat = 0.0f;
		float elapsedPosition = 0.0f;

		// 所属しているテンポデータを調べる
		PiarhythmDatas.TempoData tempoData = m_tempoDataList[0];
		for (int i = 1; i < m_tempoDataList.Count; ++i)
		{
			// 一拍当たりの時間を求める
			float beatPerTempo = 60.0f / tempoData.tempo;
			// 時間を座標に変換する
			float beatPosition = PiarhythmUtility.ConvertTimeToPosition(beatPerTempo, NotesManager.NOTES_SPEED);

			// テンポデータの終了座標を求める
			float endPosition = beatPosition * ((m_tempoDataList[i].startMeasure - tempoData.startMeasure) * 4) + elapsedPosition;

			if (positionData.position >= endPosition)
			{
				// 経過座標を更新する
				elapsedPosition = endPosition;

				// 経過拍数を増やす
				elapsedBeat += (m_tempoDataList[i].startMeasure - tempoData.startMeasure) * 4;

				// 現在のテンポデータを更新する
				tempoData = m_tempoDataList[i];
			}
			else break;
		}

		// 現在のテンポデータから正確な位置を確定させる
		{
			float beatPerTempo = 60.0f / tempoData.tempo;
			float beatPosition = PiarhythmUtility.ConvertTimeToPosition(beatPerTempo, NotesManager.NOTES_SPEED);

			// 残りの座標を求める
			float residualPosition = positionData.position - elapsedPosition;

			// 残りの拍数を求める
			float residualBeat = residualPosition / beatPosition;

			// 残りの拍数を0.25倍に丸める
			residualBeat = PiarhythmUtility.MRound(residualBeat, 0.25f);

			// 経過拍数に加算する
			elapsedBeat += residualBeat;

			// データを保存する
			notesData.startBeat = elapsedBeat;


			// 長さを求める
			float noteLength = positionData.lenght / beatPosition;

			// 0.25倍に丸める
			noteLength = PiarhythmUtility.MRound(noteLength, 0.25f);

			// 一番近い長さを元に音符を決める
			if (Mathf.Approximately(noteLength, PiarhythmDatas.NoteTime.WHOLE_NOTE_SEMIBREVE)) notesData.noteLength = 0;
			else if (Mathf.Approximately(noteLength, PiarhythmDatas.NoteTime.HALF_NOTE_MININ)) notesData.noteLength = 1;
			else if (Mathf.Approximately(noteLength, PiarhythmDatas.NoteTime.QUARTER_NOTE_CROCHET)) notesData.noteLength = 2;
			else if (Mathf.Approximately(noteLength, PiarhythmDatas.NoteTime.EIGHTH_NOTE_QUAVER)) notesData.noteLength = 3;
			else if (Mathf.Approximately(noteLength, PiarhythmDatas.NoteTime.SIXTEENTH_NOTE_SEMIQUAVER)) notesData.noteLength = 4;
			else if (Mathf.Approximately(noteLength, PiarhythmDatas.NoteTime.WHOLE_DOTTED_NOTE_SEMIBREVE)) notesData.noteLength = 5;
			else if (Mathf.Approximately(noteLength, PiarhythmDatas.NoteTime.HALF_DOTTED_NOTE_MININ)) notesData.noteLength = 6;
			else if (Mathf.Approximately(noteLength, PiarhythmDatas.NoteTime.QUARTER_DOTTED_NOTE_CROCHET)) notesData.noteLength = 7;
			else if (Mathf.Approximately(noteLength, PiarhythmDatas.NoteTime.EIGHTH_DOTTED_NOTE_QUAVER)) notesData.noteLength = 8;
			else notesData.noteLength = 2;
		}

		return notesData;
	}
	#endregion

	#region 設定データの取得
	//-----------------------------------------------------------------
	//! @summary   設定データの取得
	//!
	//! @return    設定データ
	//-----------------------------------------------------------------
	public PiarhythmDatas.OptionData GetOptionData()
	{
		// データをまとめる
		m_optionData.tempDatas = m_tempoDataList.ToArray();
		m_optionData.wholeMeasure = m_wholeMeasure;

		return m_optionData;
	}
	#endregion

	#region 設定データの設定
	//-----------------------------------------------------------------
	//! @summary   設定データの設定
	//!
	//! @parameter [optionData] 設定する設定データ
	//-----------------------------------------------------------------
	public void SetOptionData(PiarhythmDatas.OptionData optionData)
	{
		m_optionData = optionData;
	}
	#endregion
}
