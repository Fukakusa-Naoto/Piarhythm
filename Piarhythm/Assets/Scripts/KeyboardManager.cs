using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class NoteInfo
{
	public int note;
	public List<GameObject> elements;

	public NoteInfo(int note)
	{
		this.note = note;
		elements = new List<GameObject>();
	}
}


public class KeyboardManager : MonoBehaviour
{
	List<NoteInfo> noteInfoList;

	// 鍵盤
	public GameObject m_keyboard;
	private Dictionary<int, GameObject> m_keys;

	void Awake()
	{
		noteInfoList = new List<NoteInfo>();
		m_keys = new Dictionary<int, GameObject>();

		for(int i=0;i<m_keyboard.transform.childCount;++i)
		{
			m_keys[i + 21] = m_keyboard.transform.GetChild(i).gameObject;
		}
	}

	// Update is called once per frame
	void Update()
    {

    }


	// 押されている
	void OnNoteOn(MidiMessage midi)
	{
		int note = midi.data1;

		foreach (var ni in noteInfoList)
		{
			if (ni.note == note)
				return;
		}

		var noteInfo = new NoteInfo(note);

		Debug.Log(noteInfo.note);
		m_keys[noteInfo.note].GetComponent<KeyController>().Press();

		noteInfoList.Add(noteInfo);
	}


	// 離された
	void OnNoteOff(MidiMessage midi)
	{
		int note = midi.data1;

		NoteInfo niFound = null;
		foreach (var ni in noteInfoList)
		{
			if (ni.note == note)
			{
				niFound = ni;
				break;
			}
		}

		if (niFound != null)
		{
			m_keys[niFound.note].GetComponent<KeyController>().Release();

			foreach (var e in niFound.elements)
			{
				//e.GetComponent<Element>().StartShrink();
			}
			noteInfoList.Remove(niFound);
		}
	}
}
