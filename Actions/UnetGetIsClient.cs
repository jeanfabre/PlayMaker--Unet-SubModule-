// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Get The Networked GameObject isClient property. Requires a NetworkBehaviour Component on the GameObject")]
	public class UnetGetIsClient : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkBehaviour))]
		public FsmOwnerDefault gameObject;

		public FsmEvent isClientEvent;

		public FsmEvent isNotClientEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool isClient;


		public override void Reset()
		{
			gameObject = null;
			isClientEvent = null;
			isNotClientEvent = null;
			isClient = null;

		}

		public override void OnEnter()
		{
			CheckIfClient();


			Finish();

		}

		void CheckIfClient()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go == null) 
			{
				return;
			}

			NetworkBehaviour _nb = go.GetComponent<NetworkBehaviour>();

			if (_nb==null)
			{
				return;
			}
				
			isClient.Value = _nb.isClient;

			if (_nb.isClient)
			{
				Fsm.Event(isClientEvent);
			}else{
				Fsm.Event(isNotClientEvent);
			}

		}
	}
}

