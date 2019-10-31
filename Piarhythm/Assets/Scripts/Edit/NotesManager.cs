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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class NotesManager : MonoBehaviour
{
	// <メンバ定数>
	public static readonly float NOTES_SPEED = 10.0f;


	// <メンバ変数>
	// 生成された全てのノーツリスト
	private List<GameObject> m_notesList = null;
	// 選択されているノーツ
	private GameObject m_selectNotes = null;
	// 生成するノーツのPrefab
	public GameObject m_notesPrefab = null;
	// 譜面オブジェクト
	[SerializeField]
	private GameObject m_musicalScore = null;
	// キーボード情報
	private Dictionary<string, RectTransform> m_keyDictionary = null;

	// UI
	[SerializeField]
	private Canvas m_canvas = null;
	[SerializeField]
	private GameObject m_musicalScaleInputField = null;
	[SerializeField]
	private GameObject m_startTimeInputField = null;
	[SerializeField]
	private GameObject m_endTimeInputField = null;
	[SerializeField]
	private GameObject m_colorDropdown = null;
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
	private void Start()
	{
		m_notesList = new List<GameObject>();

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
		m_selectNotes = selectNotes;

		// UIへ情報を反映させる
		DisplayNotes(m_selectNotes);
	}
	#endregion

	#region ノーツの生成処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの生成処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    生成されたノーツ
	//-----------------------------------------------------------------
	private void CreateNotes()
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

		// MusicalScoreの子に設定する
		if (m_musicalScore == null) Debug.Log("MusicalScoreが設定されていません");
		newNotes.GetComponent<RectTransform>().SetParent(m_musicalScore.GetComponent<RectTransform>());

		// リストに登録する
		m_notesList.Add(newNotes);
	}
	#endregion

	#region ノーツの削除処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの削除処理
	//!
	//! @parameter [destroyNotes] 削除するノーツ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void DestroyNotes(GameObject destroyNotes)
	{
		if (destroyNotes == null) Debug.Log("ノーツが選択されていません");

		// リストから外す
		m_notesList.Remove(m_selectNotes);

		// オブジェクトを削除する
		Destroy(m_selectNotes);
	}
	#endregion

	#region UIへノーツ情報を反映させる
	//-----------------------------------------------------------------
	//! @summary   UIへノーツ情報を反映させる
	//!
	//! @parameter [displayNotes] 表示するノーツ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void DisplayNotes(GameObject displayNotes)
	{
		// UIへ情報を反映させる
		if (displayNotes == null)
		{
			m_musicalScaleInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = "None";
			m_startTimeInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = "None";
			m_endTimeInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = "None";
			m_colorDropdown.GetComponent<Dropdown>().value = 0;
		}
		else
		{
			// ノーツデータの取得
			PiarhythmDatas.NotesData notesData = displayNotes.GetComponent<EditNotesController>().GetNotesData();
			m_musicalScaleInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = notesData.scale;
			m_startTimeInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = notesData.startTime.ToString();
			m_endTimeInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = notesData.endTime.ToString();
			if(notesData.color == Color.red) m_colorDropdown.GetComponent<Dropdown>().value = 0;
			else if(notesData.color == Color.green) m_colorDropdown.GetComponent<Dropdown>().value = 1;
			else if(notesData.color == Color.blue) m_colorDropdown.GetComponent<Dropdown>().value = 2;
		}
	}
	#endregion

	#region 作成ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   作成ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickNotesCreateButton()
	{
		// ノーツの作成
		CreateNotes();
	}
	#endregion

	#region 削除ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   削除ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickNotesDestroyButton()
	{
		// 選択されているノーツの削除
		DestroyNotes(m_selectNotes);

		// 選択されているノーツを設定し直す
		SetSelectNotes(null);
	}
	#endregion

	#region 音階の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   音階の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnMusicalScaleInputField()
	{
		// ノーツが選択されていない場合処理を終了する
		if (!m_selectNotes) return;

		// コンポーネントの取得
		InputField inputField = m_musicalScaleInputField.GetComponent<InputField>();

		// 文字列を大文字、小文字の区別なくチェックする
		foreach(KeyValuePair<string, RectTransform> n in m_keyDictionary)
		{
			if(inputField.text.Equals(n.Key,StringComparison.OrdinalIgnoreCase))
			{
				// 文字列を大文字にする
				string scale = inputField.text.ToUpper();
				// 選択されているノーツに設定する
				m_selectNotes.GetComponent<EditNotesController>().SetNotesScale(scale);

				// 処理を終了する
				return;
			}
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
		// コンポーネントの取得
		InputField inputField = m_musicalScaleInputField.GetComponent<InputField>();

		// UIへ反映する
		inputField.text = scale;

		// ノーツが選択されていない場合処理を終了する
		if (!m_selectNotes) return;

		// ノーツへ設定する
		m_selectNotes.GetComponent<EditNotesController>().SetNotesScale(scale);
	}
	#endregion

	#region 色の選択がされた時の処理
	//-----------------------------------------------------------------
	//! @summary   色の選択がされた時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnColorDropdown()
	{
		// ノーツが選択されていない場合処理を終了する
		if (!m_selectNotes) return;

		// コンポーネントの取得
		Dropdown colorDropdown = m_colorDropdown.GetComponent<Dropdown>();

		// 入力値の応じて色を設定する
		switch(colorDropdown.value)
		{
			case 0:     // 赤
				m_selectNotes.GetComponent<EditNotesController>().SetNotesColor(Color.magenta);
				break;
			case 1:     // 緑
				m_selectNotes.GetComponent<EditNotesController>().SetNotesColor(Color.green);
				break;
			case 2:     // 青
				m_selectNotes.GetComponent<EditNotesController>().SetNotesColor(Color.cyan);
				break;
		}
	}
	#endregion

#if false
	// <メンバ変数>
	public GameObject m_musicalScale;
	public GameObject m_notesStart;
	public GameObject m_notesEnd;
	public GameObject m_color;
	public GameObject m_moveNote;
	public GameObject m_selectNote;
	public GameObject m_notePrefab;
	public GameObject m_musicalScore;
	private List<GameObject> m_noteList;


	// メンバ関数の定義 =====================================================
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Start()
	{
		m_noteList = new List<GameObject>();
		m_selectNote = null;
	}



	//-----------------------------------------------------------------
	//! @summary   更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Update()
	{
		// 左クリックされる
		if (Input.GetMouseButtonDown(0))
		{
			foreach (var n in m_noteList)
			{
				var note = n.GetComponent<NoteEdit>().OnCollision();
				if (note)
				{
					m_moveNote = m_selectNote = note;
					break;
				}
			}
		}
		else if (Input.GetMouseButtonUp(0)) m_moveNote = null;

		if(Input.GetMouseButton(0))
		{
			if (m_moveNote) m_moveNote.GetComponent<NoteEdit>().OnMove();
		}

		if (m_selectNote)
		{
			Datas.NotesData nodeData = m_selectNote.GetComponent<NoteEdit>().GetNodeData();
			m_musicalScale.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = nodeData.scale;
			m_notesStart.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = nodeData.startTime.ToString();
			m_notesEnd.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = nodeData.endTime.ToString();
		}
		else
		{
			m_musicalScale.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = "";
			m_notesStart.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = "";
			m_notesEnd.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = "";
		}
	}



	//-----------------------------------------------------------------
	//! @summary   新たにノードを作成する
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnNewNoteButton()
	{
		m_selectNote = Instantiate(m_notePrefab);
		m_selectNote.transform.SetParent(m_musicalScore.transform);
		m_noteList.Add(m_selectNote);
	}



	//-----------------------------------------------------------------
	//! @summary   音階の入力情報の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnMusicalScaleInputField()
	{
		InputField inputField = m_musicalScale.GetComponent<InputField>();
		if (m_selectNote) m_selectNote.GetComponent<NoteEdit>().SetMusicScale(inputField.text);
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの開始時間の入力情報の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnStartTimeInputField()
	{
		// 文字列を数値に変換
		InputField inputField = m_notesStart.GetComponent<InputField>();
		if (m_selectNote) m_selectNote.GetComponent<NoteEdit>().SetStartTime(float.Parse(inputField.text));

		Datas.NotesData nodeData = m_selectNote.GetComponent<NoteEdit>().GetNodeData();
		m_notesEnd.GetComponent<InputField>().text = nodeData.endTime.ToString();
		m_notesEnd.transform.GetChild(2).GetComponent<Text>().text = nodeData.endTime.ToString();
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの終了時間の入力情報の取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndTimeInputField()
	{
		// 文字列を数値に変換
		InputField inputField = m_notesEnd.GetComponent<InputField>();
		if (m_selectNote) m_selectNote.GetComponent<NoteEdit>().SetEndTime(float.Parse(inputField.text));

		Datas.NotesData nodeData = m_selectNote.GetComponent<NoteEdit>().GetNodeData();
		m_notesStart.GetComponent<InputField>().text = nodeData.startTime.ToString();
		m_notesStart.transform.GetChild(2).GetComponent<Text>().text = nodeData.startTime.ToString();
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツデータの取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public Datas.NotesData[] GetNotesDatas()
	{
		Datas.NotesData[] notesDataList = new Datas.NotesData[m_noteList.Count];
		for(int i = 0; i < m_noteList.Count; ++i)
		{
			notesDataList[i] = m_noteList[i].GetComponent<NoteEdit>().GetNodeData();
		}
		return notesDataList;
	}
#endif
}
