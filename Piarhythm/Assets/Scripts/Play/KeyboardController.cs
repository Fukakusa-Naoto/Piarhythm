using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
	private Animator m_animator = null;
	private bool m_playFlag = false;

	public void Start()
	{
		m_animator = GetComponent<Animator>();
	}

	public void FinishAnimation()
	{
		m_playFlag = false;
	}

	public void PlayAnimation()
	{
		m_animator.SetBool("StartTrgger", true);
		m_playFlag = true;
	}

	public bool GetPlayFlag()
	{
		return m_playFlag;
	}
}
