//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		NotesManager.cs
//!
//! @summary	EditSceneにおけるノーツ全ての管理に関するC#スクリプト
//!
//! @date		2019.08.21
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class NotesManager : MonoBehaviour
{
	// <メンバ定数>
	public static readonly float NOTES_SPEED = 100.0f;


	// <メンバ変数>
	// 生成された全てのノーツリスト
	private List<GameObject> m_notesList = null;
	// 選択されているノーツ
	private List<GameObject> m_selectNotes = null;
	// 生成するノーツのPrefab
	public GameObject m_notesPrefab = null;
	// 連結ノーツのPrefab
	public GameObject m_connectNotePrefab = null;
	// 譜面オブジェクト
	[SerializeField]
	private GameObject m_musicalScore = null;
	// キーボード情報
	private Dictionary<string, RectTransform> m_keyDictionary = null;
	// 複数選択フラグ
	private bool m_multipleSelectFlag = false;
	// 全てのノーツデータを保存しておく配列
	private PiarhythmDatas.NoteData[] m_noteDatas = null;

	// UI
	[SerializeField]
	private Canvas m_canvas = null;
	[SerializeField]
	private GameObject m_keyboard = null;

	// コントローラー
	[SerializeField]
	private NotesSheetController m_notesSheetController = null;
	[SerializeField]
	private OptionSheetController m_optionSheetController = null;
	[SerializeField]
	private ConnectNoteSheetController m_connectNoteSheetController = null;


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
		m_notesList = new List<GameObject>();
		m_selectNotes = new List<GameObject>();

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

	#region 選択されているノーツを設定する
	//-----------------------------------------------------------------
	//! @summary   選択されているノーツを設定する
	//!
	//! @parameter [selectNotes] 設定するノーツ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetSelectNotes(GameObject selectNotes)
	{
		if (selectNotes != null)
		{
			// Ctrlキーが押されている
			if (Input.GetKey(KeyCode.LeftControl))
			{
				// 登録されているか調べる
				int index = m_selectNotes.IndexOf(selectNotes);

				// 登録されていない場合
				if (index < 0)
				{
					// 選択されているノーツを追加する
					m_selectNotes.Add(selectNotes);

					// 光彩を付ける
					selectNotes.GetComponent<EditNotesController>().SwitchGlow(true);
				}
				else
				{
					// 選択リストから外す
					m_selectNotes.RemoveAt(index);

					// 選択されたノーツの光彩を切る
					selectNotes.GetComponent<EditNotesController>().SwitchGlow(false);
				}
			}
			else
			{
				// 選択されている全ての光彩を切る
				if (m_selectNotes.Count == 0)
					foreach (GameObject note in m_selectNotes)
						note.GetComponent<EditNotesController>().SwitchGlow(false);

				// リストをクリアする
				m_selectNotes.Clear();

				// 選択されているノーツを追加する
				m_selectNotes.Add(selectNotes);

				// 光彩を付ける
				if (selectNotes.GetComponent<EditNotesController>()) selectNotes.GetComponent<EditNotesController>().SwitchGlow(true);
				else selectNotes.GetComponent<ConnectNoteController>().SwitchGlow(true);
			}

			// シートの切り替え
			if (m_selectNotes.Count > 1)
			{
				// 複数選択のフラグを立てる
				m_multipleSelectFlag = true;

				// 連結ノーツシートを手前に持ってくる
				m_connectNoteSheetController.SetAsLastSibling();
			}
			else
			{
				if (m_selectNotes.Count == 0)
				{
					// 複数選択のフラグを倒す
					m_multipleSelectFlag = false;

					// 連結ノーツシートを奥に持っていく
					m_connectNoteSheetController.SetAsFirstSibling();
				}
				else
				{
					if (m_selectNotes[0].GetComponent<EditNotesController>())
					{
						// 複数選択のフラグを倒す
						m_multipleSelectFlag = false;

						// 連結ノーツシートを奥に持っていく
						m_connectNoteSheetController.SetAsFirstSibling();
					}
					else
					{
						// 複数選択のフラグを立てる
						m_multipleSelectFlag = true;

						// 連結ノーツシートを手前に持ってくる
						m_connectNoteSheetController.SetAsLastSibling();
					}
				}
			}

			// UIへ情報を反映させる
			m_notesSheetController.DisplayNotes(selectNotes.GetComponent<EditNotesController>());
			m_connectNoteSheetController.DisplayNotes(selectNotes.GetComponent<ConnectNoteController>());
		}
		else
		{
			// 複数選択のフラグを倒す
			m_multipleSelectFlag = false;

			// 連結ノーツシートを奥に持っていく
			m_connectNoteSheetController.SetAsFirstSibling();

			// UIへ情報を反映させる
			m_notesSheetController.DisplayNotes(null);
			m_connectNoteSheetController.DisplayNotes(null);
		}
	}
	#endregion

	#region ノーツの生成処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの生成処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void CreateNotes()
	{
		// ノーツを生成する
		GameObject newNotes = Instantiate(m_notesPrefab);

		// コンポーネントの取得
		EditNotesController editNotes = newNotes.GetComponent<EditNotesController>();

		// マネージャーを設定する
		editNotes.SetNotesManager(this);
		// キャンバスの設定
		editNotes.SetCanvas(m_canvas);
		// キーボード情報
		editNotes.SetKeyDictionary(m_keyDictionary);
		// NotesSheetControllerを設定する
		editNotes.SetNotesSheetController(m_notesSheetController);
		// OptionSheetControllerを設定する
		editNotes.SetOptionSheetController(m_optionSheetController);

		// MusicalScoreの子に設定する
		newNotes.GetComponent<RectTransform>().SetParent(m_musicalScore.GetComponent<RectTransform>());

		// 初期化処理
		editNotes.Initialize();

		// リストに登録する
		m_notesList.Add(newNotes);

		// 生成されたノーツを選択中にする
		SetSelectNotes(newNotes);
	}
	#endregion

	#region 連結ノーツの生成処理
	//-----------------------------------------------------------------
	//! @summary   連結ノーツの生成処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void CreateConnectNote()
	{
		// 連結可能か調べる
		if (!CheckConnectNote()) return;

		// 連結ノーツを生成する
		GameObject connectNote = Instantiate(m_connectNotePrefab);

		// コンポーネントの取得
		ConnectNoteController connectNoteController = connectNote.GetComponent<ConnectNoteController>();

		// NoteManagerを設定する
		connectNoteController.SetNoteManager(this);
		// キャンバスの設定
		connectNoteController.SetCanvas(m_canvas);
		// キーボード情報
		connectNoteController.SetKeyDictionary(m_keyDictionary);
		// ConnectNoteSheetControllerを設定する
		connectNoteController.SetConnectNoteSheetController(m_connectNoteSheetController);
		// OptionSheetControllerを設定する
		connectNoteController.SetOptionSheetController(m_optionSheetController);

		// MusicalScoreの子に設定する
		connectNote.GetComponent<RectTransform>().SetParent(m_musicalScore.GetComponent<RectTransform>());

		// 連結に使用したノーツデータを登録する
		for (int i = 0; i < m_selectNotes.Count; ++i)
		{
			// データを取得する
			PiarhythmDatas.NoteData notesData = m_selectNotes[i].GetComponent<EditNotesController>().GetNotesData();

			// ノーツデータの連結先を更新する
			if (i - 1 > 0) notesData.m_prevNoteData = m_selectNotes[i - 1].GetComponent<EditNotesController>().GetNotesData();
			if (i + 1 < m_selectNotes.Count) notesData.m_nextNoteData = m_selectNotes[i + 1].GetComponent<EditNotesController>().GetNotesData();

			// 登録する
			connectNoteController.AddNoteData(notesData);

			// リストから削除する
			m_notesList.Remove(m_selectNotes[i]);

			// ノーツを削除する
			Destroy(m_selectNotes[i]);
		}

		// リストをクリアする
		m_selectNotes.Clear();

		// 初期化処理
		connectNoteController.Initialize();

		// リストに登録する
		m_notesList.Add(connectNote);

		// 生成されたノーツを選択中にする
		SetSelectNotes(connectNote);
	}
	#endregion

	#region ノーツの生成処理(引数あり)
	//-----------------------------------------------------------------
	//! @summary   ノーツの生成処理(引数あり)
	//!
	//! @parameter [notesDetas] ノーツデータの配列
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void CreateNotes(PiarhythmDatas.NoteData noteData)
	{
		// ノーツを生成する
		GameObject newNotes = Instantiate(m_notesPrefab);

		// コンポーネントの取得
		EditNotesController editNotes = newNotes.GetComponent<EditNotesController>();

		// マネージャーを設定する
		editNotes.SetNotesManager(this);
		// キャンバスの設定
		editNotes.SetCanvas(m_canvas);
		// キーボード情報
		editNotes.SetKeyDictionary(m_keyDictionary);
		// NotesSheetControllerを設定する
		editNotes.SetNotesSheetController(m_notesSheetController);
		// OptionSheetControllerを設定する
		editNotes.SetOptionSheetController(m_optionSheetController);

		// MusicalScoreの子に設定する
		newNotes.GetComponent<RectTransform>().SetParent(m_musicalScore.GetComponent<RectTransform>());

		// 初期化処理
		editNotes.Initialize();

		// ノーツデータを設定する
		editNotes.SetNotesData(noteData);

		// リストに登録する
		m_notesList.Add(newNotes);
	}
	#endregion

	#region 連結ノーツの生成処理(引数あり)
	//-----------------------------------------------------------------
	//! @summary   連結ノーツの生成処理(引数あり)
	//!
	//! @parameter [noteData] 連結させるノーツデータ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void CreateConnectNote(PiarhythmDatas.NoteData noteData)
	{
		// 連結ノーツを生成する
		GameObject connectNote = Instantiate(m_connectNotePrefab);

		// コンポーネントの取得
		ConnectNoteController connectNoteController = connectNote.GetComponent<ConnectNoteController>();

		// NoteManagerを設定する
		connectNoteController.SetNoteManager(this);
		// キャンバスの設定
		connectNoteController.SetCanvas(m_canvas);
		// キーボード情報
		connectNoteController.SetKeyDictionary(m_keyDictionary);
		// OptionSheetControllerを設定する
		connectNoteController.SetOptionSheetController(m_optionSheetController);

		// MusicalScoreの子に設定する
		connectNote.GetComponent<RectTransform>().SetParent(m_musicalScore.GetComponent<RectTransform>());

		// 連結に使用したノーツデータを登録する
		PiarhythmDatas.NoteData nextNoteData = noteData;
		while (nextNoteData != null)
		{
			connectNoteController.AddNoteData(nextNoteData);
			nextNoteData = nextNoteData.m_nextNoteData;
		}

		// 初期化処理
		connectNoteController.Initialize();

		// リストに登録する
		m_notesList.Add(connectNote);

		// 生成されたノーツを選択中にする
		SetSelectNotes(connectNote);
	}
	#endregion

	#region ノーツの削除処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの削除処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void DestroyNotes()
	{
		foreach (GameObject note in m_selectNotes)
		{
			// リストから外す
			m_notesList.Remove(note);

			// オブジェクトを削除する
			Destroy(note);
		}

		// リストをクリアする
		m_selectNotes.Clear();

		// 選択されているノーツを設定し直す
		SetSelectNotes(null);
	}
	#endregion

	#region ノーツの連結を解除
	//-----------------------------------------------------------------
	//! @summary   ノーツの連結を解除
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void CuttingNote()
	{
		// 選択されているノーツが複数だった場合、処理を終了する
		if (m_selectNotes.Count != 1) return;

		// 選択されているノーツが連結ノーツでなければ処理を終了する
		ConnectNoteController connectNoteController = m_selectNotes[0].GetComponent<ConnectNoteController>();
		if (connectNoteController == null) return;

		// データを取得する
		PiarhythmDatas.NoteData noteData = connectNoteController.GetNoteData();

		while (noteData != null)
		{
			// 次のノーツを保存しておく
			PiarhythmDatas.NoteData nextNoteData = noteData.m_nextNoteData;

			// 連結を切る
			noteData.m_prevNoteData = noteData.m_nextNoteData = null;

			// ノーツを生成する
			CreateNotes(noteData);

			// ノーツの更新をする
			noteData = nextNoteData;
		}

		// リストから削除する
		m_notesList.Remove(m_selectNotes[0]);

		// 削除する
		Destroy(m_selectNotes[0]);

		// リストをクリアする
		m_selectNotes.Clear();

		// UIの表示を更新する
		SetSelectNotes(null);
	}
	#endregion

	#region 連結可能か調べる
	//-----------------------------------------------------------------
	//! @summary   連結可能か調べる
	//!
	//! @parameter [void] なし
	//!
	//! @return    true :連結可能
	//! @return    false:連結不可
	//-----------------------------------------------------------------
	private bool CheckConnectNote()
	{
		PiarhythmDatas.NoteData notesData = m_selectNotes[0].GetComponent<EditNotesController>().GetNotesData();
		string scale = notesData.m_scale;
		PiarhythmDatas.Color color = notesData.m_color;
		float nextStart = notesData.m_startBeat;
		switch (notesData.m_noteLength)
		{
			case 0:
				nextStart += PiarhythmDatas.NoteTime.WHOLE_NOTE_SEMIBREVE;
				break;
			case 1:
				nextStart += PiarhythmDatas.NoteTime.HALF_NOTE_MININ;
				break;
			case 2:
				nextStart += PiarhythmDatas.NoteTime.QUARTER_NOTE_CROCHET;
				break;
			case 3:
				nextStart += PiarhythmDatas.NoteTime.EIGHTH_NOTE_QUAVER;
				break;
			case 4:
				nextStart += PiarhythmDatas.NoteTime.SIXTEENTH_NOTE_SEMIQUAVER;
				break;
			case 5:
				nextStart += PiarhythmDatas.NoteTime.WHOLE_DOTTED_NOTE_SEMIBREVE;
				break;
			case 6:
				nextStart += PiarhythmDatas.NoteTime.HALF_DOTTED_NOTE_MININ;
				break;
			case 7:
				nextStart += PiarhythmDatas.NoteTime.QUARTER_DOTTED_NOTE_CROCHET;
				break;
			case 8:
				nextStart += PiarhythmDatas.NoteTime.EIGHTH_DOTTED_NOTE_QUAVER;
				break;
		}

		for (int i = 1; i < m_selectNotes.Count; ++i)
		{
			// データを取得する
			notesData = m_selectNotes[i].GetComponent<EditNotesController>().GetNotesData();

			// 音階が一致するか調べる
			if (notesData.m_scale != scale) return false;

			// 次の開始位置を調べる
			if (!Mathf.Approximately(notesData.m_startBeat, nextStart)) return false;

			// 色を調べる
			if ((!Mathf.Approximately(notesData.m_color.r, color.r))
				&& (!Mathf.Approximately(notesData.m_color.g, color.g))
				&& (!Mathf.Approximately(notesData.m_color.b, color.b))) return false;

			// データを更新する
			switch (notesData.m_noteLength)
			{
				case 0:
					nextStart += PiarhythmDatas.NoteTime.WHOLE_NOTE_SEMIBREVE;
					break;
				case 1:
					nextStart += PiarhythmDatas.NoteTime.HALF_NOTE_MININ;
					break;
				case 2:
					nextStart += PiarhythmDatas.NoteTime.QUARTER_NOTE_CROCHET;
					break;
				case 3:
					nextStart += PiarhythmDatas.NoteTime.EIGHTH_NOTE_QUAVER;
					break;
				case 4:
					nextStart += PiarhythmDatas.NoteTime.SIXTEENTH_NOTE_SEMIQUAVER;
					break;
				case 5:
					nextStart += PiarhythmDatas.NoteTime.WHOLE_DOTTED_NOTE_SEMIBREVE;
					break;
				case 6:
					nextStart += PiarhythmDatas.NoteTime.HALF_DOTTED_NOTE_MININ;
					break;
				case 7:
					nextStart += PiarhythmDatas.NoteTime.QUARTER_DOTTED_NOTE_CROCHET;
					break;
				case 8:
					nextStart += PiarhythmDatas.NoteTime.EIGHTH_DOTTED_NOTE_QUAVER;
					break;
			}
		}

		return true;
	}
	#endregion

	#region 楽曲再生中の更新処理
	//-----------------------------------------------------------------
	//! @summary   楽曲再生中の更新処理
	//!
	//! @parameter [elapsedTime] 経過時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateAllEditNotes(float elapsedTime)
	{
		foreach(GameObject notes in m_notesList)
		{
			notes.GetComponent<EditNotesController>().UpdateEditNotes(elapsedTime);
		}
	}
	#endregion

	#region 再生瞬間のノーツの処理
	//-----------------------------------------------------------------
	//! @summary   再生瞬間のノーツの処理
	//!
	//! @parameter [elapsedTime] 経過時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void PlayMomentEditNotes(float elapsedTime)
	{
		foreach (GameObject notes in m_notesList)
		{
			// コンポーネントの取得
			EditNotesController editNotesController = notes.GetComponent<EditNotesController>();

			// データの取得
			PiarhythmDatas.NoteData notesData = editNotesController.GetNotesData();

			// 経過時間が既にノーツの開始時間を過ぎている
			if (elapsedTime >= notesData.m_startBeat)
			{
				// 音をならないようにする
				editNotesController.SetPlayedFlag(true);
			}
		}
	}
	#endregion

	#region 停止瞬間のノーツの処理
	//-----------------------------------------------------------------
	//! @summary   停止瞬間のノーツの処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void StopMomentEditNotes()
	{
		foreach (GameObject notes in m_notesList)
		{
			// コンポーネントの取得
			EditNotesController editNotesController = notes.GetComponent<EditNotesController>();

			// 音を復活させる
			editNotesController.SetPlayedFlag(false);
		}
	}
	#endregion

	#region 選択されているノーツに音階を設定する
	//-----------------------------------------------------------------
	//! @summary   選択されているノーツに音階を設定する
	//!
	//! @parameter [scale] 設定する音階
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetSelectNotesScale(string scale)
	{
		// 1つのノーツが選択されていなければ、処理を終了する
		if (m_selectNotes.Count != 1) return;

		// ノーツへ設定する
		if (m_selectNotes[0].GetComponent<EditNotesController>() != null)
		{
			// 通常のノーツの設定
			m_selectNotes[0].GetComponent<EditNotesController>().SetNotesScale(scale);
		}
		else
		{
			// 連結ノーツの設定
			m_selectNotes[0].GetComponent<ConnectNoteController>().SetNotesScale(scale);
		}
	}
	#endregion

	#region 選択されているノーツに開始時間を設定する
	//-----------------------------------------------------------------
	//! @summary   選択されているノーツに開始時間を設定する
	//!
	//! @parameter [startTime] 設定する開始時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetSelectNotesStartTime(float startTime)
	{
		// 1つのノーツが選択されていなければ、処理を終了する
		if (m_selectNotes.Count != 1) return;

		// ノーツへ設定する
		if (m_selectNotes[0].GetComponent<EditNotesController>() != null)
		{
			// 通常のノーツの設定
			m_selectNotes[0].GetComponent<EditNotesController>().SetNotesStartTime(startTime);
		}
		else
		{
			// 連結ノーツの設定
			m_selectNotes[0].GetComponent<ConnectNoteController>().SetStartBeat(startTime);
		}
	}
	#endregion

	#region 選択されているノーツに長さを設定する
	//-----------------------------------------------------------------
	//! @summary   選択されているノーツに長さを設定する
	//!
	//! @parameter [lengthTime] 設定する長さ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetSelectNotesLengthTime(int lengthTime)
	{
		// 1つのノーツが選択されていなければ、処理を終了する
		if (m_selectNotes.Count != 1) return;

		// ノーツへ設定する
		m_selectNotes[0].GetComponent<EditNotesController>().SetNotesLengthTime(lengthTime);
	}
	#endregion

	#region 選択されているノーツに色を設定する
	//-----------------------------------------------------------------
	//! @summary   選択されているノーツに色を設定する
	//!
	//! @parameter [color] 設定する色
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetSelectNotesColor(PiarhythmDatas.Color color)
	{
		// 1つのノーツが選択されていなければ、処理を終了する
		if (m_selectNotes.Count != 1) return;

		// ノーツへ設定する
		if (m_selectNotes[0].GetComponent<EditNotesController>() != null)
		{
			// 通常のノーツの設定
			m_selectNotes[0].GetComponent<EditNotesController>().SetNotesColor(color);
		}
		else
		{
			// 連結ノーツの設定
			m_selectNotes[0].GetComponent<ConnectNoteController>().SetColor(color);
		}
	}
	#endregion

	#region 全てのノーツデータを取得する
	//-----------------------------------------------------------------
	//! @summary   全てのノーツデータを取得する
	//!
	//! @return    ノーツデータの配列
	//-----------------------------------------------------------------
	public PiarhythmDatas.NoteData[] GetNotesDatas()
	{
		m_noteDatas = new PiarhythmDatas.NoteData[m_notesList.Count];

		// ノーツデータをまとめる
		for (int i = 0; i < m_notesList.Count; ++i)
		{
			if(m_notesList[i].GetComponent<EditNotesController>()) m_noteDatas[i] = m_notesList[i].GetComponent<EditNotesController>().GetNotesData();
			else m_noteDatas[i] = m_notesList[i].GetComponent<ConnectNoteController>().GetNoteData();
		}

		return m_noteDatas;
	}
	#endregion

	#region 全てのノーツの音量を設定する
	//-----------------------------------------------------------------
	//! @summary   全てのノーツの音量を設定する
	//!
	//! @parameter [volume] 音量
	//-----------------------------------------------------------------
	public void SetAllNotesVolume(float volume)
	{
		foreach(GameObject notes in m_notesList)
		{
			notes.GetComponent<AudioSource>().volume = volume;
		}
	}
	#endregion

	#region 複数選択フラグを取得する
	//-----------------------------------------------------------------
	//! @summary   複数選択フラグを取得する
	//!
	//! @return    フラグの状態
	//-----------------------------------------------------------------
	public bool GetMultipleSelectFlag()
	{
		return m_multipleSelectFlag;
	}
	#endregion
}
