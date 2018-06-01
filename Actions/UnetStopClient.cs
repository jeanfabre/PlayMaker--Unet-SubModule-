// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Stops a client. Requires a NetworkManager Component")]
	public class UnetStopClient : ComponentAction<NetworkManager>
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkManager))]
		[Tooltip("The target. An NetworkManager component is required")]
		public FsmOwnerDefault gameObject;

		NetworkManager _networkManager;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			if (UpdateCache (Fsm.GetOwnerDefaultTarget (gameObject))) {
				_networkManager = (NetworkManager)this.cachedComponent;
				Execute ();
			}
		
			Finish();
		}

		void Execute()
		{
			if (_networkManager == null) {
				return;
			}
			_networkManager.StopClient();
		}

	}
}