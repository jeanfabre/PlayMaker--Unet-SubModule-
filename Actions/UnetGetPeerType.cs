// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Get The Network peertype status: Disconnected, Server, Client, Connecting")]
	public class UnetGetPeerType : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[ObjectType(typeof(NetworkPeerType))]
		[Tooltip("PeerType value")]
		public FsmEnum peerType;

		[UIHint(UIHint.Variable)]
		[Tooltip("PeerType as string")]
		public FsmString peerTypeAsString;

		[Tooltip("Event sent if peertype is 'Disconnected'")]
		public FsmEvent disconnected;

		[Tooltip("Event sent if peertype is 'Server'")]
		public FsmEvent server;

		[Tooltip("Event sent if peertype is 'Client'")]
		public FsmEvent client;

		[Tooltip("Event sent if peertype is 'Connecting'")]
		public FsmEvent connecting;

		[Tooltip("Useful if used for watching connection status")]
		public bool everyframe;


		public override void Reset()
		{
			peerType = null;
			peerTypeAsString = null;
			disconnected = null;
			server = null;
			client = null;
			connecting = null;
		}

		public override void OnEnter()
		{
			GetPeerType();

			if (!everyframe) 
			{
				Finish ();
			}

		}

		public override void OnUpdate()
		{
			GetPeerType ();
		}

		void GetPeerType()
		{

			if (!peerType.IsNone) 
			{
				peerType.Value = Network.peerType;
			}

			if (!peerTypeAsString.IsNone) 
			{
				peerTypeAsString.Value = Network.peerType.ToString();
			}

			if (disconnected!=null && Network.peerType == NetworkPeerType.Disconnected) 
			{
				Fsm.Event (disconnected);
			}

			if (server!=null && Network.peerType == NetworkPeerType.Server) 
			{
				Fsm.Event (server);
			}

			if (client!=null && Network.peerType == NetworkPeerType.Client) 
			{
				Fsm.Event (client);
			}

			if (connecting!=null && Network.peerType == NetworkPeerType.Connecting) 
			{
				Fsm.Event (connecting);
			}

		}
	}
}

