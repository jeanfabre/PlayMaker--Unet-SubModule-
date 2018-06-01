// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Get The Networked GameObject isServer property. Requires a NetworkBehaviour Component on the GameObject")]
	public class UnetGetIsServer : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkBehaviour))]
		public FsmOwnerDefault gameObject;

		public FsmEvent isServerEvent;

		public FsmEvent isNotServerEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool isServer;


		public override void Reset()
		{
			gameObject = null;
			isServerEvent = null;
			isNotServerEvent = null;
			isServer = null;

		}

		public override void OnEnter()
		{
			CheckIfServer();


			Finish();

		}

		void CheckIfServer()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go == null) 
			{
				LogError ("Missing GameObject target");
				return;
			}

			NetworkBehaviour _nb = go.GetComponent<NetworkBehaviour>();

			if (_nb==null)
			{
				LogError ("Missing NetworkBehaviour Component");
				return;
			}
				
			UnityEngine.Debug.Log ("Action Is Server? =" + _nb.isServer);
			isServer.Value = _nb.isServer;

			if (_nb.isServer)
			{
				Fsm.Event(isServerEvent);
			}else{
				Fsm.Event(isNotServerEvent);
			}

		}
	}
}

