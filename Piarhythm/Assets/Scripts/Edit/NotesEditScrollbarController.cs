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
	// コンポーネント
	private Scrollbar m_scrollbar = null;
	private RawImage m_rawImage = null;

	// マネージャー
	[SerializeField]
	private EditManager m_editManager = null;

	private int m_imageHeight;
	private Texture2D m_texture;
	private float[] m_samples;


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
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void UpdateTexture()
	{
		m_samples = m_editManager.GetAudioData();
		if (m_samples == null) return;

		int textureY = 0;
		float maxSample = 0;

		for (int i = 0, l = m_samples.Length; (i < l) && (textureY < m_imageHeight); ++i)
		{
			maxSample = Mathf.Max(maxSample, m_samples[i]);
			int denominator = (m_samples.Length < m_imageHeight) ? i : m_samples.Length / m_imageHeight;

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
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void SetScrollBarValue(float value)
	{
		m_scrollbar.value = value;
	}
	#endregion
}
