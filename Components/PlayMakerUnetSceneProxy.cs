using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

using HutongGames.PlayMaker;

public class PlayMakerUnetSceneProxy : NetworkBehaviour {



	public override void OnStartServer()
	{
		base.OnStartServer ();

		// disable client stuff
	//	PlayMakerFSM.BroadcastEvent("UNET / ON START SERVER");
	}

	public override void OnStartClient()
	{
		base.OnStartClient ();
	//	PlayMakerFSM.BroadcastEvent("UNET / ON START CLIENT");
	}

	public override void OnStartAuthority ()
	{
		base.OnStartClient ();
	}

	public override void OnStopAuthority ()
	{
		base.OnStopAuthority ();
	}

	public override void OnNetworkDestroy ()
	{
		base.OnNetworkDestroy ();
	}

}
