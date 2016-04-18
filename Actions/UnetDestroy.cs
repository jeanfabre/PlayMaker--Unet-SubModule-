// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;

using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Destroy the given GameObject on all corresponding GameObjects on all Clients. Action must be performed when peerType is Server")]
	public class UnetDestroy : FsmStateAction
	{

		[RequiredField]
		[CheckForComponent(typeof(NetworkIdentity))]
		[Tooltip("The target to destroy over the network, Requires a NetworkIdentity")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Event Sent if Destroy could not be performend, likely because PeerType is not Server.")]
		public FsmEvent failureEvent;


		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			if (Network.peerType == NetworkPeerType.Server) {
				NetworkServer.Destroy (Fsm.GetOwnerDefaultTarget (gameObject));
			} else {
				Debug.LogWarning ("NetworkPeerType is not Server ( ->"+Network.peerType+"). Cannot destroy objects over the network.");

				GameObject.Destroy (Fsm.GetOwnerDefaultTarget (gameObject));
				Fsm.Event(failureEvent);
			}


			Finish ();
		}

	}
}