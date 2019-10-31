//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		EditNotesController.cs
//!
//! @summary	エディター用ノーツに関するC#スクリプト
//!
//! @date		2019.10.25
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class EditNotesController : MonoBehaviour
{
	// <メンバ定数>
	// #時の色の変化率
	private static readonly float SHARP_COLOR_PERCENTAGE = 0.7f;

	// <メンバ変数>
	// コンポーネント
	private RectTransform m_transform = null;
	private Image m_image = null;
	private RectTransform m_musicalScoreTransform = null;

	// キャンバス
	private Canvas m_canvas = null;
	// キー情報が保存された連想配列
	private Dictionary<string, RectTransform> m_keyDictionary = null;

	// マネージャー
	private NotesManager m_notesManager = null;

	// ノーツ情報
	private PiarhythmDatas.NotesData m_notesData;


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
		m_musicalScoreTransform = m_transform.parent.GetComponent<RectTransform>();

		// データの初期化
		m_notesData = new PiarhythmDatas.NotesData();

		// 色の初期化
		m_notesData.color = m_image.color = Color.green;

		// スケールの初期化
		m_transform.localScale = Vector3.one;

		// 座標の初期化
		MoveEditNotes("C4", 0.0f);


		// 作成されたノーツを選択状態にする
		m_notesManager.SetSelectNotes(gameObject);
	}
	#endregion

	#region ドラッグ時の移動処理
	//-----------------------------------------------------------------
	//! @summary   ドラッグ時の移動処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnMouseDrag()
	{
		// マウス座標の取得
		Vector2 localPoint = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(m_musicalScoreTransform, Input.mousePosition, m_canvas.worldCamera, out localPoint);

		// ノーツの移動
		MoveEditNotes(m_notesData.scale, localPoint.y);
	}
	#endregion

	#region ノーツが選択された時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツが選択された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnPointerDown()
	{
		// 選択されたことをNotesManagerに伝える
		m_notesManager.SetSelectNotes(gameObject);
	}
	#endregion

	#region ノーツの移動処理
	//-----------------------------------------------------------------
	//! @summary   ノーツの移動処理
	//!
	//! @parameter [scale] 音階
	//! @parameter [positionY] Y座標
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void MoveEditNotes(string scale, float positionY)
	{
		// ノーツの移動
		m_transform.localPosition = new Vector3(m_transform.localPosition.x, positionY);
		SetNotesScale(scale);

		// データの更新
		m_notesData.scale = scale;
	}
	#endregion

	#region ノーツ情報の取得
	//-----------------------------------------------------------------
	//! @summary   ノーツ情報の取得
	//!
	//! @return    ノーツ情報
	//-----------------------------------------------------------------
	public PiarhythmDatas.NotesData GetNotesData()
	{
		return m_notesData;
	}
	#endregion

	#region ノーツの音階を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの音階を設定する
	//!
	//! @parameter [scale] 設定する音階
	//-----------------------------------------------------------------
	public void SetNotesScale(string scale)
	{
		// データの更新
		m_notesData.scale = scale;

		// 座標を設定された音階の位置に移動させる
		m_transform.position = new Vector3(m_keyDictionary[scale].position.x, m_transform.position.y);

		// 幅を合わせる
		float width= m_keyDictionary[scale].sizeDelta.x
			* m_keyDictionary[scale].parent.GetComponent<RectTransform>().localScale.x;
		m_transform.sizeDelta = new Vector2(width, m_transform.sizeDelta.y);

		// #の色を変化させる
		m_image.color = (scale.Contains("#"))
			? new Color(m_notesData.color.r * SHARP_COLOR_PERCENTAGE, m_notesData.color.g * SHARP_COLOR_PERCENTAGE, m_notesData.color.b * SHARP_COLOR_PERCENTAGE, 1.0f)
			: m_notesData.color;
	}
	#endregion

	#region ノーツの開始時間を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの開始時間を設定する
	//!
	//! @parameter [startTime] 設定する開始時間
	//-----------------------------------------------------------------
	public void SetNotesStartTime(float startTime)
	{
		m_notesData.startTime = startTime;
	}
	#endregion

	#region ノーツの終了時間を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの終了時間を設定する
	//!
	//! @parameter [endTime] 設定する終了時間
	//-----------------------------------------------------------------
	public void SetNotesEndTime(float endTime)
	{
		m_notesData.endTime = endTime;
	}
	#endregion

	#region ノーツの色を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの色を設定する
	//!
	//! @parameter [color] 設定する色
	//-----------------------------------------------------------------
	public void SetNotesColor(Color color)
	{
		m_notesData.color = color;
	}
	#endregion

	#region NotesManagerを設定する
	//-----------------------------------------------------------------
	//! @summary   NotesManagerを設定する
	//!
	//! @parameter [notesManager] 設定するNotesManager
	//-----------------------------------------------------------------
	public void SetNotesManager(NotesManager notesManager)
	{
		m_notesManager = notesManager;
	}
	#endregion

	#region キャンバスを設定する
	//-----------------------------------------------------------------
	//! @summary   キャンバスを設定する
	//!
	//! @parameter [canvas] 設定するキャンバス
	//-----------------------------------------------------------------
	public void SetCanvas(Canvas canvas)
	{
		m_canvas = canvas;
	}
	#endregion

	#region キー情報が保存された連想配列を設定する
	//-----------------------------------------------------------------
	//! @summary   キー情報が保存された連想配列を設定する
	//!
	//! @parameter [m_keyDictionary] 設定するキー情報が保存された連想配列
	//-----------------------------------------------------------------
	public void SetKeyDictionary(Dictionary<string,RectTransform> keyDictionary)
	{
		m_keyDictionary = keyDictionary;
	}
	#endregion
}
