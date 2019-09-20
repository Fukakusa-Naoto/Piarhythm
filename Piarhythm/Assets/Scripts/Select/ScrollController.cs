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
using UnityEngine.UI;


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

	// タイルの順番を記憶しておく (0:左,4:右)
	private Transform[] m_soundTiles;

	// 正面のタイルの番号
	private int m_frontTileNumber;

	// 楽曲名リスト
	private string[] m_musicPieceList;

	// 選択シーンマネージャー
	public SelectManager m_selectManager;


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

		// タイルの並び順を記憶しておく
		m_soundTiles = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; ++i) m_soundTiles[i] = transform.GetChild(i);


		// Z座標順に描画を変更する
		ZSort();

		m_frontTileNumber = 0;

		m_musicPieceList = m_selectManager.GetMusicPieceList();

		// タイルに曲名を入れていく
		for (int i = 0; i < m_soundTiles.Length; ++i)
		{
			m_soundTiles[i].GetChild(0).GetComponent<Text>().text =
				m_musicPieceList[(m_musicPieceList.Length - i + m_frontTileNumber - 2) % m_musicPieceList.Length];
		}
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
			m_currentTileNumber = 0;
		}
		// マウスの左ボタンが押されている
		else if(Input.GetMouseButton(0))
		{
			// 現在の座標を更新する
			m_nowXPosition = Input.mousePosition.x;

			// 移動量を計算する
			float movement = m_nowXPosition - m_startXPosition;

			// 新しい角度を作成して代入する
			m_nextTileNumber = (int)(movement / (360 / m_divNum));


			if (m_currentTileNumber < m_nextTileNumber)		// 右回転
			{
				// 一番左のタイルの座標を記録する
				Vector3 tmpPosition = m_soundTiles[0].GetComponent<RectTransform>().position;

				// 1フレームのタイルの移動量に制限をかける
				m_nextTileNumber = m_currentTileNumber + Mathf.Clamp(m_nextTileNumber - m_currentTileNumber, 0, 1);

				// 正面のタイルを更新する
				m_frontTileNumber += m_nextTileNumber;

				// 全体を回転させる
				Vector3 newRotation = new Vector3(0.0f, -m_nextTileNumber * (360 / m_divNum), 0.0f);
				transform.rotation = Quaternion.Euler(newRotation + m_startRotation);

				// 正面のタイルの値を更新する
				m_currentTileNumber = m_nextTileNumber;

				// 一番右のタイルを一番左に移動させる
				m_soundTiles[m_soundTiles.Length - 1].GetComponent<RectTransform>().position = tmpPosition;

				// タイルの順番を右にシフトさせる
				ArrayRotate(ref m_soundTiles, 1);

				// 描画順を調整する
				ZSort();

				// タイルに曲名を入れていく
				for (int i = 0; i < m_soundTiles.Length; ++i)
				{
					m_soundTiles[i].GetChild(0).GetComponent<Text>().text =
						m_musicPieceList[(m_musicPieceList.Length - i + m_frontTileNumber - 2) % m_musicPieceList.Length];
				}
			}
			else if(m_currentTileNumber > m_nextTileNumber)		// 左回転
			{
				// 一番右のタイルの座標を記録する
				Vector3 tmpPosition = m_soundTiles[m_soundTiles.Length - 1].GetComponent<RectTransform>().position;

				// 1フレームのタイルの移動量に制限をかける
				m_nextTileNumber = m_currentTileNumber + Mathf.Clamp(m_nextTileNumber - m_currentTileNumber, -1, 0);

				// 正面のタイルを更新する
				m_frontTileNumber += m_nextTileNumber;

				// 全体を回転させる
				Vector3 newRotation = new Vector3(0.0f, -m_nextTileNumber * (360 / m_divNum), 0.0f);
				transform.rotation = Quaternion.Euler(newRotation + m_startRotation);

				// 正面のタイルの値を更新する
				m_currentTileNumber = m_nextTileNumber;

				// 一番左のタイルを一番右に移動させる
				m_soundTiles[0].GetComponent<RectTransform>().position = tmpPosition;

				// タイルの順番を左にシフトさせる
				ArrayRotate(ref m_soundTiles, -1);

				// 描画順を調整する
				ZSort();

				// タイルに曲名を入れていく
				for (int i = 0; i < m_soundTiles.Length; ++i)
				{
					int soundNumber = (m_musicPieceList.Length - i + m_frontTileNumber - 2) % m_musicPieceList.Length;
					m_soundTiles[i].GetChild(0).GetComponent<Text>().text = m_musicPieceList[soundNumber];
				}
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


	//-----------------------------------------------------------------
	//! @summary   配列をローテーションさせる
	//!
	//! @parameter [array] ローテーションさせる配列
	//! @parameter [shiftCount] シフトさせる数
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public static void ArrayRotate<T>(ref T[] array, int shiftCount)
	{
		T[] backupArray = new T[array.Length];

		for (int i = 0; i < array.Length; i++)
		{
			backupArray[(i + array.Length + shiftCount % array.Length) % array.Length] = array[i];
		}

		array = backupArray;
	}
}
