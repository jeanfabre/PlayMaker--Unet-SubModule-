// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Get The Networked GameObject is Local Player property. Requires a NetworkBehaviour Component on the GameObject")]
	public class UnetGetIsLocalPlayer : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkBehaviour))]
		public FsmOwnerDefault gameObject;

		public FsmEvent IsLocalPlayerEvent;

		public FsmEvent IsNotLocalPlayerEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool isLocalPlayer;


		public override void Reset()
		{
			gameObject = null;
			IsLocalPlayerEvent = null;
			IsNotLocalPlayerEvent = null;
			isLocalPlayer = null;

		}

		public override void OnEnter()
		{
			CheckIfLocalPlayer();


			Finish();

		}

		void CheckIfLocalPlayer()
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


			isLocalPlayer.Value = _nb.isLocalPlayer;

			if (_nb.isLocalPlayer)
			{
				Fsm.Event(IsLocalPlayerEvent);
			}else{
				Fsm.Event(IsNotLocalPlayerEvent);
			}

		}
	}
}

