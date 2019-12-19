//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		NoteController.cs
//!
//! @summary	ノーツの制御に関するC#スクリプト
//!
//! @date		2019.12.18
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class NoteController : MonoBehaviour
{
	// <メンバ変数>
	private PiarhythmDatas.NoteData m_noteData = null;
	// キー情報が保存された連想配列
	private Dictionary<string, RectTransform> m_keyDictionary = null;

	// コンポーネント
	private RectTransform m_transform = null;
	private Image m_image = null;

	// コントローラー
	private MusicController m_musicController = null;


	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Start()
	{
		// コンポーネントの取得
		m_transform = GetComponent<RectTransform>();
		m_image = GetComponent<Image>();

		// 色の初期化
		// #の色を変化させる
		m_image.color = (m_noteData.m_scale.Contains("#"))
			? new UnityEngine.Color(m_noteData.m_color.r * PiarhythmDatas.SHARP_COLOR_PERCENTAGE, m_noteData.m_color.g * PiarhythmDatas.SHARP_COLOR_PERCENTAGE, m_noteData.m_color.b * PiarhythmDatas.SHARP_COLOR_PERCENTAGE, 1.0f)
			: new UnityEngine.Color(m_noteData.m_color.r, m_noteData.m_color.g, m_noteData.m_color.b, 1.0f);

		UnityEngine.Color color = new UnityEngine.Color(m_noteData.m_color.r, m_noteData.m_color.g, m_noteData.m_color.b, m_noteData.m_color.a);
		m_image.color = color;

		// スケールの初期化
		m_transform.localScale = Vector3.one;

		// 音階の設定
		// 座標を設定された音階の位置に移動させる
		m_transform.position = new Vector3(m_keyDictionary[m_noteData.m_scale].position.x, m_transform.position.y, m_transform.position.z);

		Vector3 localScale = m_transform.localScale;
		localScale.x = (m_noteData.m_scale.Contains("#")) ? 0.6f : 0.8f;
		m_transform.localScale = localScale;

		// 手前に持ってくる
		Vector3 position = m_transform.localPosition;
		position.z = 0.0f;
		m_transform.localPosition = position;

		// 開始時間と長さの初期化
		PiarhythmDatas.PositionData positionData = m_musicController.ConvertToPositionData(m_noteData.m_startBeat, m_noteData.m_noteLength);
		Vector2 offsetMin = m_transform.offsetMin;
		offsetMin.y = positionData.m_position;
		m_transform.offsetMin = offsetMin;
		Vector2 offsetMax = m_transform.offsetMax;
		offsetMax.y = offsetMin.y + positionData.m_lenght;
		m_transform.offsetMax = offsetMax;

		PiarhythmDatas.NoteData noteData = m_noteData.m_nextNoteData;
		while (noteData != null)
		{
			positionData = m_musicController.ConvertToPositionData(noteData.m_startBeat, noteData.m_noteLength);
			offsetMax = m_transform.offsetMax;
			offsetMax.y += positionData.m_lenght;
			m_transform.offsetMax = offsetMax;

			noteData = noteData.m_nextNoteData;
		}
	}
	#endregion

	#region ノーツデータを設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツデータを設定する
	//!
	//! @parameter [noteData] 設定するノーツデータ
	//-----------------------------------------------------------------
	public void SetNoteData(PiarhythmDatas.NoteData noteData)
	{
		m_noteData = noteData;
	}
	#endregion

	#region MusicControllerを設定する
	//-----------------------------------------------------------------
	//! @summary   MusicControllerを設定する
	//!
	//! @parameter [musicController] 設定するMusicController
	//-----------------------------------------------------------------
	public void SetMusicController(MusicController musicController)
	{
		m_musicController = musicController;
	}
	#endregion

	#region キー情報が保存された連想配列を設定する
	//-----------------------------------------------------------------
	//! @summary   キー情報が保存された連想配列を設定する
	//!
	//! @parameter [keyDictionary] 設定するキー情報が保存された連想配列
	//-----------------------------------------------------------------
	public void SetKeyDictionary(Dictionary<string, RectTransform> keyDictionary)
	{
		m_keyDictionary = keyDictionary;
	}
	#endregion
}
