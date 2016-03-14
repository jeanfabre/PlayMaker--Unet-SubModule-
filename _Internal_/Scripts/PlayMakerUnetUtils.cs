// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using HutongGames.PlayMaker;

public class PlayMakerUnetUtils {


	public static NetworkWriter WriteStreamFromFsmVars(NetworkBehaviour source,NetworkWriter writer,Fsm fromFsm,Dictionary<string, FsmVar> vars,bool debug)
	{

		if (fromFsm==null)
		{
			if (debug) Debug.Log("FromFsm is null, nothing will be writing in the stream : ",source);
			return writer;
		}
			
		writer.Write(vars.Count);

		foreach(KeyValuePair<string, FsmVar> entry in vars)
		{
			if (debug) Debug.Log("Writing entry key : " +entry.Key);

			writer.Write(entry.Key);

			FsmVar fsmVar = entry.Value;
			if (fsmVar.useVariable)
			{
				string _name = fsmVar.variableName;

				if (debug) Debug.Log("Writing entry var ( using variable )" +_name+" of type "+fsmVar.Type);

				switch (fsmVar.Type){
				case VariableType.Int:
					writer.Write(Convert.ToInt32( PlayMakerUtils.GetValueFromFsmVar(fromFsm,fsmVar) ))    ; //  fromFsm.Variables.GetFsmInt(_name).Value));
					break;
				case VariableType.Float:
					writer.Write((float)PlayMakerUtils.GetValueFromFsmVar(fromFsm,fsmVar));//fromFsm.Variables.GetFsmFloat(_name).Value);
					break;
				case VariableType.Bool:
					writer.Write(fromFsm.Variables.GetFsmBool(_name).Value);
					break;
				case VariableType.Color:

					writer.Write( fromFsm.Variables.GetFsmColor(_name).Value);
					break;
				case VariableType.Quaternion:
					writer.Write( fromFsm.Variables.GetFsmQuaternion(_name).Value);
					break;
				case VariableType.Rect:
					Rect _rect = fromFsm.Variables.GetFsmRect(_name).Value;
					writer.Write(new Vector4(_rect.x,_rect.y,_rect.width,_rect.height));
					break;
				case VariableType.Vector2:
					writer.Write( fromFsm.Variables.GetFsmVector2(_name).Value);
					break;
				case VariableType.Vector3:
					writer.Write((Vector3)PlayMakerUtils.GetValueFromFsmVar(fromFsm,fsmVar));//fromFsm.Variables.GetFsmVector3(_name).Value);
					break;
				case VariableType.Texture:
					// we must write it ( or not add the key)
				//	writer.Write<Texture>(fromFsm.Variables.GetFsmTexture(_name).Value);
					break;
				case VariableType.Material:
					// we must write it ( or not add the key)
				//	writer.Write<Material>( fromFsm.Variables.GetFsmMaterial(_name).Value);
					break;
				case VariableType.String:
					writer.Write( fromFsm.Variables.GetFsmString(_name).Value);
					break;
				case VariableType.GameObject:
					writer.Write(fromFsm.Variables.GetFsmGameObject (_name).Value);
					break;
				case VariableType.Object:
					// we must write it ( or not add the key)
				//	writer.Write<UnityEngine.Object>( fromFsm.Variables.GetFsmObject(_name).Value,null);
					break;
				}

			}else{



				switch (fsmVar.Type){
				case VariableType.Int:
					writer.Write(Convert.ToInt32(fsmVar.intValue));
					break;
				case VariableType.Float:
					if (debug) Debug.Log("Writing entry var " +fsmVar.variableName+" of type "+fsmVar.Type+" = "+fsmVar.floatValue);
					float _valFloat = fsmVar.floatValue;
					writer.Write(fromFsm.Variables.GetFsmFloat(fsmVar.variableName).Value);
					break;
				case VariableType.Bool:
					writer.Write(fsmVar.boolValue);
					break;
				case VariableType.Color:
					if (debug) UnityEngine.Debug.Log("Fsmvar value " +fromFsm.Variables.GetFsmColor(fsmVar.variableName).Value);
					writer.Write(fromFsm.Variables.GetFsmColor(fsmVar.variableName).Value);
					break;
				case VariableType.Quaternion:
					writer.Write(fsmVar.quaternionValue);
					break;
				case VariableType.Rect:
					Rect _rect = fsmVar.rectValue;
					writer.Write(new Vector4(_rect.x,_rect.y,_rect.width,_rect.height));
					break;
				case VariableType.Vector2:
					writer.Write(fsmVar.vector2Value);
					break;
				case VariableType.Vector3:
					Vector3 _var = (Vector3)PlayMakerUtils.GetValueFromFsmVar(fromFsm,fsmVar);
					if (debug) UnityEngine.Debug.Log("Fsmvar value " +_var);
					writer.Write(_var);//fsmVar.vector3Value);
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
					writer.Write(fsmVar.stringValue);
					break;
				case VariableType.GameObject:
					GameObject _goVar =  fsmVar.gameObjectValue;
					writer.Write(fromFsm.Variables.GetFsmGameObject(fsmVar.variableName).Value);
					break;

				case VariableType.Object:
					writer.Write (false); // we must write it ( or not add the key)
				//	writer.Write<UnityEngine.Object>(fsmVar.objectReference,null);
					break;
				}
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
	public static bool ReadStreamToFsmVars(NetworkBehaviour source,Fsm toFsm,Dictionary<string, FsmVar> vars, NetworkReader reader,out bool missingData,bool debug)
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
					FsmVar _fsmVar = vars[_key];

					if (debug) Debug.Log("Variable for key of type "+_fsmVar.Type,source);

					_processedKey.Add (_key);

					switch (_fsmVar.Type)
					{
					case VariableType.Int:
						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,reader.ReadInt32());
						break;
					case VariableType.Float:
						float _varFloat = reader.ReadSingle();
						//if (debug) Debug.Log("Variable float value = "+_varFloat,source);
						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,_varFloat);
						break;
					case VariableType.Bool:
						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,reader.ReadBoolean());
						break;
					case VariableType.Color:
						Color _c  = reader.ReadColor();

						if (debug) UnityEngine.Debug.Log("Fsmvar value " +_c);
						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,_c);
						break;
					case VariableType.Quaternion:
						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,reader.ReadQuaternion());
						break;
					case VariableType.Rect:
						Vector4 _rectRaw = reader.ReadVector4();
						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,new Rect(_rectRaw[0],_rectRaw[1],_rectRaw[2],_rectRaw[3]));
						break;
					case VariableType.Vector2:
						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,reader.ReadVector2());
						break;
					case VariableType.Vector3:
						Vector3 _var = reader.ReadVector3();
						//	if (debug) Debug.Log("Variable value = "+_var,source);

						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,_var);

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
						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,reader.ReadString());
						break;
					case VariableType.GameObject:
						
						//UnityEngine.Debug.Log("Found NetworkViewID owner "+_nv.gameObject.name);
						PlayMakerUtils.ApplyValueToFsmVar(toFsm,_fsmVar,reader.ReadGameObject());
							
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
