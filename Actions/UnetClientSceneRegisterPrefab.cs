// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("")]
	public class UnetClientSceneRegisterPrefab : FsmStateAction
	{
		[Tooltip("The prefab to register")]
		public FsmGameObject prefab;

		public override void Reset()
		{
			prefab = null;
		}

		public override void OnEnter()
		{
			if (prefab.Value != null) {
				UnityEngine.Debug.Log("ClientScene.RegisterPrefab ("+prefab.Value.name+")");
				ClientScene.RegisterPrefab (prefab.Value);
			}

			Finish();

		}
	}
}