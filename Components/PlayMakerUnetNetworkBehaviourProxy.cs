using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Networking
{
	[RequireComponent(typeof(NetworkBehaviour))]
	public class PlayMakerUnetNetworkBehaviourProxy : NetworkBehaviour {

		NetworkTransform _t;

		public enum Status {Nothing, IsWriting, IsReading };

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

		void Update()
		{
			CheckForDirtyFsmVariables();

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

		#region NetworkBehavior Overrides

		public override void OnStartServer()
		{
			base.OnStartServer ();

			if (debug) Debug.Log(this.name+ ": OnStartServer ", this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, "UNET / ON START SERVER");
		}

		public override void OnStartAuthority ()
		{
			base.OnStartAuthority ();

			if (debug) Debug.Log(this.name+ ": OnStartAuthority ", this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, "UNET / ON START AUTHORITY");
		}

		public override void OnStartClient()
		{
			base.OnStartClient (); 

			if (debug) Debug.Log(this.name+ ": OnStartClient ", this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, "UNET / ON START CLIENT");
		}

		#endregion

		#region Network Inter Communication

		[Command]
		public void Cmd_SendEvent(string eventName)
		{
			if (debug) Debug.Log(this.name+ ": Cmd_SendEvent-> "+eventName, this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, eventName);
		}

		[ClientRpc]
		public void Rpc_SendEvent(string eventName)
		{
			if (debug) Debug.Log(this.name+ ": Rpc_SendEvent-> "+eventName, this);
			PlayMakerUtils.SendEventToGameObject(null, this.gameObject, eventName);
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
			SetUpFsmVariableToSynch();

		}// Awake


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
				Debug.LogError ("SetUpFsmVariableToSynch(): No Observed PlayMaker", this);
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
			CurrentStatus = Status.IsWriting;
			//if (debug) Debug.Log ("OnSerialize forceAll:" + forceAll, this);
			PlayMakerUnetUtils.WriteStreamFromFsmVars(this,writer, observed.Fsm, NetworkSynchVariableLOT, false);

			return true;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			CurrentStatus = Status.IsReading;

			//if (debug) Debug.Log ("OnSerialize initialState:" + initialState, this);
			bool missingData = false;

			PlayMakerUnetUtils.ReadStreamToFsmVars(this,observed.Fsm,NetworkSynchVariableLOT,reader,out missingData, false);

		}

		#endregion

	}
}