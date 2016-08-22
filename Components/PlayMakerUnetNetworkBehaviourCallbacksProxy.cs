using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Networking
{

	/// <summary>
	/// Play maker unet network behaviour callbacks proxy.
	/// 
	/// TODO: maybe create that component at runtime if not found and GameObject is listening to some of these events.
	/// TODO: prevent duplicate drops on the same gameobject or hierarchy?
	/// </summary>
	[RequireComponent(typeof(NetworkBehaviour))]
	public class PlayMakerUnetNetworkBehaviourCallbacksProxy : NetworkBehaviour {



		[SerializeField]
		bool debug = false;

		#region NetworkBehaviour Overrides

		public override void OnStartServer()
		{
			base.OnStartServer ();

			if (debug) Debug.Log(this.name+ ": OnStartServer ", this);

			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, "UNET / ON START SERVER");
		}

		public override void OnStartAuthority ()
		{
			base.OnStartAuthority ();

			if (debug) Debug.Log(this.name+ ": OnStartAuthority", this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, "UNET / ON START AUTHORITY");
		}

		public override void OnStartClient()
		{
			base.OnStartClient (); 

			if (debug) Debug.Log(this.name+ ": OnStartClient", this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, "UNET / ON START CLIENT");
		}

		public override void OnStartLocalPlayer ()
		{
			base.OnStartLocalPlayer ();

			if (debug) Debug.Log(this.name+ ": OnStartClient isLocalPlayer:"+isLocalPlayer, this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, "UNET / ON START LOCAL PLAYER");
		}


		public override void OnNetworkDestroy()
		{
			base.OnNetworkDestroy (); 

			if (debug) Debug.Log(this.name+ ": OnNetworkDestroy ", this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, "UNET / ON NETWORK DESTROY");
		}

		#endregion

	}
}