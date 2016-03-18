
using System;
using UnityEngine;
using UnityEngine.Networking;

using HutongGames.PlayMaker.Ecosystem.Networking;

using UnityEditor;

namespace HutongGames.PlayMaker.Ecosystem.Networking.Editor
{
	[CustomEditor(typeof(PlayMakerUnetNetworkBehaviourProxy))]
	public class PlayMakerUnetNetworkBehaviourProxyInspector : UnityEditor.Editor
	{
		bool m_Initialized;


		SerializedProperty m_DebugProperty;
		SerializedProperty m_NetworkChannelProperty;
		SerializedProperty m_NetworkSendIntervalProperty;
		SerializedProperty m_ObservedProperty;

		GUIContent m_NetworkSendIntervalLabel;

		GUIContent m_CurrentStatusLabel;

		PlayMakerUnetNetworkBehaviourProxy _target;

		void OnEnable () {
			Init(true);
		}

		//
		// Methods
		//
		public void Init (bool force = false)
		{
			if (this.m_Initialized && !force) {
					return;
			}

			this.m_Initialized = true;

			_target = (this.target as PlayMakerUnetNetworkBehaviourProxy);

			this.m_DebugProperty = base.serializedObject.FindProperty("debug");

			this.m_NetworkChannelProperty = base.serializedObject.FindProperty ("NetworkChannel");

			this.m_NetworkSendIntervalProperty = base.serializedObject.FindProperty ("m_SendInterval");
			this.m_NetworkSendIntervalLabel = new GUIContent ("Send Rate/seconds", "Number of network updates per second");

			this.m_ObservedProperty = base.serializedObject.FindProperty ("_observed");

			if (Application.isEditor) {
				this.m_CurrentStatusLabel = new GUIContent ("Current Status", "What it's currently doing");
			}
		}

		public override void OnInspectorGUI ()
		{
			this.ShowControls();
		}

		protected void ShowControls ()
		{
			this.Init ();

			base.serializedObject.Update ();

			// m_NetworkSendIntervalProperty
			int num = 0;
			if (this.m_NetworkSendIntervalProperty.floatValue != 0) {
				num = (int)(1 / this.m_NetworkSendIntervalProperty.floatValue);
			}
			int num2 = EditorGUILayout.IntSlider (this.m_NetworkSendIntervalLabel, num, 0, 30, new GUILayoutOption[0]);
			if (num2 != num) {
				if (num2 == 0) {
					this.m_NetworkSendIntervalProperty.floatValue = 0;
				}
				else {
					this.m_NetworkSendIntervalProperty.floatValue = 1 / (float)num2;
				}
			}

			EditorGUILayout.PropertyField (this.m_ObservedProperty);

			EditorGUILayout.PropertyField (this.m_NetworkChannelProperty);

			EditorGUILayout.PropertyField(this.m_DebugProperty);

			base.serializedObject.ApplyModifiedProperties ();

			// live feedback when playing
	
			if (Application.isPlaying && PrefabUtility.GetPrefabType(_target) != PrefabType.Prefab ) {
				EditorGUILayout.LabelField(this.m_CurrentStatusLabel.text, _target.CurrentStatus.ToString());
				EditorGUILayout.LabelField("Is Server", _target.isServer.ToString());
				EditorGUILayout.LabelField("Is Client", _target.isClient.ToString());
				EditorGUILayout.LabelField("Is localPlayer", _target.isLocalPlayer.ToString());
				EditorGUILayout.LabelField("Has Authority", _target.hasAuthority.ToString());
			}

		}
	}
}
