// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Get The Networked GameObject isClient property. Requires a NetworkBehaviour Component on the GameObject")]
	public class UnetGetIsClient : ComponentAction<NetworkBehaviour>
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkBehaviour))]
		public FsmOwnerDefault gameObject;

		public FsmEvent isClientEvent;

		public FsmEvent isNotClientEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool isClient;

		NetworkBehaviour _networkBehaviour;

		public override void Reset()
		{
			gameObject = null;
			isClientEvent = null;
			isNotClientEvent = null;
			isClient = null;
		}

		public override void OnEnter()
		{
			if (UpdateCache (Fsm.GetOwnerDefaultTarget (gameObject))) {
				_networkBehaviour = (NetworkBehaviour)this.cachedComponent;
				Execute ();
			}

			Finish();
		}

		void Execute()
		{
			if (_networkBehaviour == null) {
				return;
			}

			isClient.Value = _networkBehaviour.isClient;

			if (_networkBehaviour.isClient)
			{
				Fsm.Event(isClientEvent);
			}else{
				Fsm.Event(isNotClientEvent);
			}
		}
	}
}