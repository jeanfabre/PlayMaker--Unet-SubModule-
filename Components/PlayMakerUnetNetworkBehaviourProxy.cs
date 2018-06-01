using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Networking
{

	#pragma warning disable CS0414  

	[RequireComponent(typeof(NetworkBehaviour))]
	public class PlayMakerUnetNetworkBehaviourProxy : NetworkBehaviour {

		NetworkTransform _t;
		NetworkIdentity _ni;


		public enum Status {Nothing, IsWriting, IsReading };

		public enum SyncDirection {ServerToClient,ClientToServer};

		public SyncDirection synchDirection;

		public Status CurrentStatus;

		[SerializeField]
		bool debug = false;

		[SerializeField]
		int NetworkChannel = 1;

		[SerializeField]
		private float m_SendInterval = 0.1f;

		public float sendInterval {
			get {
				return this.m_SendInterval;
			}
			set {
				this.m_SendInterval = value;
			}
		}

		void Awake()
		{
			Awake_network ();
		}

		void Start()
		{
			Debug.Log( "Is Server" + isServer,this);

		}


		private float _lastClientToServerSynch = 0f;
		NetworkWriter _nw;

		void Update()
		{
			CheckForDirtyFsmVariables();


			if (!isServer && hasAuthority && observed != null && synchDirection == SyncDirection.ClientToServer) {
				CurrentStatus = Status.IsWriting;

				if (Time.realtimeSinceStartup - _lastClientToServerSynch > m_SendInterval) {
					_lastClientToServerSynch = Time.realtimeSinceStartup;

					Debug.Log ("Synching from Client to Server");
					_nw = new NetworkWriter ();
					PlayMakerUnetUtils.WriteStreamFromFsmVars (this, _nw, observed.Fsm, NetworkSynchVariableLOT, true);
					this.Cmd_SerializeData (_nw.ToArray ());
				}


			} else if (isServer && hasAuthority) {
				

			}



		}

		#region NetworkBehaviour overrides
		public override int GetNetworkChannel ()
		{
			return NetworkChannel;
		}

		public override float GetNetworkSendInterval ()
		{
			return this.m_SendInterval;
		}

			
		#endregion

		#region Network Inter Communication

		[Command]
		public void Cmd_SendEvent(string eventName)
		{
			if (debug) Debug.Log(this.name+ ": Cmd_SendEvent-> "+eventName, this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, eventName);
		}

		[Command]
		public void Cmd_SendEvent_WithStringData(string eventName,string data)
		{
			if (debug) Debug.Log(this.name+ ": Cmd_SendEvent-> "+eventName+" : data:"+data, this);
			Fsm.EventData.StringData = data;
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, eventName);
		}

		[ClientRpc]
		public void Rpc_SendEvent(string eventName)
		{
			if (debug) Debug.Log(this.name+ ": Rpc_SendEvent-> "+eventName, this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, eventName);
		}


		[ClientRpc]
		public void Rpc_SendEvent_WithStringData(string eventName,string data)
		{
			if (debug) Debug.Log(this.name+ ": Rpc_SendEvent-> "+eventName+" : data:"+data, this);
			Fsm.EventData.StringData = data;
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, eventName);
		}


		[Command]
		public void Cmd_SerializeData(byte[] data)
		{
			if (isServer) {
				
				if (debug)
					Debug.Log (this.name + ": Cmd_SerializeData-> ", this);

				CurrentStatus = Status.IsReading;

				//if (debug) Debug.Log ("OnSerialize initialState:" + initialState, this);
				bool missingData = false;
				if (observed != null) {
					PlayMakerUnetUtils.ReadStreamToFsmVars (this, observed.Fsm, NetworkSynchVariableLOT, new NetworkReader(data), out missingData, true);
				}

			}

		}

		#endregion

		#region Variable synchronization

		/// <summary>
		/// The fsm Component being observed. 
		/// We implement the setter to set up FsmVariable as soon as possible, 
		/// else the photonView will miss it and create errors as we start streaming the fsm vars too late ( we have to do it before the Start() )
		/// </summary>
		public PlayMakerFSM observed
		{
			set{
				_observed = value;
				SetUpFsmVariableToSynch();
			}

			get{
				return _observed;
			}
		}

		[SerializeField]
		private PlayMakerFSM _observed;

		/// <summary>
		/// Holds all the variables references to read from and write to during serialization.
		/// </summary>
		public Dictionary<string, NamedVariable> NetworkSynchVariableLOT = new Dictionary<string, NamedVariable>();

		Dictionary<string, object> NetworkSynchValuesLOT = new Dictionary<string, object>();

		/// <summary>
		/// awake network related features
		/// </summary>	
		void Awake_network()
		{
			_ni = this.GetComponent<NetworkIdentity> ();

			if (isServer && observed != null && synchDirection == SyncDirection.ClientToServer) {
				_ni.connectionToClient.RegisterHandler((short)555,synchHandler);
			}

			SetUpFsmVariableToSynch();

		}// Awake


		void synchHandler(NetworkMessage netMsg)
		{
			Debug.Log ("hello");

		}


		void CheckForDirtyFsmVariables()
		{
			SetDirtyBit (0xFFFFFFFF);
		}

		/// <summary>
		/// Sets up fsm variable caching for synch.
		/// It scans the observed Fsm for all variable checked for network synching
		/// It store all these variables in  variableLOT to be iterated during stream read and write.
		/// </summary>
		private void SetUpFsmVariableToSynch()
		{
			if (debug) Debug.Log("SetUpFsmVariableToSynch(): "+_observed, this);

			NetworkSynchVariableLOT = new Dictionary<string, NamedVariable>();

			if (_observed==null)
			{
				return;
			}

			NamedVariable[] _allVars = _observed.FsmVariables.GetAllNamedVariables();

			foreach(NamedVariable fsmNamedVariable in _allVars)
			{
				bool isNetworked = fsmNamedVariable.NetworkSync;

				if (isNetworked)
				{
					NetworkSynchVariableLOT[fsmNamedVariable.Name] = fsmNamedVariable;
					if (debug) Debug.Log("NetworkSynchVariableLOT found "+fsmNamedVariable, this);
				}

			}

		}// SetUpFsmVariableToSynch


		public override bool OnSerialize(NetworkWriter writer, bool forceAll)
		{
			if (synchDirection == SyncDirection.ServerToClient) {
				CurrentStatus = Status.IsWriting;
				//if (debug) Debug.Log ("OnSerialize forceAll:" + forceAll, this);
				if (observed != null) {
					PlayMakerUnetUtils.WriteStreamFromFsmVars (this, writer, observed.Fsm, NetworkSynchVariableLOT, false);
				}
				return true;
			}

			return false;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if (synchDirection == SyncDirection.ServerToClient) {
				CurrentStatus = Status.IsReading;

				//if (debug) Debug.Log ("OnSerialize initialState:" + initialState, this);
				bool missingData = false;
				if (observed != null) {
					PlayMakerUnetUtils.ReadStreamToFsmVars (this, observed.Fsm, NetworkSynchVariableLOT, reader, out missingData, false);
				}
			}
		}

		#endregion


		#region Debug

		void OnGUI()
		{
			if (debug) {
				OnGUI_BeginAreaFollow ();
				GUILayout.TextArea (
				"Player ID: "+this.playerControllerId + "\n" +
				"net ID: "+this.netId + "\n" +
				"Serialization: "+ CurrentStatus.ToString () + "\n" +
				"Is Server: " + isServer.ToString () + "\n" +
				"Is Client: " + isClient.ToString () + "\n" +
				"Is localPlayer: " + isLocalPlayer.ToString () + "\n" +
				"Has Authority: " + hasAuthority.ToString ()
				);
				GUILayout.EndArea ();
			}
		}


		private void OnGUI_BeginAreaFollow()
		{
			var worldPosition = this.transform.position;
		
			// get screen position

			Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

			var left = screenPos.x +30 ;
			var top = screenPos.y;

			var rect = new Rect(left, top, 150, 200);


			// convert screen coordinates
			rect.y = Screen.height - rect.y;

			GUILayout.BeginArea(rect);
		}

		#endregion
	}
}