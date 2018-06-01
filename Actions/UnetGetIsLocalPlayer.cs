// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Get The Networked GameObject is Local Player property. Requires a NetworkBehaviour Component on the GameObject")]
	public class UnetGetIsLocalPlayer : ComponentAction<NetworkBehaviour>
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkBehaviour))]
		public FsmOwnerDefault gameObject;

		public FsmEvent IsLocalPlayerEvent;

		public FsmEvent IsNotLocalPlayerEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool isLocalPlayer;

		NetworkBehaviour _networkBehaviour;

		public override void Reset()
		{
			gameObject = null;
			IsLocalPlayerEvent = null;
			IsNotLocalPlayerEvent = null;
			isLocalPlayer = null;

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

			isLocalPlayer.Value = _networkBehaviour.isLocalPlayer;

			if (_networkBehaviour.isLocalPlayer)
			{
				Fsm.Event(IsLocalPlayerEvent);
			}else{
				Fsm.Event(IsNotLocalPlayerEvent);
			}

		}
	}
}

