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

	private int m_currentTileNumber;
	private int m_nextTileNumber;


	// 描画順を管理する配列
	private Transform[] m_drowingTransforms;


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
			RectTransform rectTransform = tile.GetComponent<RectTransform>();
			rectTransform.position = new Vector3(x, 0.0f, z) + transform.position;

			// 子に設定する
			rectTransform.parent = transform;
		}

		m_currentTileNumber = m_nextTileNumber = 0;

		// Z座標順に描画を変更する
		ZSort();


		// 子オブジェクトを取得する
		m_drowingTransforms = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; ++i) m_drowingTransforms[i] = transform.GetChild(i);
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
			m_nextTileNumber = (int)(movement / (360 / m_divNum));

			if (m_currentTileNumber < m_nextTileNumber)
			{
				Vector3 newRotation = new Vector3(0.0f, -m_nextTileNumber * (360 / m_divNum), 0.0f);
				transform.rotation = Quaternion.Euler(newRotation + m_startRotation);

				m_currentTileNumber = m_nextTileNumber;


				// 描画順を調整する
				Swap(ref m_drowingTransforms[0], ref m_drowingTransforms[1]);


				for (int i = 0; i < transform.childCount; ++i) m_drowingTransforms[i].SetAsFirstSibling();
			}
			else if(m_currentTileNumber > m_nextTileNumber)
			{
				Vector3 newRotation = new Vector3(0.0f, -m_nextTileNumber * (360 / m_divNum), 0.0f);
				transform.rotation = Quaternion.Euler(newRotation + m_startRotation);

				m_currentTileNumber = m_nextTileNumber;


				// 描画順を調整する


				for (int i = 0; i < transform.childCount; ++i) m_drowingTransforms[i].SetAsFirstSibling();
			}
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

		QuickSort(children, 0, children.Length - 1);

		for (int i = 0; i < transform.childCount; ++i) children[i].SetAsFirstSibling();
	}



	/*
   * 軸要素の選択
   * 順に見て、最初に見つかった異なる2つの要素のうち、
   * 大きいほうの番号を返します。
   * 全部同じ要素の場合は -1 を返します。
   */
	int Pivot(Transform[] a, int i, int j)
	{
		int k = i + 1;
		while (k <= j && a[i].position.z.Equals(a[k].position.z)) k++;
		if (k > j) return -1;
		if (a[i].position.z >= a[k].position.z) return i;
		return k;
	}

	/*
	 * パーティション分割
	 * a[i]～a[j]の間で、x を軸として分割します。
	 * x より小さい要素は前に、大きい要素はうしろに来ます。
	 * 大きい要素の開始番号を返します。
	 */
	int Partition(Transform[] a, int i, int j, Transform x)
	{
		int l = i, r = j;

		// 検索が交差するまで繰り返します
		while (l <= r)
		{
			// 軸要素以上のデータを探します
			while (l <= j && a[l].position.z < x.position.z) l++;

			// 軸要素未満のデータを探します
			while (r >= i && a[r].position.z >= x.position.z) r--;

			if (l > r) break;
			Swap(ref a[l], ref a[r]);
			l++; r--;
		}
		return l;
	}

	/*
	 * クイックソート（再帰用）
	 * 配列aの、a[i]からa[j]を並べ替えます。
	 */
	public void QuickSort(Transform[] a, int i, int j)
	{
		if (i == j) return;
		int p = Pivot(a, i, j);
		if (p != -1)
		{
			int k = Partition(a, i, j, a[p]);
			QuickSort(a, i, k - 1);
			QuickSort(a, k, j);
		}
	}


	static void Swap<T>(ref T lhs, ref T rhs)
	{
		T temp;
		temp = lhs;
		lhs = rhs;
		rhs = temp;
	}
}
