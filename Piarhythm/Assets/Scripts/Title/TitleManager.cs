//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file		TitleManager.cs
//!
//! @summary	タイトルシーンの管理に関するC#スクリプト
//!
//! @date		2019.08.08
//!
//! @author		深草直斗
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の省略 ===========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// クラスの定義 =============================================================
public class TitleManager : MonoBehaviour
{
	private void Awake()
	{
		Screen.fullScreen = true;
	}


	//-----------------------------------------------------------------
	//! @summary   プレイボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnPlayButton()
	{
		// セレクトシーンに遷移する
		SceneManager.LoadScene((int)PiarhythmDatas.ScenenID.SCENE_SELECT, LoadSceneMode.Single);
	}



	//-----------------------------------------------------------------
	//! @summary   エディットボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnEditButton()
	{
		// エディットシーンに遷移する
		SceneManager.LoadScene((int)PiarhythmDatas.ScenenID.SCENE_EDIT, LoadSceneMode.Single);
	}



	//-----------------------------------------------------------------
	//! @summary   設定ボタンが押された時の処理
	//!
	//! @parameter [void] なし
	//!
	//! @return    なし
	//-----------------------------------------------------------------
	public void OnSettingButton()
	{
		// 設定シーンに遷移する
		SceneManager.LoadScene((int)PiarhythmDatas.ScenenID.SCENE_SETTING, LoadSceneMode.Additive);
	}
}
