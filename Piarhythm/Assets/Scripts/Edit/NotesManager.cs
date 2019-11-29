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
	// 譜面オブジェクト
	[SerializeField]
	private GameObject m_musicalScore = null;
	// キーボード情報
	private Dictionary<string, RectTransform> m_keyDictionary = null;
	// 複数選択フラグ
	private bool m_multipleSelectFlag = false;

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
				foreach (GameObject note in m_selectNotes) note.GetComponent<EditNotesController>().SwitchGlow(false);

				// リストをクリアする
				m_selectNotes.Clear();

				// 選択されているノーツを追加する
				m_selectNotes.Add(selectNotes);

				// 光彩を付ける
				selectNotes.GetComponent<EditNotesController>().SwitchGlow(true);
			}

			// シートの切り替え
			if (m_selectNotes.Count > 1)
			{
				// 複数選択のフラグを立てる
				m_multipleSelectFlag = true;

				// 連結可能か調べる

				// 連結ノーツシートを手前に持ってくる
				m_connectNoteSheetController.SetAsLastSibling();
			}
			else
			{
				// 複数選択のフラグを倒す
				m_multipleSelectFlag = false;

				// 連結ノーツシートを奥に持っていく
				m_connectNoteSheetController.SetAsFirstSibling();
			}

			// UIへ情報を反映させる
			m_notesSheetController.DisplayNotes(selectNotes.GetComponent<EditNotesController>());
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
		if (m_notesPrefab == null) Debug.Log("NotesPrefabが設定されていません");
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
		if (m_musicalScore == null) Debug.Log("MusicalScoreが設定されていません");
		newNotes.GetComponent<RectTransform>().SetParent(m_musicalScore.GetComponent<RectTransform>());

		// 初期化処理
		editNotes.Initialize();

		// リストに登録する
		m_notesList.Add(newNotes);

		// 生成されたノーツを選択中にする
		SetSelectNotes(newNotes);
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
	public void CreateNotes(PiarhythmDatas.NotesData[] notesDatas)
	{
		if (m_notesPrefab == null) Debug.Log("NotesPrefabが設定されていません");

		foreach (PiarhythmDatas.NotesData notesData in notesDatas)
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
			editNotes.SetNotesData(notesData);

			// リストに登録する
			m_notesList.Add(newNotes);
		}
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
		if (m_selectNotes == null) Debug.Log("ノーツが選択されていません");

		foreach(GameObject note in m_selectNotes)
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
			PiarhythmDatas.NotesData notesData = editNotesController.GetNotesData();

			// 経過時間が既にノーツの開始時間を過ぎている
			if (elapsedTime >= notesData.startBeat)
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
		// UIへ反映する
		m_notesSheetController.UpdateMusicalScale(scale);

		// 1つのノーツが選択されていなければ、処理を終了する
		if (m_selectNotes.Count != 1) return;

		// ノーツへ設定する
		m_selectNotes[0].GetComponent<EditNotesController>().SetNotesScale(scale);
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
		m_selectNotes[0].GetComponent<EditNotesController>().SetNotesStartTime(startTime);
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
	public void SetSelectNotesColor(Color color)
	{
		// 1つのノーツが選択されていなければ、処理を終了する
		if (m_selectNotes.Count != 1) return;

		// ノーツへ設定する
		m_selectNotes[0].GetComponent<EditNotesController>().SetNotesColor(color);
	}
	#endregion

	#region 全てのノーツデータを取得する
	//-----------------------------------------------------------------
	//! @summary   全てのノーツデータを取得する
	//!
	//! @return    ノーツデータの配列
	//-----------------------------------------------------------------
	public PiarhythmDatas.NotesData[] GetNotesDatas()
	{
		PiarhythmDatas.NotesData[] notesDatas = new PiarhythmDatas.NotesData[m_notesList.Count];

		// ノーツデータをまとめる
		int i = 0;
		foreach(GameObject notes in m_notesList)
		{
			notesDatas[i]=notes.GetComponent<EditNotesController>().GetNotesData();
			++i;
		}

		return notesDatas;
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
