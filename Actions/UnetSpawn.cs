// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;

using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Spawn the given GameObject on all Client which are ready.")]
	public class UnetSpawn : FsmStateAction
	{

		[RequiredField]
		[CheckForComponent(typeof(NetworkIdentity))]
		[Tooltip("The target to spawn over the network. Requires a NetworkIdentity")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Event Sent if Spawn could not be perform")]
		public FsmEvent failureEvent;



		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			Spawn ();

			Finish ();
		}

		void Spawn()
		{
			if (!NetworkServer.active) {
				Debug.LogWarning ("NetworkServer is not active. Cannot spawn objects without an active server.");

				Fsm.Event (failureEvent);
				return;
			}

			NetworkServer.Spawn(Fsm.GetOwnerDefaultTarget(gameObject));
		
		}
			
	}
}