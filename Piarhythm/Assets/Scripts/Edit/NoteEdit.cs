using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteEdit : MonoBehaviour
{
	public NotesManager m_notesManager;
	private bool m_isMove;

	// Start is called before the first frame update
	void Start()
	{
		m_isMove = false;
		m_notesManager = GameObject.Find("NotesManager").GetComponent<NotesManager>();
	}

	// Update is called once per frame
	void Update()
	{
		Move();
	}

	private void Move()
	{
		if (Input.GetMouseButtonDown(0))
		{
			PointerEventData pointer = new PointerEventData(EventSystem.current);
			pointer.position = Input.mousePosition;
			List<RaycastResult> result = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointer, result);

			foreach (RaycastResult raycastResult in result)
			{
				if (gameObject.name == raycastResult.gameObject.name)
				{
					m_isMove = true;
				}
			}
		}
		else if (Input.GetMouseButtonUp(0)) m_isMove = false;

		if (m_isMove) GetComponent<RectTransform>().position = Input.mousePosition;
	}
}
