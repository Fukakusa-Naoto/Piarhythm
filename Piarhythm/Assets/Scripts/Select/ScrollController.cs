//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		ScrollController.cs
//!
//! @summary	楽曲タイルのスクロールに関するC#スクリプト
//!
//! @date		2019.09.05
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using UnityEngine;
using System;
using System.Collections;



// クラスの定義 =============================================================
public class ScrollController : MonoBehaviour
{
	// <メンバ変数>
	// 楽曲タイルのプレハブ
	public GameObject m_soundTilePrefab;
	// 分割数
	private int m_divNum = 20;
	// 半径の距離
	private float m_range = 400.0f;

	// クリックされた時の最初のX座標
	private float m_startXPosition;
	// クリックされている間のX座標
	private float m_nowXPosition;
	// リックされた時の最初の角度
	private Vector3 m_startRotation;


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
		for(int i = 0; i < 5; ++i)
		{
			// 角度を求める
			float deg = (360 / m_divNum) * i;
			deg += (360 / m_divNum) * 13;
			float rad = deg * Mathf.Deg2Rad;

			// 座標を求める
			float x = m_range * Mathf.Cos(rad);
			float z = m_range * Mathf.Sin(rad);

			// タイルを生成する
			GameObject tile = Instantiate(m_soundTilePrefab);

			// タイルを配置する
			RectTransform transform = tile.GetComponent<RectTransform>();
			transform.position = new Vector3(x, 0.0f, z) + this.transform.position;

			// 子に設定する
			transform.parent = this.transform;
		}
    }



	//-----------------------------------------------------------------
	//! @summary   初期化処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	void Update()
    {
		// 左クリックがされた
		if(Input.GetMouseButtonDown(0))
		{
			// 最初の座標と角度を記録
			m_startXPosition = Input.mousePosition.x;
			m_startRotation = transform.rotation.eulerAngles;
		}
		// マウスの左ボタンが押されている
		else if(Input.GetMouseButton(0))
		{
			// 現在の座標を更新する
			m_nowXPosition = Input.mousePosition.x;

			// 移動量を計算する
			float movement = m_nowXPosition - m_startXPosition;
			movement *= 0.1f;

			// 新しい角度を作成して代入する
			int angle = (int)(movement / (360 / m_divNum));
			Vector3 newRotation = new Vector3(0.0f, -angle * (360 / m_divNum), 0.0f);
			transform.rotation = Quaternion.Euler(newRotation + m_startRotation);
		}
	}



	//-----------------------------------------------------------------
	//! @summary   Z座標でソートする
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	private void ZSort()
	{
		// 子オブジェクトを取得する
		Transform[] children = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; ++i) children[i] = transform.GetChild(i);


	}



	//-----------------------------------------------------------------
	//! @summary   挿入ソート
	//!
	//! @parameter [array] ソート対象の配列
	//! @parameter [first] ソート対象の先頭インデックス
	//! @parameter [last] ソート対象の末尾インデックス
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	static void InsertSort<T>(T[] a, int first, int last)
	  where T : IComparable<T>
	{
		for (int i = first + 1; i <= last; i++)
			for (int j = i; j > first && a[j - 1].CompareTo(a[j]) > 0; --j)
				Swap(ref a[j], ref a[j - 1]);
	}



	public static void Swap<T>(ref T a, ref T b)
	{
		T c;

		c = a;
		a = b;
		b = c;
	}
}
