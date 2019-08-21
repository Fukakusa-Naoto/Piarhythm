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
public struct NodeData
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
	public GameObject m_note;
	public GameObject m_notePrefab;
	public GameObject m_musicalScore;
	private List<GameObject> m_nodeList;


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
		m_nodeList = new List<GameObject>();
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
			foreach (var n in m_nodeList)
			{
				m_note = n.GetComponent<NoteEdit>().OnCollision();
				if (m_note) break;
			}
		}
		else if (Input.GetMouseButtonUp(0)) m_note = null;

		if(Input.GetMouseButton(0))
		{
			// m_nodeのm_isMoveをtrueにする
			if (m_note) m_note.GetComponent<NoteEdit>().OnMove();
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
		m_note = Instantiate(m_notePrefab);
		m_note.transform.SetParent(m_musicalScore.transform);
		m_note.GetComponent<RectTransform>().localPosition =
			new Vector3(-24.0f, -119 - m_musicalScore.GetComponent<RectTransform>().localPosition.y, 0.0f);
		m_nodeList.Add(m_note);
	}
}
