//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		BGMSheetController.cs
//!
//! @summary	BGMシートに関するC#スクリプト
//!
//! @date		2019.11.07
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


// クラスの定義 =============================================================
public class BGMSheetController : MonoBehaviour
{
	// <メンバ変数>
	private string m_filePath = null;
	private bool m_loadFlag = false;
	private AudioClip m_audioClip = null;
	private IEnumerator<UnityWebRequestAsyncOperation> m_coroutine = null;

	// UI
	[SerializeField]
	private Text m_nameText = null;
	[SerializeField]
	private InputField m_startTimeInputField = null;
	[SerializeField]
	private InputField m_endTimeInputField = null;

	// BGMデータ
	private PiarhythmDatas.BGMData m_BGMData;


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
		// 初期化
		m_BGMData = new PiarhythmDatas.BGMData();
	}
	#endregion

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
		if(m_loadFlag)
		{
			// ファイルを読み込む
			StartCoroutine(m_coroutine);

			// 読み込みが完了した
			if (m_coroutine.Current.isDone)
			{
				// AudioClipへ変換する
				m_audioClip = PiarhythmUtility.ConvertToAudioClip(m_coroutine.Current.webRequest);

				// 読み込みフラグを倒す
				m_loadFlag = false;

				// BGMデータを設定する
				SetBGMData();
			}
		}
	}
	#endregion

	#region 曲選択ボタンが押された時の処理
	//-----------------------------------------------------------------
	//! @summary   曲選択ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnClickSelectButton()
	{
		// ダイアログを開いて、ファイルパスを取得する
		m_filePath = PiarhythmUtility.OpenExistFileDialog();

		// ファイルが選択されていなければ処理を終了する
		if (m_filePath != "") return;

		// 読み込み開始フラグをたてる
		m_loadFlag = true;

		// コルーチンを設定する
		m_coroutine = PiarhythmUtility.LoadAudioFile(m_filePath);
	}
	#endregion

	#region 曲の開始時間が入力された時の処理
	//-----------------------------------------------------------------
	//! @summary   曲の開始時間が入力された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditStartTimeInputField()
	{

	}
	#endregion

	#region 曲の終了時間が入力された時の処理
	//-----------------------------------------------------------------
	//! @summary   曲の終了時間が入力された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEndEditEndTimeInputField()
	{

	}
	#endregion

	#region BGMデータをUIへ反映させる
	//-----------------------------------------------------------------
	//! @summary   BGMデータをUIへ反映させる
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void DisplayBGMData()
	{

	}
	#endregion

	#region BGMデータを設定する
	//-----------------------------------------------------------------
	//! @summary   BGMデータを設定する
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void SetBGMData()
	{
		m_BGMData.path = m_filePath;
	}
	#endregion
}
