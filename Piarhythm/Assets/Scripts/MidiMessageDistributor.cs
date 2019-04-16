﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiMessageDistributor : MonoBehaviour
{
	public GameObject[] targets;
	MidiReceiver receiver;

	void Start()
	{
		receiver = FindObjectOfType(typeof(MidiReceiver)) as MidiReceiver;
	}

	void Update()
	{
		while (!receiver.IsEmpty)
		{
			var message = receiver.PopMessage();
			if (message.status == 0x90)
			{
				foreach (var go in targets)
				{
					go.SendMessage("OnNoteOn", message);
				}
			}
			else if (message.status == 0x80)
			{
				foreach (var go in targets)
				{
					go.SendMessage("OnNoteOff", message);
				}
			}
		}
	}
}
