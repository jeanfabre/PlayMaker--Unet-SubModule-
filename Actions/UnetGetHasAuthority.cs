// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Get The Networked GameObject HasAuthority property. Requires a NetworkBehaviour Component on the GameObject")]
	public class UnetGetHasAuthority : ComponentAction<NetworkBehaviour>
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkBehaviour))]
		public FsmOwnerDefault gameObject;

		public FsmEvent hasAuthorityEvent;

		public FsmEvent hasNotAuthorityEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool hasAuthority;

		NetworkBehaviour _networkBehaviour;

		public override void Reset()
		{
			gameObject = null;
			hasAuthorityEvent = null;
			hasNotAuthorityEvent = null;
			hasAuthority = null;
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

			hasAuthority.Value = _networkBehaviour.hasAuthority;

			if (_networkBehaviour.isClient)
			{
				Fsm.Event(hasAuthorityEvent);
			}else{
				Fsm.Event(hasNotAuthorityEvent);
			}
		}
	}
}