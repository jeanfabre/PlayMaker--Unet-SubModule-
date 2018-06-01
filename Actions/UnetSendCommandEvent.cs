// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;

using UnityEngine;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Sends an Command Event. Requires a PlayMakerUnetNetworkBehaviourProxy Component on the GameObject")]
	public class UnetSendCommandEvent : FsmStateAction
	{
		
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerUnetNetworkBehaviourProxy))]
		[Tooltip("The target to send the command event to. Must have a PlayMakerUnetNetworkBehaviourProxy component attached")]
		public FsmOwnerDefault gameObject;

		// this is not going to be featured in the action interface, but we need it for the FsmEvent to only allow global events
		public FsmEventTarget eventTarget;

		[RequiredField]
		[Tooltip("The event to send to the Server instance")]
		public FsmEvent sendEvent;

		[Tooltip("Optional data to be sent with the event, use GetEventInfo action to retrieve it, Leave to none for no effect")]
		public FsmString data;


		public override void Reset()
		{
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;

			sendEvent = null;

			data = new FsmString(){UseVariable=true};

		}

		public override void OnEnter()
		{

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go == null) 
			{
				return;
			}

			PlayMakerUnetNetworkBehaviourProxy _nb = go.GetComponent<PlayMakerUnetNetworkBehaviourProxy>();

			if (_nb==null)
			{
				return;
			}

			if (!data.IsNone) {
				_nb.Cmd_SendEvent_WithStringData(sendEvent.Name,data.Value);
			} else {
				_nb.Cmd_SendEvent (sendEvent.Name);
			}

			Finish ();
		}
			
	}
}