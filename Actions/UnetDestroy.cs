// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;

using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Destroy the given GameObject on all corresponding GameObjects on all Clients.")]
	public class UnetDestroy : FsmStateAction
	{

		[RequiredField]
		[Tooltip("The target to destroy over the network")]
		public FsmOwnerDefault gameObject;


		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			NetworkServer.Destroy(Fsm.GetOwnerDefaultTarget(gameObject));

			Finish ();
		}

	}
}