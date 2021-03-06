﻿// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Ecosystem.Networking;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Send a PlayMaker Event over the Network. Requires a PlayMakerUnetNetworkBehaviourProxy Component on the GameObject")]
	public class UnetSendEvent : FsmStateAction
	{
		
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerUnetNetworkBehaviourProxy))]
		public FsmOwnerDefault gameObject;

		// ONLY ACCEPTS BROADCAST OR SELF
		public FsmEventTarget eventTarget;

		public FsmEvent eventToSend;

		public override void Reset()
		{
			gameObject = null;
			eventTarget = null;
			eventToSend = null;

		}

		public override void OnEnter()
		{
			Execute();


			Finish();

		}

		void Execute()
		{
			/*
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			PlayMakerUnetGameObjectProxy _proxy = go.GetComponent<PlayMakerUnetGameObjectProxy>();

			if (_proxy==null)
			{
				return;
			}

			_proxy.CmdSendPlayMakerEvent(eventToSend.Name,null);
*/
		}
	}
}

