// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

using UnityEngine.Networking;

using HutongGames.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Actions
{
	[ActionCategory("Unity Networking")]
	[Tooltip("Used to set the behaviour as dirty, so that a network update will be sent for the object. Requires a NetworkBehaviour Component on the GameObject")]
	public class UnetSetDirtyBit : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkBehaviour))]
		[Tooltip("GameObject target. Will pick the first NetworkBehaviour attached. use 'NetworkBehaviour' property to target a specific NetworkBehaviour")]
		public FsmOwnerDefault gameObject;

		[Tooltip("If more than one NetworkBehaviour is on the GameObject, you can target precisly the one you want.")]
		[ObjectType(typeof(NetworkBehaviour))]
		public FsmObject networkBehaviour;


		[RequiredField]
		[Tooltip("Bit mask to set.")]
		public FsmInt dirtyBit;


		NetworkBehaviour _nb;

		public override void Reset()
		{
			gameObject = null;

			networkBehaviour = new FsmObject() {UseVariable= true};

			dirtyBit = null;

		}

		public override void OnEnter()
		{
			setDirty();


			Finish();

		}

		void setDirty()
		{
			if (!networkBehaviour.IsNone) {
				_nb = networkBehaviour.Value as NetworkBehaviour;
			}

			if (_nb ==null)
			{
				GameObject go = Fsm.GetOwnerDefaultTarget (gameObject);
				if (go != null) {
					_nb = go.GetComponent<NetworkBehaviour> ();
				}
			}

			if (_nb==null)
			{
				return;
			}

			_nb.SetDirtyBit ((uint)dirtyBit.Value);
		}
	}
}

