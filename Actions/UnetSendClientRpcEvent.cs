// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;

using UnityEngine;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Sends an ClientRpc Event. Requires a PlayMakerUnetNetworkBehaviourProxy Component on the GameObject")]
	public class UnetSendClientRpcEvent : FsmStateAction
	{
		
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerUnetNetworkBehaviourProxy))]
		[Tooltip("The target to send the command event to. Must have a PlayMakerUnetNetworkBehaviourProxy component attached")]
		public FsmOwnerDefault gameObject;

		// this is not going to be featured in the action interface, but we need it for the FsmEvent to only allow global events
		public FsmEventTarget eventTarget;

		[RequiredField]
		[Tooltip("The event to send to the Client instance from the server")]
		public FsmEvent sendEvent;


		public override void Reset()
		{
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;

			sendEvent = null;

		}

		public override void OnEnter()
		{

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			PlayMakerUnetNetworkBehaviourProxy _nb = go.GetComponent<PlayMakerUnetNetworkBehaviourProxy>();

			if (_nb==null)
			{
				return;
			}
			_nb.Rpc_SendEvent(sendEvent.Name);

			Finish ();
		}
			
	}
}