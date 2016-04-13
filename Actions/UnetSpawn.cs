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
		[Tooltip("The target to spawn over the network")]
		public FsmOwnerDefault gameObject;


		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			NetworkServer.Spawn(Fsm.GetOwnerDefaultTarget(gameObject));

			Finish ();
		}

	}
}