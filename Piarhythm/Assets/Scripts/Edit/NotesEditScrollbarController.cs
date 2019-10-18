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
	[SerializeField]
	private RawImage m_image;
	private int m_imageHeight;

	private Texture2D m_texture;
	private float[] m_samples;

	public EditManager m_editManager;


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
		m_imageHeight = (int)Mathf.Pow(2.0f, 14.0f);
		InitializeTexture();
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
    }



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
		m_image.texture = m_texture;
	}



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
}
