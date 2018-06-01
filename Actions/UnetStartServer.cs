// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using System;

using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Starts a server. Requires a NetworkManager")]
	public class UnetStartServer : ComponentAction<NetworkManager>
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkManager))]
		[Tooltip("The target. An NetworkManager component is required")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("IP address of the host. Either a dotted IP address or a domain name.")]
		public FsmInt maximumConnections;

		[RequiredField]
		[Tooltip("The port on the remote machine to connect to.")]
		public FsmInt remotePort;

		NetworkManager _networkManager;

		public override void Reset()
		{
			gameObject = null;
			remotePort = 25001;
			maximumConnections = 10;
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

			_networkManager.networkPort= remotePort.Value;
			_networkManager.maxConnections = maximumConnections.Value;
			_networkManager.StartServer();

		}

	}
}