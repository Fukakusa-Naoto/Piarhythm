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
	private GameObject m_keyboard = null;
	[SerializeField]
	private NotesSheetController m_notesSheetController = null;


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
		m_notesSheetController.DisplayNotes(m_selectNotes.GetComponent<EditNotesController>());
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
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void DestroyNotes()
	{
		if (m_selectNotes == null) Debug.Log("ノーツが選択されていません");

		// リストから外す
		m_notesList.Remove(m_selectNotes);

		// オブジェクトを削除する
		Destroy(m_selectNotes);

		// 選択されているノーツを設定し直す
		SetSelectNotes(null);
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

		// ノーツが選択されていない場合処理を終了する
		if (!m_selectNotes) return;

		// ノーツへ設定する
		m_selectNotes.GetComponent<EditNotesController>().SetNotesScale(scale);
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
		// ノーツが選択されていない場合処理を終了する
		if (!m_selectNotes) return;

		// ノーツへ設定する
		m_selectNotes.GetComponent<EditNotesController>().SetNotesStartTime(startTime);
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
	public void SetSelectNotesLengthTime(float lengthTime)
	{
		// ノーツが選択されていない場合処理を終了する
		if (!m_selectNotes) return;

		// ノーツへ設定する
		m_selectNotes.GetComponent<EditNotesController>().SetNotesLengthTime(lengthTime);
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
		// ノーツが選択されていない場合処理を終了する
		if (!m_selectNotes) return;

		// ノーツへ設定する
		m_selectNotes.GetComponent<EditNotesController>().SetNotesColor(color);
	}
	#endregion
}
