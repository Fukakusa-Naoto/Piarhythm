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
	// 光彩の最小サイズ
	private static readonly float MIN_GLOW_SIZE = 2.0f;
	// 光彩の最大サイズ
	private static readonly float MAX_GLOW_SIZE = 10.0f;


	// <メンバ変数>
	// キャンバス
	private Canvas m_canvas = null;
	// キー情報が保存された連想配列
	private Dictionary<string, RectTransform> m_keyDictionary = null;
	// 音を鳴らしたか判定するためのフラグ
	private bool m_playedFlag = false;

	// コンポーネント
	private RectTransform m_transform = null;
	private GlowImage m_glowImage = null;
	private RectTransform m_musicalScoreTransform = null;
	private AudioSource m_audioSource = null;


	// マネージャー
	private NotesManager m_notesManager = null;

	// ノーツ情報
	private PiarhythmDatas.NotesData m_notesData;

	// コントローラー
	private NotesSheetController m_notesSheetController = null;
	private OptionSheetController m_optionSheetController = null;


	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void Initialize()
	{
		// コンポーネントの取得
		m_transform = GetComponent<RectTransform>();
		m_glowImage = GetComponent<GlowImage>();
		m_musicalScoreTransform = m_transform.parent.GetComponent<RectTransform>();
		m_audioSource = GetComponent<AudioSource>();

		// データの初期化
		m_notesData = new PiarhythmDatas.NotesData();

		// 色の初期化
		m_notesData.color = m_glowImage.color = m_glowImage.glowColor = Color.green;

		// スケールの初期化
		m_transform.localScale = Vector3.one;

		// 音階の設定
		SetNotesScale("C4");

		// 手前に持ってくる
		Vector3 position = m_transform.localPosition;
		position.z = 0.0f;
		m_transform.localPosition = position;

		// 開始時間と長さの初期化
		PiarhythmDatas.PositionData positionData = new PiarhythmDatas.PositionData();
		positionData.position = m_transform.offsetMin.y;
		positionData.lenght = m_transform.sizeDelta.y;
		PiarhythmDatas.NotesData notesData = m_optionSheetController.ConvertToNotesData(positionData);
		m_notesData.startBeat = notesData.startBeat;
		m_notesData.noteLength = 2;
		positionData = m_optionSheetController.ConvertToPositionData(m_notesData.startBeat, m_notesData.noteLength);
		m_transform.offsetMin = new Vector2(m_transform.offsetMin.x, positionData.position);
		m_transform.offsetMax = new Vector2(m_transform.offsetMax.x, m_transform.offsetMin.y + positionData.lenght);

		// 光彩を切る
		m_glowImage.glowSize = 0.0f;

		// 連結情報を初期化する
		m_notesData.connectElement = -1;
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

		float minDistance = float.MaxValue;
		string scale = m_notesData.scale;
		// ワールド座標のマウス座標を取得する
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 10.0f;
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

		// 最も近い音階を調べる
		foreach(KeyValuePair<string,RectTransform> n in m_keyDictionary)
		{
			// コンポーネントの取得
			RectTransform keyTransform = n.Value;

			// 距離を求める
			float distance = Mathf.Abs(worldPosition.x - keyTransform.position.x);

			// 最も近いキーを調べる
			if (minDistance >= distance)
			{
				// 最短距離を更新
				minDistance = distance;
				// 音階を更新する
				scale = n.Key;
			}
		}

		// ノーツの移動
		MoveEditNotes(scale, localPoint.y);

		// ノーツ情報をUIへ反映させる
		m_notesSheetController.DisplayNotes(this);
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

		// 光彩を起動する
		float glowSize = MAX_GLOW_SIZE - (m_transform.sizeDelta.y * 0.1f);
		glowSize = Mathf.Clamp(glowSize, MIN_GLOW_SIZE, MAX_GLOW_SIZE);
		m_glowImage.glowSize = glowSize;
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
		m_transform.localPosition = new Vector3(m_transform.localPosition.x, positionY, 0.0f);
		SetNotesScale(scale);

		// 移動制限
		Vector2 offsetMin = m_transform.offsetMin;
		if (offsetMin.y <= 0.0) offsetMin.y = 0.0f;
		m_transform.offsetMin = offsetMin;

		// 最新データの作成
		PiarhythmDatas.PositionData positionData = new PiarhythmDatas.PositionData();
		positionData.position = m_transform.offsetMin.y;
		positionData.lenght = m_transform.sizeDelta.y;

		// データの更新
		PiarhythmDatas.NotesData notesData = m_optionSheetController.ConvertToNotesData(positionData);
		m_notesData.startBeat = notesData.startBeat;
		m_notesData.scale = scale;

		// 位置調整
		positionData = m_optionSheetController.ConvertToPositionData(m_notesData.startBeat, m_notesData.noteLength);
		m_transform.offsetMin = new Vector2(m_transform.offsetMin.x, positionData.position);
		m_transform.offsetMax = new Vector2(m_transform.offsetMax.x, m_transform.offsetMin.y + positionData.lenght);
	}
	#endregion

	#region 楽曲再生中の更新処理
	//-----------------------------------------------------------------
	//! @summary   楽曲再生中の更新処理
	//!
	//! @parameter [elapsedTime] 経過時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateEditNotes(float elapsedTime)
	{
		// まだ音を鳴らしていない
		if(!m_playedFlag)
		{
			// 経過時間がノーツの開始時間を過ぎた
			if (m_notesData.startBeat <= elapsedTime)
			{
				// 音を鳴らす
				m_audioSource.Play();

				// 音を鳴らしたからフラグを立てる
				m_playedFlag = true;
			}
		}
	}
	#endregion

	#region 光彩のOn/Offの設定処理
	//-----------------------------------------------------------------
	//! @summary   光彩のOn/Offの設定処理
	//!
	//! @parameter [flag] 光彩のOn/Offのフラグ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SwitchGlow(bool flag)
	{
		if(flag)
		{
			// 光彩を起動する
			float glowSize = MAX_GLOW_SIZE - (m_transform.sizeDelta.y * 0.1f);
			glowSize = Mathf.Clamp(glowSize, MIN_GLOW_SIZE, MAX_GLOW_SIZE);
			m_glowImage.glowSize = glowSize;
		}
		else
		{
			m_glowImage.glowSize = 0.0f;
		}
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
		m_transform.position = new Vector3(m_keyDictionary[scale].position.x, m_transform.position.y, m_transform.position.z);

		// 音を設定する
		m_audioSource.clip = m_keyDictionary[scale].GetComponent<AudioSource>().clip;

		// 幅を合わせる
		// GlowImageの解析と改造が終わるまで下の処理で代用する
		//float width = m_keyDictionary[scale].sizeDelta.x
		//	* m_keyDictionary[scale].parent.GetComponent<RectTransform>().localScale.x;
		//m_transform.sizeDelta = new Vector2(width, m_transform.sizeDelta.y);

		Vector3 localScale = m_transform.localScale;
		localScale.x = (scale.Contains("#")) ? 0.6f : 0.8f;
		m_transform.localScale = localScale;

		// #の色を変化させる
		m_glowImage.color = (scale.Contains("#"))
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
		// 開始位置がマイナスだった場合は処理を終了する
		if (startTime < 0.0f) return;

		// データを更新する
		m_notesData.startBeat = PiarhythmUtility.MRound(startTime, 0.25f);

		// 位置の更新
		PiarhythmDatas.PositionData positionData = m_optionSheetController.ConvertToPositionData(m_notesData.startBeat, m_notesData.noteLength);
		m_transform.offsetMin = new Vector2(m_transform.offsetMin.x, positionData.position);
		m_transform.offsetMax = new Vector2(m_transform.offsetMax.x, m_transform.offsetMin.y + positionData.lenght);

		// UIを更新
		m_notesSheetController.DisplayNotes(this);
	}
	#endregion

	#region ノーツの長さを設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの長さを設定する
	//!
	//! @parameter [lengthTime] 設定する長さ
	//-----------------------------------------------------------------
	public void SetNotesLengthTime(int  lengthTime)
	{
		// データを更新する
		m_notesData.noteLength = lengthTime;

		// 長さの更新
		PiarhythmDatas.PositionData positionData = m_optionSheetController.ConvertToPositionData(m_notesData.startBeat, m_notesData.noteLength);
		m_transform.offsetMin = new Vector2(m_transform.offsetMin.x, positionData.position);
		m_transform.offsetMax = new Vector2(m_transform.offsetMax.x, m_transform.offsetMin.y + positionData.lenght);
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
		// 情報を更新する
		m_notesData.color = color;

		// 色を反映させる
		// #の色を変化させる
		m_glowImage.color = (m_notesData.scale.Contains("#"))
			? new Color(m_notesData.color.r * SHARP_COLOR_PERCENTAGE, m_notesData.color.g * SHARP_COLOR_PERCENTAGE, m_notesData.color.b * SHARP_COLOR_PERCENTAGE, 1.0f)
			: m_notesData.color;

		// 光彩の色を更新する
		m_glowImage.glowColor = color;
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
	//! @parameter [keyDictionary] 設定するキー情報が保存された連想配列
	//-----------------------------------------------------------------
	public void SetKeyDictionary(Dictionary<string,RectTransform> keyDictionary)
	{
		m_keyDictionary = keyDictionary;
	}
	#endregion

	#region NotesSheetControllerを設定する
	//-----------------------------------------------------------------
	//! @summary   NotesSheetControllerを設定する
	//!
	//! @parameter [notesSheetController] 設定するNotesSheetController
	//-----------------------------------------------------------------
	public void SetNotesSheetController(NotesSheetController notesSheetController)
	{
		m_notesSheetController = notesSheetController;
	}
	#endregion

	#region OptionSheetControllerを設定する
	//-----------------------------------------------------------------
	//! @summary   OptionSheetControllerを設定する
	//!
	//! @parameter [optionSheetController] 設定するOptionSheetController
	//-----------------------------------------------------------------
	public void SetOptionSheetController(OptionSheetController optionSheetController)
	{
		m_optionSheetController = optionSheetController;
	}
	#endregion

	#region 音を鳴らしたか判定するためのフラグの設定
	//-----------------------------------------------------------------
	//! @summary   音を鳴らしたか判定するためのフラグの設定
	//!
	//! @parameter [playedFlag] 設定するフラグの値
	//-----------------------------------------------------------------
	public void SetPlayedFlag(bool playedFlag)
	{
		m_playedFlag = playedFlag;
	}
	#endregion

	#region ノーツデータを設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツデータを設定する
	//!
	//! @parameter [notesData] 設定するノーツデータ
	//-----------------------------------------------------------------
	public void SetNotesData(PiarhythmDatas.NotesData notesData)
	{
		// データを設定する
		m_notesData = notesData;

		SetNotesScale(m_notesData.scale);
		SetNotesStartTime(m_notesData.startBeat);
		SetNotesLengthTime(m_notesData.noteLength);
		SetNotesColor(m_notesData.color);
	}
	#endregion

	#region ノーツが譜面の範囲外に出た時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツが譜面の範囲外に出た時の処理
	//!
	//! @parameter [collision] 衝突したオブジェクト
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "LimitArea")
		{
			// 光彩の色を赤に変える
			m_glowImage.glowColor = Color.red;
		}
	}
	#endregion

	#region ノーツが譜面の範囲内に戻った時の処理
	//-----------------------------------------------------------------
	//! @summary   ノーツが譜面の範囲内に戻った時の処理
	//!
	//! @parameter [collision] 衝突したオブジェクト
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.name == "LimitArea")
		{
			// 光彩の色を元に戻す
			m_glowImage.glowColor = (m_notesData.scale.Contains("#"))
				? new Color(m_notesData.color.r * SHARP_COLOR_PERCENTAGE, m_notesData.color.g * SHARP_COLOR_PERCENTAGE, m_notesData.color.b * SHARP_COLOR_PERCENTAGE, 1.0f)
				: m_notesData.color;
		}
	}
	#endregion
}
