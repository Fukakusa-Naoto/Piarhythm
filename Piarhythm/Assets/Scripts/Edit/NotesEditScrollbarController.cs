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
	[SerializeField]
	private int m_imageWidth;

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
		m_imageWidth = (int)GetComponent<RectTransform>().sizeDelta.x;
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
		UpdateTexture();
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
		m_texture = new Texture2D(m_imageWidth, 1);
		m_texture.SetPixels(Enumerable.Range(0, m_imageWidth).Select(_ => Color.clear).ToArray());
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
	private void UpdateTexture()
	{
		m_samples = m_editManager.GetAudioData();
		if (m_samples == null) return;

		int textureX = 0;
		int skipSamples = 200;
		float maxSample = 0;

		for (int i = 0, l = m_samples.Length; i < l && textureX < m_imageWidth; i++)
		{
			maxSample = Mathf.Max(maxSample, m_samples[i]);

			if (i % skipSamples == 0)
			{
				m_texture.SetPixel(textureX, 0, new Color(maxSample, 0, 0));
				maxSample = 0;
				textureX++;
			}
		}

		m_texture.Apply();
	}
}
