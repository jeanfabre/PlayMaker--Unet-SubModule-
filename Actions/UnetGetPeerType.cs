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
		public FsmEnum peerType;

		[UIHint(UIHint.Variable)]
		public FsmString peerTypeAsString;

		public FsmEvent disconnected;

		public FsmEvent server;

		public FsmEvent client;

		public FsmEvent connecting;


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


			Finish();

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

