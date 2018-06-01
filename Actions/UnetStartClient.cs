// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Starts to a client. Requires a NetworkManager component")]
	public class UnetStartClient : ComponentAction<NetworkManager>
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkManager))]
		[Tooltip("The target. An NetworkManager component is required")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("IP address of the host. Either a dotted IP address or a domain name.")]
		public FsmString remoteIP;

		[RequiredField]
		[Tooltip("The port on the remote machine to connect to.")]
		public FsmInt remotePort;

		NetworkManager _networkManager;

		public override void Reset()
		{
			gameObject = null;
			remoteIP = "127.0.0.1";
			remotePort = 25001;
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

			_networkManager.networkAddress = remoteIP.Value;
			_networkManager.networkPort = remotePort.Value;

			_networkManager.StartClient();

		}

	}
}