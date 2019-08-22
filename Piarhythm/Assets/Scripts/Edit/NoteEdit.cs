//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		NoteEdit.cs
//!
//! @summary	ノーツに関するC#スクリプト
//!
//! @date		2019.08.21
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


// クラスの定義 =============================================================
public class NoteEdit : MonoBehaviour
{
	// <メンバ変数>
	public NotesManager m_notesManager;
	private bool m_isMove;
	private Dictionary<string, float> m_keyPositionDictionary;
	private NoteData m_noteData;


	// メンバ関数の定義 =====================================================
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Start()
	{
		m_noteData = new NoteData();
		m_noteData.scale = "C4";
		m_isMove = false;
		m_notesManager = GameObject.Find("NotesManager").GetComponent<NotesManager>();

		GameObject key = GameObject.Find("88Key");
		m_keyPositionDictionary = new Dictionary<string, float>();
		for (int i = 0; i < key.transform.childCount; ++i)
		{
			// キーのローカル座標 + キーボードのローカル座標
			m_keyPositionDictionary[key.transform.GetChild(i).name] =
				key.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x
				+ key.GetComponent<RectTransform>().localPosition.x;
		}

		GetComponent<RectTransform>().localPosition =
			new Vector3(m_keyPositionDictionary["C4"], -120 - transform.parent.GetComponent<RectTransform>().localPosition.y, 0.0f);

		m_noteData.startTime = GetComponent<RectTransform>().offsetMin.y;
		m_noteData.endTime = GetComponent<RectTransform>().offsetMax.y;
	}



	//-----------------------------------------------------------------
	//! @summary   更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Update()
	{
		if(m_isMove)
		{
			Vector2 localpoint;
			Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
			RectTransform parentRect = transform.parent.GetComponent<RectTransform>();

			RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, canvas.worldCamera, out localpoint);

			foreach(KeyValuePair<string, float> n in m_keyPositionDictionary)
			{
				if ((localpoint.x > n.Value - 5.0f) && (localpoint.x < n.Value + 5.0f))
				{
					localpoint.x = n.Value;
					GetComponent<RectTransform>().localPosition = localpoint;
					m_noteData.scale = n.Key;
					break;
				}
				else
				{
					GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, localpoint.y, this.GetComponent<RectTransform>().localPosition.z);
					m_noteData.startTime = GetComponent<RectTransform>().offsetMin.y;
					m_noteData.endTime = GetComponent<RectTransform>().offsetMax.y;
				}
			}

			m_isMove = false;
		}
	}



	//-----------------------------------------------------------------
	//! @summary   移動中のフラグを立てる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnMove()
	{
		m_isMove = true;
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツとマウスの判定処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
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



	// ゲッター関数の定義 ===================================================
	//-----------------------------------------------------------------
	//! @summary   ノードデータの取得
	//-----------------------------------------------------------------
	public NoteData GetNodeData() { return m_noteData; }
}
