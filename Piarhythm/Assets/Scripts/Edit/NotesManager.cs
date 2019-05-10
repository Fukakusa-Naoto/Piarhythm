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

	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
	}

	public void SelectNote(GameObject note)
	{
		if(note)
		{
			m_note = note;
		}
		else
		{
			m_musicalScale.transform.GetChild(0).GetComponent<Text>().text = "";
			m_notesStart.transform.GetChild(0).GetComponent<Text>().text = "0.0";
			m_notesEnd.transform.GetChild(0).GetComponent<Text>().text = "0.0";
		}
	}

	public void OnNewNoteButton()
	{
		m_note = Instantiate(m_notePrefab);
		m_note.transform.SetParent(m_musicalScore.transform);
		m_note.GetComponent<RectTransform>().localPosition = new Vector3(-29.2f, -210 - m_musicalScore.GetComponent<RectTransform>().localPosition.y, 0.0f);
	}
}
