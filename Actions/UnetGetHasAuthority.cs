// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Get The Networked GameObject HasAuthority property")]
	public class UnetGetHasAuthority : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkBehaviour))]
		public FsmOwnerDefault gameObject;

		public FsmEvent hasAuthorityEvent;

		public FsmEvent hasNotAuthorityEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool hasAuthority;


		public override void Reset()
		{
			gameObject = null;
			hasAuthorityEvent = null;
			hasNotAuthorityEvent = null;
			hasAuthority = null;

		}

		public override void OnEnter()
		{
			CheckIfHasAuthority();


			Finish();

		}

		void CheckIfHasAuthority()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			NetworkBehaviour _nb = go.GetComponent<NetworkBehaviour>();

			if (_nb==null)
			{
				return;
			}


			hasAuthority.Value = _nb.hasAuthority;

			if (_nb.isClient)
			{
				Fsm.Event(hasAuthorityEvent);
			}else{
				Fsm.Event(hasNotAuthorityEvent);
			}

		}
	}
}

