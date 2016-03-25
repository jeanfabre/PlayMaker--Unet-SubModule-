// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using HutongGames.PlayMaker;

public class PlayMakerUnetUtils {


	public static NetworkWriter WriteStreamFromFsmVars(NetworkBehaviour source,NetworkWriter writer,Fsm fromFsm,Dictionary<string, NamedVariable> vars,bool debug)
	{
		

		if (fromFsm==null)
		{
			if (debug) Debug.Log("FromFsm is null, nothing will be writing in the stream : ",source);
			return writer;
		}
			
		writer.Write(vars.Count);

		string variableName;

		foreach(KeyValuePair<string, NamedVariable> entry in vars)
		{
			if (debug) Debug.Log("Writing entry key : " +entry.Key);

			writer.Write(entry.Key);

			variableName = entry.Value.Name;

			switch (entry.Value.VariableType){
				case VariableType.Int:
					writer.Write(fromFsm.Variables.GetFsmInt(variableName).Value);
					break;
				case VariableType.Float:
				if (debug) Debug.Log("Writing entry var " +variableName+" of type "+entry.Value.VariableType+" = "+fromFsm.Variables.GetFsmFloat(variableName).Value);
					
					writer.Write(fromFsm.Variables.GetFsmFloat(variableName).Value);
					break;
				case VariableType.Bool:
					writer.Write(fromFsm.Variables.GetFsmBool(variableName).Value);
					break;
				case VariableType.Color:
					if (debug) UnityEngine.Debug.Log("Fsmvar value " +fromFsm.Variables.GetFsmColor(variableName).Value);
					writer.Write(fromFsm.Variables.GetFsmColor(variableName).Value);
					break;
				case VariableType.Quaternion:
					writer.Write(fromFsm.Variables.GetFsmQuaternion(variableName).Value);
					break;
				case VariableType.Rect:
					writer.Write(fromFsm.Variables.GetFsmInt(variableName).Value);
					break;
				case VariableType.Vector2:
					writer.Write(fromFsm.Variables.GetFsmVector2(variableName).Value);
					break;
				case VariableType.Vector3:
					writer.Write(fromFsm.Variables.GetFsmVector3(variableName).Value);
					break;
				case VariableType.Texture:
					writer.Write (false); // we must write it ( or not add the key)
					//writer.Write<Texture>(fsmVar.textureValue);
					break;
				case VariableType.Material:
					writer.Write (false); // we must write it ( or not add the key)
					//writer.Write<Material>(fsmVar.materialValue);
					break;
				case VariableType.String:
					writer.Write(fromFsm.Variables.GetFsmString(variableName).Value);
					break;
				case VariableType.GameObject:
					writer.Write(fromFsm.Variables.GetFsmGameObject(variableName).Value);
					break;

				case VariableType.Object:
					writer.Write (false); // we must write it ( or not add the key)
				//	writer.Write<UnityEngine.Object>(fsmVar.objectReference,null);
					break;
			}
		}


		return writer;
	}

	/// <summary>
	/// Reads the stream to fsm variables. The stream is composed with a string for the key followed by the related value.
	/// </summary>
	/// <returns><c>true</c> if all went fine.</returns>
	/// <param name="toFsm">To fsm.</param>
	/// <param name="vars">The key/Fsm variables</param>
	/// <param name="reader"> The NetworkReader</param>
	/// <param name="missingData"> if stream is missing datas, <c>false</c> otherwise which means all expected data was found</param>
	public static bool ReadStreamToFsmVars(NetworkBehaviour source,Fsm toFsm,Dictionary<string, NamedVariable> vars, NetworkReader reader,out bool missingData,bool debug)
	{
		missingData=false;

		if (toFsm==null)
		{
			Debug.LogError("fromFsm is null",source);
			return false;
		}

		// meta info to double check all expected data was processed

		List<string> _processedKey = new List<string>();


		if (debug) Debug.Log("reading stream",source);

		try{

			int _pairCount = reader.ReadInt32();

			if (debug) Debug.Log("reading "+_pairCount+" Key/value pairs",source);

			for(int i=0; i<_pairCount;i++)
			{	

				string _key = reader.ReadString();


				if (debug) Debug.Log("reading for key "+_key,source);

				if (string.IsNullOrEmpty(_key))
				{
					reader.ReadBoolean();

					if (debug) Debug.LogWarning("Found null or empty Stream key property on "+toFsm.Owner.name,source);
				}else if (! vars.ContainsKey(_key))
				{
					reader.ReadBoolean();

					if (debug) Debug.LogWarning("Stream property <"+_key+"> not found as a Fsmvariable on "+toFsm.Owner.name,source);
					missingData = true;

				}else{
					NamedVariable _NamedVariable = vars[_key];

					if (debug) Debug.Log("Variable for key of type "+_NamedVariable.VariableType,source);

					_processedKey.Add (_key);

					string _variableName = _NamedVariable.Name;
					switch (_NamedVariable.VariableType)
					{
					case VariableType.Int:
						toFsm.Variables.GetFsmInt(_variableName).Value = reader.ReadInt32();
						break;
					case VariableType.Float:
						toFsm.Variables.GetFsmFloat(_variableName).Value = reader.ReadSingle();
						break;
					case VariableType.Bool:
						toFsm.Variables.GetFsmBool(_variableName).Value = reader.ReadBoolean();
						break;
					case VariableType.Color:
						toFsm.Variables.GetFsmColor(_variableName).Value = reader.ReadColor();
						break;
					case VariableType.Quaternion:
						toFsm.Variables.GetFsmQuaternion(_variableName).Value =reader.ReadQuaternion();
						break;
					case VariableType.Rect:
						toFsm.Variables.GetFsmRect(_variableName).Value = reader.ReadRect();
						break;
					case VariableType.Vector2:
						toFsm.Variables.GetFsmVector2(_variableName).Value = reader.ReadVector2();
						break;
					case VariableType.Vector3:
						toFsm.Variables.GetFsmVector2(_variableName).Value = reader.ReadVector3();
						break;
					case VariableType.Texture:
						reader.ReadBoolean(); // we must read it however to keep moving forward
						//UnityEngine.Debug.LogWarning("Texture not supported in initial data");
						break;
					case VariableType.Material:
						reader.ReadBoolean(); // we must read it however to keep moving forward
					//	UnityEngine.Debug.LogWarning("Material not supported in initial data");
						break;
					case VariableType.String:
						toFsm.Variables.GetFsmString(_variableName).Value = reader.ReadString();
						break;
					case VariableType.GameObject:
						toFsm.Variables.GetFsmGameObject(_variableName).Value = reader.ReadGameObject();
						break;
					case VariableType.Object:
						reader.ReadBoolean();
						//Debug.LogWarning("Object not supported in initial data",source);
						break;
					}


				}

			}

			if (vars.Count>_processedKey.Count)
			{
				string[] _unprocessedKeys = new string[vars.Count];
				//vars.Keys.CopyTo(_unprocessedKeys,0);

				UnityEngine.Debug.LogWarning("Stream data missing properties <"+String.Join(",",_unprocessedKeys)+"> on "+toFsm.Owner.name);
				missingData = true;
			}

			return true;

		}catch(System.Exception e)
		{
			UnityEngine.Debug.LogWarning("Stream data stream error "+e.Message);
		}


		return false;
	}


}
