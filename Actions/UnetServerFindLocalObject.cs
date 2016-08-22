// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("This finds the local NetworkIdentity object with the specified network Id on a server.\n\nSince netIds are the same on the server and all clients for a game, this allows clients to send a netId of a local game objects, and have the server find the corresponding server object.")]
	public class UnetServerFindLocalObject : FsmStateAction
	{
		[Tooltip("NetID value")]
		public FsmInt netID;

		[ActionSection("Result")]

		[Tooltip("the Local Object on the server with the given netID")]
		public FsmGameObject localObject;

		[Tooltip("True if netId was resolved with a localObject")]
		public FsmBool foundFlag;

		[Tooltip("Event Sent if netId was resolved with a localObject")]
		public FsmEvent foundEvent;

		[Tooltip("Event send if netId was NOT resolved with a localObject")]
		public FsmEvent notFoundEvent;

		[Tooltip("Event send if call failed, likely because we are not on a server")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			netID = null;
			localObject = null;
			foundFlag = null;
			foundEvent = null;
			notFoundEvent = null;
			failureEvent = null;
		}

		public override void OnEnter()
		{
			FindlocalObject ();

			Finish ();
		}

		void FindlocalObject()
		{
			if (!Network.isServer) {
				Event (failureEvent);
			}

			GameObject _obj =	NetworkServer.FindLocalObject(new NetworkInstanceId((uint)netID.Value));
			if (_obj != null) {
				localObject.Value = _obj;
				foundFlag.Value = true;
				Event (foundEvent);
			} else {
				localObject.Value = null;
				foundFlag.Value = false;
				Event (notFoundEvent);
			}
		}


	}
}

