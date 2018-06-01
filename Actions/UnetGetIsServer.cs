// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Get The Networked GameObject isServer property. Requires a NetworkBehaviour Component on the GameObject")]
	public class UnetGetIsServer : ComponentAction<NetworkBehaviour>
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkBehaviour))]
		public FsmOwnerDefault gameObject;

		public FsmEvent isServerEvent;

		public FsmEvent isNotServerEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool isServer;

		NetworkBehaviour _networkBehaviour;

		public override void Reset()
		{
			gameObject = null;
			isServerEvent = null;
			isNotServerEvent = null;
			isServer = null;
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

			isServer.Value = _networkBehaviour.isServer;

			if (_networkBehaviour.isServer)
			{
				Fsm.Event(isServerEvent);
			}else{
				Fsm.Event(isNotServerEvent);
			}

		}
	}
}