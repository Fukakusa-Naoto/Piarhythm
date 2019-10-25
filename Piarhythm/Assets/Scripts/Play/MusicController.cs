//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		MusicController.cs
//!
//! @summary	ノーツスクロールに関するC#スクリプト
//!
//! @date		2019.10.04
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class MusicController : MonoBehaviour
{
	// <メンバ変数>
	private float m_speed = 0.0f;
	private RectTransform m_transform;
	public PlayManager m_playManager;
	public GameObject m_notePrefab;
	public GameObject m_keyboard;
	private Dictionary<string, float> m_keyPositionDictionary;


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
		m_transform = GetComponent<RectTransform>();

		m_speed = m_playManager.GetPlaySpeed();

		m_keyPositionDictionary = new Dictionary<string, float>();
		for (int i = 0; i < m_keyboard.transform.childCount; ++i)
		{
			// キーのローカル座標 + キーボードのローカル座標
			m_keyPositionDictionary[m_keyboard.transform.GetChild(i).name] =
				m_keyboard.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x
				+ m_keyboard.GetComponent<RectTransform>().localPosition.x;
		}


		// ノーツの生成
		Create(m_playManager.GetNoteDatas());
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
		// 譜面を流す
		float pos = (m_speed * -100.0f) * m_playManager.GetElapsedTime();
		m_transform.localPosition = new Vector3(0, pos, 0);
    }



	//-----------------------------------------------------------------
	//! @summary   ノーツの作成
	//!
	//! @parameter [notesDeta] 作成するノーツデータの配列
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Create(Datas.NotesData[] noteDatas)
	{
		foreach(Datas.NotesData n in noteDatas)
		{
			// オブジェクトを生成する
			GameObject newNote = Instantiate(m_notePrefab);


			// 子として設定する
			RectTransform rectTransform = newNote.GetComponent<RectTransform>();
			rectTransform.parent = GetComponent<RectTransform>();

			// 座標を設定する
			// 左下の座標
			Vector2 offsetMin = rectTransform.offsetMin;
			offsetMin.y = (m_speed * 100.0f) * n.startTime;
			rectTransform.offsetMin = offsetMin;
			// 右上の座標
			Vector2 offsetMax = rectTransform.offsetMax;
			offsetMax.y = (m_speed * 100.0f) * n.endTime;
			rectTransform.offsetMax = offsetMax;

			// 音階に合わせて配置
			rectTransform.localPosition = new Vector3(m_keyPositionDictionary[n.scale], rectTransform.localPosition.y, rectTransform.localPosition.z);

			// ノーツの色と幅を設定する
			if (n.scale.Contains("#"))
			{
				rectTransform.localScale = new Vector3(0.1f, 1.0f, 1.0f);
				newNote.GetComponent<Image>().color = new Color(64.0f / 256.0f, 103.0f / 256.0f, 38.0f / 256.0f);
			}
			else
			{
				rectTransform.localScale = new Vector3(0.2f, 1.0f, 1.0f);
				newNote.GetComponent<Image>().color = new Color(112.0f / 256.0f, 173.0f / 256.0f, 71.0f / 256.0f);
			}
		}
	}
}
