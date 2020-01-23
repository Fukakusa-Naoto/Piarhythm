//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		ConnectNoteController.cs
//!
//! @summary	連結ノーツの制御に関するC#スクリプト
//!
//! @date		2019.11.29
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class ConnectNoteController : MonoBehaviour
{
	// <メンバ変数>
	// キャンバス
	private Canvas m_canvas = null;
	// キー情報が保存された連想配列
	private Dictionary<string, RectTransform> m_keyDictionary = null;
	// 連結に使用したノーツデータのリスト
	private List<PiarhythmDatas.NoteData> m_noteList = new List<PiarhythmDatas.NoteData>();
	// 音を鳴らしたか判定するためのフラグ
	private bool m_playedFlag = false;
	// 音を鳴らす開始時間
	private float m_startTime = 0.0f;

	// コンポーネント
	private RectTransform m_transform = null;
	private GlowImage m_glowImage = null;
	private RectTransform m_musicalScoreTransform = null;
	private AudioSource m_audioSource = null;

	// コントローラー
	private OptionSheetController m_optionSheetController = null;
	private ConnectNoteSheetController m_connectNoteSheetController = null;

	// マネージャー
	private NotesManager m_noteManager = null;


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

		// 色の初期化
		UnityEngine.Color color = new UnityEngine.Color(m_noteList[0].m_color.r, m_noteList[0].m_color.g, m_noteList[0].m_color.b, m_noteList[0].m_color.a);
		m_glowImage.color = m_glowImage.glowColor = color;

		// スケールの初期化
		m_transform.localScale = Vector3.one;

		// 音階の設定
		SetNotesScale(m_noteList[0].m_scale);

		// 手前に持ってくる
		Vector3 position = m_transform.localPosition;
		position.z = 0.0f;
		m_transform.localPosition = position;

		// 開始時間と長さの初期化
		PiarhythmDatas.PositionData positionData = m_optionSheetController.ConvertToPositionData(m_noteList[0].m_startBeat, m_noteList[0].m_noteLength);
		m_transform.offsetMin = new Vector2(m_transform.offsetMin.x, positionData.m_position);
		m_transform.offsetMax = new Vector2(m_transform.offsetMax.x, m_transform.offsetMin.y + positionData.m_lenght);
		for (int i = 1; i < m_noteList.Count; ++i)
		{
			positionData = m_optionSheetController.ConvertToPositionData(m_noteList[i].m_startBeat, m_noteList[i].m_noteLength);
			Vector2 offsetMax = m_transform.offsetMax;
			offsetMax.y += positionData.m_lenght;
			m_transform.offsetMax = offsetMax;
		}

		// 光彩を切る
		m_glowImage.glowSize = 0.0f;
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
		string scale = m_noteList[0].m_scale;
		// ワールド座標のマウス座標を取得する
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 10.0f;
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

		// 最も近い音階を調べる
		foreach (KeyValuePair<string, RectTransform> n in m_keyDictionary)
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
		if (m_connectNoteSheetController != null) m_connectNoteSheetController.DisplayNotes(this);
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
		m_noteManager.SetSelectNotes(gameObject);

		// 光彩を起動する
		float glowSize = PiarhythmDatas.MAX_GLOW_SIZE - (m_transform.sizeDelta.y * 0.1f);
		glowSize = Mathf.Clamp(glowSize, PiarhythmDatas.MIN_GLOW_SIZE, PiarhythmDatas.MAX_GLOW_SIZE);
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
		positionData.m_position = m_transform.offsetMin.y;
		positionData.m_lenght = m_transform.sizeDelta.y;

		// データの更新
		PiarhythmDatas.NoteData notesData = m_optionSheetController.ConvertToNotesData(positionData);
		SetStartBeat(notesData.m_startBeat);
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
		for (int i = 0; i < m_noteList.Count; ++i) m_noteList[i].m_scale = scale;

		// 座標を設定された音階の位置に移動させる
		m_transform.position = new Vector3(m_keyDictionary[scale].position.x, m_transform.position.y, m_transform.position.z);

		// 音を設定する
		m_audioSource.clip = m_keyDictionary[scale].GetComponent<AudioSource>().clip;

		// 幅を合わせる
		//GlowImageの解析と改造が終わるまで下の処理で代用する
		//float width = m_keyDictionary[scale].sizeDelta.x
		//	* m_keyDictionary[scale].parent.GetComponent<RectTransform>().localScale.x;
		//m_transform.sizeDelta = new Vector2(width, m_transform.sizeDelta.y);

		Vector3 localScale = m_transform.localScale;
		localScale.x = (scale.Contains("#")) ? 0.6f : 0.8f;
		m_transform.localScale = localScale;

		// #の色を変化させる
		m_glowImage.color = (m_noteList[0].m_scale.Contains("#"))
			? new UnityEngine.Color(m_noteList[0].m_color.r * PiarhythmDatas.SHARP_COLOR_PERCENTAGE, m_noteList[0].m_color.g * PiarhythmDatas.SHARP_COLOR_PERCENTAGE, m_noteList[0].m_color.b * PiarhythmDatas.SHARP_COLOR_PERCENTAGE, 1.0f)
			: new UnityEngine.Color(m_noteList[0].m_color.r, m_noteList[0].m_color.g, m_noteList[0].m_color.b, 1.0f);
	}
	#endregion

	#region ノーツの開始の拍数を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの開始の拍数を設定する
	//!
	//! @parameter [startBeat] 設定する開始の拍数
	//-----------------------------------------------------------------
	public void SetStartBeat(float startBeat)
	{
		// 開始位置がマイナスだった場合は処理を終了する
		if (startBeat < 0.0f) return;

		// 差分を計算する
		float difference = startBeat - m_noteList[0].m_startBeat;

		// データを更新する
		foreach (PiarhythmDatas.NoteData noteData in m_noteList) noteData.m_startBeat += difference;

		// 位置の更新
		PiarhythmDatas.PositionData positionData = m_optionSheetController.ConvertToPositionData(m_noteList[0].m_startBeat, m_noteList[0].m_noteLength);
		m_transform.offsetMin = new Vector2(m_transform.offsetMin.x, positionData.m_position);
		m_transform.offsetMax = new Vector2(m_transform.offsetMax.x, m_transform.offsetMin.y + positionData.m_lenght);
		for (int i = 1; i < m_noteList.Count; ++i)
		{
			positionData = m_optionSheetController.ConvertToPositionData(m_noteList[i].m_startBeat, m_noteList[i].m_noteLength);
			Vector2 offsetMax = m_transform.offsetMax;
			offsetMax.y += positionData.m_lenght;
			m_transform.offsetMax = offsetMax;
		}

		// UIを更新
		if(m_connectNoteSheetController) m_connectNoteSheetController.DisplayNotes(this);
	}
	#endregion

	#region ノーツの色を設定する
	//-----------------------------------------------------------------
	//! @summary   ノーツの色を設定する
	//!
	//! @parameter [color] 設定する色
	//-----------------------------------------------------------------
	public void SetColor(PiarhythmDatas.Color color)
	{
		// 情報を更新する
		foreach(PiarhythmDatas.NoteData noteData in m_noteList) noteData.m_color = color;

		// #の色を変化させる
		m_glowImage.color = (m_noteList[0].m_scale.Contains("#"))
			? new UnityEngine.Color(m_noteList[0].m_color.r * PiarhythmDatas.SHARP_COLOR_PERCENTAGE, m_noteList[0].m_color.g * PiarhythmDatas.SHARP_COLOR_PERCENTAGE, m_noteList[0].m_color.b * PiarhythmDatas.SHARP_COLOR_PERCENTAGE, 1.0f)
			: new UnityEngine.Color(m_noteList[0].m_color.r, m_noteList[0].m_color.g, m_noteList[0].m_color.b, 1.0f);

		// 光彩の色を更新する
		m_glowImage.glowColor = new UnityEngine.Color(color.r, color.g, color.b, 1.0f);
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

	#region 連結に使用したノーツデータを保存する
	//-----------------------------------------------------------------
	//! @summary   連結に使用したノーツデータを保存する
	//!
	//! @parameter [noteData] 保存するノーツデータ
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void AddNoteData(PiarhythmDatas.NoteData noteData)
	{
		// 登録する
		m_noteList.Add(noteData);
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
		if (flag)
		{
			// 光彩を起動する
			float glowSize = PiarhythmDatas.MAX_GLOW_SIZE - (m_transform.sizeDelta.y * 0.1f);
			glowSize = Mathf.Clamp(glowSize, PiarhythmDatas.MIN_GLOW_SIZE, PiarhythmDatas.MAX_GLOW_SIZE);
			m_glowImage.glowSize = glowSize;
		}
		else
		{
			m_glowImage.glowSize = 0.0f;
		}
	}
	#endregion

	#region 連結されているノーツデータの取得
	//-----------------------------------------------------------------
	//! @summary   連結されているノーツデータの取得
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public PiarhythmDatas.NoteData GetNoteData()
	{
		return m_noteList[0];
	}
	#endregion

	#region ConnectNoteSheetControllerを設定する
	//-----------------------------------------------------------------
	//! @summary   ConnectNoteSheetControllerを設定する
	//!
	//! @parameter [ConnectNoteSheetController] 設定するConnectNoteSheetController
	//-----------------------------------------------------------------
	public void SetConnectNoteSheetController(ConnectNoteSheetController connectNoteSheetController)
	{
		m_connectNoteSheetController = connectNoteSheetController;
	}
	#endregion

	#region NoteManagerを設定する
	//-----------------------------------------------------------------
	//! @summary   NoteManagerを設定する
	//!
	//! @parameter [noteManager] 設定するNoteManager
	//-----------------------------------------------------------------
	public void SetNoteManager(NotesManager noteManager)
	{
		m_noteManager = noteManager;
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
		if (!m_playedFlag)
		{
			// 経過時間がノーツの開始時間を過ぎた
			if (m_startTime <= elapsedTime)
			{
				// 音を鳴らす
				m_audioSource.Play();

				// 音を鳴らしたからフラグを立てる
				m_playedFlag = true;
			}
		}
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
		// 開始時間を取得する
		if (!playedFlag) m_startTime = m_optionSheetController.GetStartTime(m_noteList[0].m_startBeat);
		m_playedFlag = playedFlag;
	}
	#endregion
}
