//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		NotesManager.cs
//!
//! @summary	ノーツの管理に関するC#スクリプト
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
[System.Serializable]
public struct NoteData
{
	// 音階
	public string scale;
	// 開始時間
	public float startTime;
	// 終了時間
	public float endTime;
}



public class NotesManager : MonoBehaviour
{
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
			NoteData nodeData = m_selectNote.GetComponent<NoteEdit>().GetNodeData();
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

		NoteData nodeData = m_selectNote.GetComponent<NoteEdit>().GetNodeData();
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

		NoteData nodeData = m_selectNote.GetComponent<NoteEdit>().GetNodeData();
		m_notesStart.GetComponent<InputField>().text = nodeData.startTime.ToString();
		m_notesStart.transform.GetChild(2).GetComponent<Text>().text = nodeData.startTime.ToString();
	}

}
