
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		AdministrationSheetController.cs
//!
//! @summary	BGM管理シートに関するC#スクリプト
//!
//! @date		2019.11.20
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class AdministrationSheetController : MonoBehaviour
{
	// <メンバ変数>
	private int m_frameCount = 0;
	private float m_prevTime = 0.0f;
	private int m_fps = 0;

	// UI
	[SerializeField]
	private Text m_fpsText = null;
	[SerializeField]
	private Scrollbar m_notesVolumeScrollbar = null;
	[SerializeField]
	private Scrollbar m_bgmVolumeScrollbar = null;

	// マネージャー
	[SerializeField]
	private EditManager m_editManager = null;
	[SerializeField]
	private NotesManager m_notesManager = null;


	// メンバ関数の定義 =====================================================
	#region 更新処理
	//-----------------------------------------------------------------
	//! @summary   更新処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void Update()
    {
		m_frameCount++;
		float time = Time.realtimeSinceStartup - m_prevTime;

		if (time >= 0.5f)
		{
			m_fps = (int)Mathf.Round(m_frameCount / time);
			m_fpsText.text = "FPS　：　" + m_fps.ToString();

			m_frameCount = 0;
			m_prevTime = Time.realtimeSinceStartup;
		}
	}
	#endregion

	#region ノーツの音量の調節
	//-----------------------------------------------------------------
	//! @summary   ノーツの音量の調節
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnValueChangedNotesVolume()
	{
		m_notesManager.SetAllNotesVolume(m_notesVolumeScrollbar.value);
	}
	#endregion

	#region BGMの音量の調節
	//-----------------------------------------------------------------
	//! @summary   BGMの音量の調節
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnValueChangedBGMVolume()
	{
		m_editManager.SetAudioVolume(m_bgmVolumeScrollbar.value);
	}
	#endregion
}
