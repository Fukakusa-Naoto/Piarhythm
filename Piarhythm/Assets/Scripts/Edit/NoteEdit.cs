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
using UnityEngine.UI;


// クラスの定義 =============================================================
public class NoteEdit : MonoBehaviour
{
	// <メンバ変数>
	public NotesManager m_notesManager;
	private bool m_isMove;
	private Dictionary<string, float> m_keyPositionDictionary;
	private NoteData m_noteData;
	private EditManager m_editManager;


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
		m_editManager = GameObject.Find("EditManager").GetComponent<EditManager>();
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

		m_noteData.startTime = ConvertDistanceToTime(GetComponent<RectTransform>().offsetMin.y);
		m_noteData.endTime = ConvertDistanceToTime(GetComponent<RectTransform>().offsetMax.y);
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

					if(n.Key.Contains("#"))
					{
						GetComponent<RectTransform>().localScale = new Vector3(0.1f, 1.0f, 1.0f);
						GetComponent<Image>().color = new Color(64.0f / 256.0f, 103.0f / 256.0f, 38.0f / 256.0f);
					}
					else
					{
						GetComponent<RectTransform>().localScale = new Vector3(0.2f, 1.0f, 1.0f);
						GetComponent<Image>().color = new Color(112.0f / 256.0f, 173.0f / 256.0f, 71.0f / 256.0f);
					}
					break;
				}
				else
				{
					GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, localpoint.y, this.GetComponent<RectTransform>().localPosition.z);

					m_noteData.startTime = ConvertDistanceToTime(GetComponent<RectTransform>().offsetMin.y);
					m_noteData.endTime = ConvertDistanceToTime(GetComponent<RectTransform>().offsetMax.y);
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



	//-----------------------------------------------------------------
	//! @summary   ノーツの音階を設定する
	//!
	//! @parameter [scale] 音階の文字列
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetMusicScale(string scale)
	{
		m_noteData.scale = scale;
		GetComponent<RectTransform>().localPosition =
			new Vector3(m_keyPositionDictionary[scale], GetComponent<RectTransform>().localPosition.y, 0.0f);
		if (scale.Contains("#"))
		{
			GetComponent<RectTransform>().localScale = new Vector3(0.1f, 1.0f, 1.0f);
			GetComponent<Image>().color = new Color(64.0f / 256.0f, 103.0f / 256.0f, 38.0f / 256.0f);
		}
		else
		{
			GetComponent<RectTransform>().localScale = new Vector3(0.2f, 1.0f, 1.0f);
			GetComponent<Image>().color = new Color(112.0f / 256.0f, 173.0f / 256.0f, 71.0f / 256.0f);
		}
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの開始時間を設定する
	//!
	//! @parameter [startTime] 開始時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetStartTime(float startTime)
	{
		float newEndTime = m_noteData.endTime;

		// ノーツの開始時間が終了時間を超えた場合
		if (m_noteData.endTime < startTime)
		{
			// 新しい終了時間を計算する
			float difference = startTime - m_noteData.startTime;
			newEndTime = m_noteData.endTime + difference;

			// ノーツ全体を移動させる
			Vector2 offsetMin = GetComponent<RectTransform>().offsetMin;
			offsetMin.y = ConvertTimeToDistance(startTime);
			GetComponent<RectTransform>().offsetMin = offsetMin;

			Vector2 offsetMax = GetComponent<RectTransform>().offsetMax;
			offsetMax.y = ConvertTimeToDistance(newEndTime);
			GetComponent<RectTransform>().offsetMax = offsetMax;
		}
		else
		{
			// 開始位置だけを移動させる
			Vector2 offsetMin = GetComponent<RectTransform>().offsetMin;
			offsetMin.y = ConvertTimeToDistance(startTime);
			GetComponent<RectTransform>().offsetMin = offsetMin;
		}

		// データを更新する
		m_noteData.startTime = startTime;
		m_noteData.endTime = newEndTime;
	}



	//-----------------------------------------------------------------
	//! @summary   ノーツの終了時間を設定する
	//!
	//! @parameter [endTime] 終了時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetEndTime(float endTime)
	{
		float newStartTime = m_noteData.startTime;

		// ノーツの終了時間が開始時間より速かった場合
		if (m_noteData.startTime > endTime)
		{
			// 新しい開始時間を計算する
			float difference = m_noteData.endTime - endTime;
			newStartTime = m_noteData.startTime + difference;

			// ノーツ全体を移動させる
			Vector2 offsetMin = GetComponent<RectTransform>().offsetMin;
			offsetMin.y = ConvertTimeToDistance(newStartTime);
			GetComponent<RectTransform>().offsetMin = offsetMin;

			Vector2 offsetMax = GetComponent<RectTransform>().offsetMax;
			offsetMax.y = ConvertTimeToDistance(endTime);
			GetComponent<RectTransform>().offsetMax = offsetMax;
		}
		else
		{
			// 終了位置だけを移動させる
			Vector2 offsetMax = GetComponent<RectTransform>().offsetMax;
			offsetMax.y = ConvertTimeToDistance(endTime);
			GetComponent<RectTransform>().offsetMax = offsetMax;
		}

		// データを更新する
		m_noteData.startTime = newStartTime; ;
		m_noteData.endTime = endTime;
	}



	//-----------------------------------------------------------------
	//! @summary   距離から時間に変換する
	//!
	//! @parameter [distance] 距離
	//!
	//! @return    時間
	//-----------------------------------------------------------------
	private float ConvertDistanceToTime(float distance)
	{
		float fps = 60.0f;
		return distance / (m_editManager.GetNotesSpeed() * fps);
	}



	//-----------------------------------------------------------------
	//! @summary   時間から距離に変換する
	//!
	//! @parameter [time] 時間
	//!
	//! @return    距離
	//-----------------------------------------------------------------
	private float ConvertTimeToDistance(float time)
	{
		float fps = 60.0f;

		return time * (m_editManager.GetNotesSpeed() * fps);
	}



	// ゲッター関数の定義 ===================================================
	//-----------------------------------------------------------------
	//! @summary   ノードデータの取得
	//-----------------------------------------------------------------
	public NoteData GetNodeData() { return m_noteData; }
}
