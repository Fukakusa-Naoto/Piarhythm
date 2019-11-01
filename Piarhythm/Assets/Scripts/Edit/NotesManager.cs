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
	private GameObject m_lengthTimeInputField = null;
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
	public void DisplayNotes(GameObject displayNotes)
	{
		// UIへ情報を反映させる
		if (displayNotes == null)
		{
			m_musicalScaleInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = "None";
			m_startTimeInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = "None";
			m_lengthTimeInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = "None";
			m_colorDropdown.GetComponent<Dropdown>().value = 0;
		}
		else
		{
			// ノーツデータの取得
			PiarhythmDatas.NotesData notesData = displayNotes.GetComponent<EditNotesController>().GetNotesData();

			// 音階の更新
			m_musicalScaleInputField.GetComponent<InputField>().text =
			m_musicalScaleInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = notesData.scale;

			// 開始時間の更新
			m_startTimeInputField.GetComponent<InputField>().text =
			m_startTimeInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = notesData.startTime.ToString();

			// 終了時間の更新
			m_lengthTimeInputField.GetComponent<InputField>().text =
			m_lengthTimeInputField.GetComponent<RectTransform>().GetChild(1).GetComponent<Text>().text = notesData.length.ToString();

			// 色の更新
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

		// 何も入力がされていなければ処理を終了する
		if (inputField.text == "") return;

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

	#region ノーツの開始時間の入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの開始時間の入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnNotesStartTimeInputField()
	{
		// ノーツが選択されていない場合、処理を終了する
		if (!m_selectNotes) return;

		// コンポーネントの取得
		InputField inputField = m_startTimeInputField.GetComponent<InputField>();

		// 何も入力されていなければ処理を終了する
		if (inputField.text == "") return;

		// 選択されているノーツに設定する
		m_selectNotes.GetComponent<EditNotesController>().SetNotesStartTime(float.Parse(inputField.text));
	}
	#endregion

	#region ノーツの長さの入力があった時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの長さの入力があった時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnNotesLengthTimeInputField()
	{
		// ノーツが選択されていない場合、処理を終了する
		if (!m_selectNotes) return;

		// コンポーネントの取得
		InputField inputField = m_lengthTimeInputField.GetComponent<InputField>();

		// 何も入力されていなければ処理を終了する
		if (inputField.text == "") return;

		// 選択されているノーツに設定する
		m_selectNotes.GetComponent<EditNotesController>().SetNotesLengthTime(float.Parse(inputField.text));
	}
	#endregion
}
