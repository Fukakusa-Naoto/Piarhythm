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

	}
}
