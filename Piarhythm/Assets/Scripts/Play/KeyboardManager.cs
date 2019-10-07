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

		for(int i = 0, j = 0; i < m_keyboard.transform.childCount; ++i)
		{
			switch(i + 21)
			{
				case 22: m_keys[22] = m_keyboard.transform.GetChild(52).gameObject; break;
				case 25: m_keys[25] = m_keyboard.transform.GetChild(53).gameObject; break;
				case 27: m_keys[27] = m_keyboard.transform.GetChild(54).gameObject; break;
				case 30: m_keys[30] = m_keyboard.transform.GetChild(55).gameObject; break;
				case 32: m_keys[32] = m_keyboard.transform.GetChild(56).gameObject; break;
				case 34: m_keys[34] = m_keyboard.transform.GetChild(57).gameObject; break;
				case 37: m_keys[37] = m_keyboard.transform.GetChild(58).gameObject; break;
				case 39: m_keys[39] = m_keyboard.transform.GetChild(59).gameObject; break;
				case 42: m_keys[42] = m_keyboard.transform.GetChild(60).gameObject; break;
				case 44: m_keys[44] = m_keyboard.transform.GetChild(61).gameObject; break;
				case 46: m_keys[46] = m_keyboard.transform.GetChild(62).gameObject; break;
				case 49: m_keys[49] = m_keyboard.transform.GetChild(63).gameObject; break;
				case 51: m_keys[51] = m_keyboard.transform.GetChild(64).gameObject; break;
				case 54: m_keys[54] = m_keyboard.transform.GetChild(65).gameObject; break;
				case 56: m_keys[56] = m_keyboard.transform.GetChild(66).gameObject; break;
				case 58: m_keys[58] = m_keyboard.transform.GetChild(67).gameObject; break;
				case 61: m_keys[61] = m_keyboard.transform.GetChild(68).gameObject; break;
				case 63: m_keys[63] = m_keyboard.transform.GetChild(69).gameObject; break;
				case 66: m_keys[66] = m_keyboard.transform.GetChild(70).gameObject; break;
				case 68: m_keys[68] = m_keyboard.transform.GetChild(71).gameObject; break;
				case 70: m_keys[70] = m_keyboard.transform.GetChild(72).gameObject; break;
				case 73: m_keys[73] = m_keyboard.transform.GetChild(73).gameObject; break;
				case 75: m_keys[75] = m_keyboard.transform.GetChild(74).gameObject; break;
				case 78: m_keys[78] = m_keyboard.transform.GetChild(75).gameObject; break;
				case 80: m_keys[80] = m_keyboard.transform.GetChild(76).gameObject; break;
				case 82: m_keys[82] = m_keyboard.transform.GetChild(77).gameObject; break;
				case 85: m_keys[85] = m_keyboard.transform.GetChild(78).gameObject; break;
				case 87: m_keys[87] = m_keyboard.transform.GetChild(79).gameObject; break;
				case 90: m_keys[90] = m_keyboard.transform.GetChild(80).gameObject; break;
				case 92: m_keys[92] = m_keyboard.transform.GetChild(81).gameObject; break;
				case 94: m_keys[94] = m_keyboard.transform.GetChild(82).gameObject; break;
				case 97: m_keys[97] = m_keyboard.transform.GetChild(83).gameObject; break;
				case 99: m_keys[99] = m_keyboard.transform.GetChild(84).gameObject; break;
				case 102: m_keys[102] = m_keyboard.transform.GetChild(85).gameObject; break;
				case 104: m_keys[104] = m_keyboard.transform.GetChild(86).gameObject; break;
				case 106: m_keys[106] = m_keyboard.transform.GetChild(87).gameObject; break;
				default: m_keys[i + 21] = m_keyboard.transform.GetChild(j++).gameObject; break;
			}
		}
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
