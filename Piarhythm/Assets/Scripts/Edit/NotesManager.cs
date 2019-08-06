using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesManager : MonoBehaviour
{
	public GameObject m_musicalScale;
	public GameObject m_notesStart;
	public GameObject m_notesEnd;
	public GameObject m_color;
	public GameObject m_note;
	public GameObject m_notePrefab;
	public GameObject m_musicalScore;
	private List<GameObject> m_nodeList;


	private void Start()
	{
		m_nodeList = new List<GameObject>();
	}



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


	public void OnNewNoteButton()
	{
		m_note = Instantiate(m_notePrefab);
		m_note.transform.SetParent(m_musicalScore.transform);
		m_note.GetComponent<RectTransform>().localPosition =
			new Vector3(-24.0f, -119 - m_musicalScore.GetComponent<RectTransform>().localPosition.y, 0.0f);
		m_nodeList.Add(m_note);
	}
}
