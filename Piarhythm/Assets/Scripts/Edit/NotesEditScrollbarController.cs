//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		NotesEditScrollbarController.cs
//!
//! @summary	ノーツ編集のスクロールバーに関するC#スクリプト
//!
//! @date		2019.10.15
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


// クラスの定義 =============================================================
public class NotesEditScrollbarController : MonoBehaviour
{
	// <メンバ変数>
	private int m_imageHeight;
	private Texture2D m_texture;
	private AudioClip m_audioClip = null;

	// コンポーネント
	private Scrollbar m_scrollbar = null;
	private RawImage m_rawImage = null;

	// コントローラー
	[SerializeField]
	private MusicalScoreController m_musicalScoreController = null;
	[SerializeField]
	private MenuController m_menuController = null;



	// メンバ関数の定義 =====================================================
	#region 初期化処理
	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Start()
    {
		// コンポーネントの取得
		m_scrollbar = GetComponent<Scrollbar>();
		m_rawImage = GetComponent<RawImage>();

		m_imageHeight = (int)Mathf.Pow(2.0f, 14.0f);
		InitializeTexture();
    }
	#endregion

	#region テクスチャの初期化処理
	//-----------------------------------------------------------------
	//! @summary   テクスチャの初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void InitializeTexture()
	{
		m_texture = new Texture2D(1, m_imageHeight);
		m_texture.SetPixels(Enumerable.Range(0, m_imageHeight).Select(n => Color.white).ToArray());
		m_texture.Apply();
		m_rawImage.texture = m_texture;

		for (int i = 0; i < m_imageHeight; ++i)
		{
			m_texture.SetPixel(0, i, new Color(0.01f, 0, 0));
		}

		m_texture.Apply();
	}
	#endregion

	#region テクスチャの更新処理
	//-----------------------------------------------------------------
	//! @summary   テクスチャの更新処理
	//!
	//! @parameter [bgmData] BGMデータ
	//! @parameter [wholeTime] 曲全体の時間
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateTexture(PiarhythmDatas.BGMData bgmData, float wholeTime)
	{
		// オーディオクリップが無ければ初期化する
		if (m_audioClip == null)
		{
			for (int i = 0; i < m_imageHeight; ++i)
			{
				m_texture.SetPixel(0, i, new Color(0.01f, 0, 0));
			}

			m_texture.Apply();

			// 処理を終了する
			return;
		}

		int textureY = 0;
		float maxSample = 0;

		// サンプルを取得する
		float[] allSamples = new float[m_audioClip.samples * m_audioClip.channels];
		float offset = bgmData.m_startTime * m_audioClip.frequency * m_audioClip.channels;
		m_audioClip.GetData(allSamples, (int)offset);

		// 使用するサンプル分だけ取り出す
		float totalTime = bgmData.m_endTime - bgmData.m_startTime;
		int totalOffset = (int)(totalTime * m_audioClip.frequency * m_audioClip.channels);
		float[] samples = new float[totalOffset];
		for (int i = 0; i < totalOffset; ++i) samples[i] = allSamples[i];

		// 画像の高さ分を超えるまで処理する
		int wholeOffset = (int)(wholeTime * m_audioClip.frequency * m_audioClip.channels);
		for (int i = 0; textureY < m_imageHeight; ++i)
		{
			// 大きい方の値を取得する
			if (i < samples.Length) maxSample = Mathf.Max(maxSample, samples[i]);

			int denominator = (wholeOffset < m_imageHeight) ? i : wholeOffset / m_imageHeight;

			if (i % denominator == 0)
			{
				m_texture.SetPixel(0, textureY, new Color(maxSample, 0, 0));
				maxSample = 0;
				textureY++;
			}
		}

		m_texture.Apply();
	}
	#endregion

	#region スクロールの値を取得
	//-----------------------------------------------------------------
	//! @summary   スクロールの値を取得
	//!
	//! @parameter [void] なし
	//!
	//! @return    スクロールバーの値(0.0~1.0)
	//-----------------------------------------------------------------
	public float GetScrollBarValue()
	{
		return m_scrollbar.value;
	}
	#endregion

	#region スクロールの値を設定
	//-----------------------------------------------------------------
	//! @summary   スクロールの値を設定
	//!
	//! @parameter [value] 設定するスクロールバーの値(0.0~1.0)
	//-----------------------------------------------------------------
	public void SetScrollBarValue(float value)
	{
		m_scrollbar.value = value;
	}
	#endregion

	#region オーディオクリップの設定
	//-----------------------------------------------------------------
	//! @summary   オーディオクリップの設定
	//!
	//! @parameter [audioClip] 設定するオーディオクリップ
	//-----------------------------------------------------------------
	public void SetAudioClip(AudioClip audioClip)
	{
		m_audioClip = audioClip;
	}
	#endregion

	#region スクロールバーの値が変更された時の処理
	//-----------------------------------------------------------------
	//! @summary   スクロールバーの値が変更された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnValueChangedScrollbar()
	{
		// MusicalScoreから時間を現在時間を取得する
		float nowTime = m_musicalScoreController.GetNowTime();

		// メニューバーの現在時間に反映させる
		m_menuController.UpdateDisplayNowTime(nowTime);
	}
	#endregion
}
