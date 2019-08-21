using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteEdit : MonoBehaviour
{
	public NotesManager m_notesManager;
	private bool m_isMove;
	private float[] m_keyPositionArray;

	// Start is called before the first frame update
	void Start()
	{
		m_isMove = false;
		m_notesManager = GameObject.Find("NotesManager").GetComponent<NotesManager>();

		GameObject key = GameObject.Find("88Key");
		m_keyPositionArray = new float[key.transform.childCount];
		for (int i = 0; i < key.transform.childCount; ++i)
		{
			// キーのローカル座標 + キーボードのローカル座標
			m_keyPositionArray[i] =
				key.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x
				+ key.GetComponent<RectTransform>().localPosition.x;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(m_isMove)
		{
			Vector2 localpoint;
			Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
			RectTransform parentRect = transform.parent.GetComponent<RectTransform>();

			RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, canvas.worldCamera, out localpoint);

			foreach(float n in m_keyPositionArray)
			{
				if ((localpoint.x > n - 5.0f) && (localpoint.x < n + 5.0f))
				{
					localpoint.x = n;
					GetComponent<RectTransform>().localPosition = localpoint;
					break;
				}
				else
				{
					this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, localpoint.y, this.GetComponent<RectTransform>().localPosition.z);
				}
			}

			m_isMove = false;
		}
	}

	public void OnMove()
	{
		m_isMove = true;
	}



	public GameObject OnCollision()
	{
		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointer, result);

		foreach (RaycastResult raycastResult in result)
		{
			if (gameObject.GetInstanceID() == raycastResult.gameObject.GetInstanceID())
			{
				return gameObject;
			}
		}

		return null;
	}
}
