// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;

using UnityEngine;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Sends an ClientRpc Event. Requires a PlayMakerUnetNetworkBehaviourProxy Component on the GameObject")]
	public class UnetSendClientRpcEvent : ComponentAction<PlayMakerUnetNetworkBehaviourProxy>
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

		[Tooltip("Optional data to be sent with the event, use GetEventInfo action to retrieve it, Leave to none for no effect")]
		public FsmString data;

		PlayMakerUnetNetworkBehaviourProxy _playMakerUnetNetworkBehaviourProxy;

		public override void Reset()
		{
			gameObject = null;
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;

			sendEvent = null;

			data = new FsmString(){UseVariable=true};
		}


		public override void OnEnter()
		{
			if (UpdateCache (Fsm.GetOwnerDefaultTarget (gameObject))) {
				_playMakerUnetNetworkBehaviourProxy = (PlayMakerUnetNetworkBehaviourProxy)this.cachedComponent;
				Execute ();
			}

			Finish();
		}

		void Execute()
		{
			if (_playMakerUnetNetworkBehaviourProxy == null) {
				return;
			}

			if (!data.IsNone) {
				_playMakerUnetNetworkBehaviourProxy.Rpc_SendEvent_WithStringData(sendEvent.Name,data.Value);
			} else {
				_playMakerUnetNetworkBehaviourProxy.Rpc_SendEvent (sendEvent.Name);
			}

			Finish ();
		}
			
	}
}