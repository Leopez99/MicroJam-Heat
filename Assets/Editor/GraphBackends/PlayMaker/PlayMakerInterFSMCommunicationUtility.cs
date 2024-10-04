using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using System.Linq;
using UnityEditor;

namespace RelationsInspector.Backend.PlayMaker
{
	// information gathered from the sending action alone. missing receiver state and transition.
	public class EventActionInfo
	{
		public FsmEventTarget target;
		public FsmInfo senderInfo;
	}

	public class GetActionParams
	{
		public System.Func<object, string> getEventName;
		public System.Func<object, FsmEventTarget> getEventTarget;
	}

	public class VariableAccessInfo
	{
		public LinkType linkType;
		public string variableName;
		public FsmInfo accessor;
		public FsmInfo owner;
	}

	public enum LinkType { SendEvent, GetVariable, SetVariable }

	// full information about sender action and receiver transition
	public class FsmLink
	{
		public LinkType linkType;
		public string variableName;
		public FsmInfo senderInfo;
		public FsmInfo receiverInfo;
	}

	public class ExtractVarAccessData
	{
		public System.Func<object, FsmOwnerDefault> getVarOwner;
		public System.Func<object, string> getVarFsmName;
		public System.Func<object, string> getVarName;
		public System.Func<LinkType> getLinkType;
	}

	public static class Utility
	{
		public static Dictionary<System.Type, GetActionParams> actionParamExtraction = new Dictionary<System.Type, GetActionParams>()
		{
			{
				typeof(SendEvent),
				new GetActionParams()
				{
					getEventName = obj => (obj as SendEvent).sendEvent.Name,
					getEventTarget = obj => (obj as SendEvent).eventTarget
				}
			},
			{
				typeof(SendEventByName),
				new GetActionParams()
				{
					getEventName = obj => (obj as SendEventByName).sendEvent.Value,
					getEventTarget = obj => (obj as SendEventByName).eventTarget
				}
			}
		};

		public static Dictionary<LinkType, ColorLegendEntry> linkTypeData = new Dictionary<LinkType, ColorLegendEntry>()
		{
			{ LinkType.SendEvent, new ColorLegendEntry() { color = Color.white, text = "Sending event" } },
			{ LinkType.SetVariable, new ColorLegendEntry() { color = Color.green, text = "Setting variable" } },
			{ LinkType.GetVariable, new ColorLegendEntry() { color = Color.red, text = "Getting variable" } }
		};

		public static Dictionary<System.Type, ExtractVarAccessData> variableAccessActions = new Dictionary<System.Type, ExtractVarAccessData>()
		{
			#region variable setters
			{
				typeof(SetFsmVariable),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmVariable).gameObject,
					getVarFsmName = o => (o as SetFsmVariable).fsmName.Value,
					getVarName = o => (o as SetFsmVariable).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmInt),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmInt).gameObject,
					getVarFsmName = o => (o as SetFsmInt).fsmName.Value,
					getVarName = o => (o as SetFsmInt).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmFloat),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmFloat).gameObject,
					getVarFsmName = o => (o as SetFsmFloat).fsmName.Value,
					getVarName = o => (o as SetFsmFloat).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmBool),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmBool).gameObject,
					getVarFsmName = o => (o as SetFsmBool).fsmName.Value,
					getVarName = o => (o as SetFsmBool).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmColor),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmColor).gameObject,
					getVarFsmName = o => (o as SetFsmColor).fsmName.Value,
					getVarName = o => (o as SetFsmColor).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmEnum),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmEnum).gameObject,
					getVarFsmName = o => (o as SetFsmEnum).fsmName.Value,
					getVarName = o => (o as SetFsmEnum).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmGameObject),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmGameObject).gameObject,
					getVarFsmName = o => (o as SetFsmGameObject).fsmName.Value,
					getVarName = o => (o as SetFsmGameObject).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmMaterial),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmMaterial).gameObject,
					getVarFsmName = o => (o as SetFsmMaterial).fsmName.Value,
					getVarName = o => (o as SetFsmMaterial).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmObject),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmObject).gameObject,
					getVarFsmName = o => (o as SetFsmObject).fsmName.Value,
					getVarName = o => (o as SetFsmObject).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmQuaternion),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmQuaternion).gameObject,
					getVarFsmName = o => (o as SetFsmQuaternion).fsmName.Value,
					getVarName = o => (o as SetFsmQuaternion).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmRect),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmRect).gameObject,
					getVarFsmName = o => (o as SetFsmRect).fsmName.Value,
					getVarName = o => (o as SetFsmRect).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmString),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmString).gameObject,
					getVarFsmName = o => (o as SetFsmString).fsmName.Value,
					getVarName = o => (o as SetFsmString).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmTexture),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmTexture).gameObject,
					getVarFsmName = o => (o as SetFsmTexture).fsmName.Value,
					getVarName = o => (o as SetFsmTexture).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmVector2),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmVector2).gameObject,
					getVarFsmName = o => (o as SetFsmVector2).fsmName.Value,
					getVarName = o => (o as SetFsmVector2).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			{
				typeof(SetFsmVector3),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as SetFsmVector3).gameObject,
					getVarFsmName = o => (o as SetFsmVector3).fsmName.Value,
					getVarName = o => (o as SetFsmVector3).variableName.Value,
					getLinkType = () => LinkType.SetVariable
				}
			},
			#endregion

			#region variable getters
			{
				typeof(GetFsmVariable),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmVariable).gameObject,
					getVarFsmName = o => (o as GetFsmVariable).fsmName.Value,
					getVarName = o => (o as GetFsmVariable).storeValue.variableName,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmBool),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmBool).gameObject,
					getVarFsmName = o => (o as GetFsmBool).fsmName.Value,
					getVarName = o => (o as GetFsmBool).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmColor),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmColor).gameObject,
					getVarFsmName = o => (o as GetFsmColor).fsmName.Value,
					getVarName = o => (o as GetFsmColor).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmEnum),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmEnum).gameObject,
					getVarFsmName = o => (o as GetFsmEnum).fsmName.Value,
					getVarName = o => (o as GetFsmEnum).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmFloat),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmFloat).gameObject,
					getVarFsmName = o => (o as GetFsmFloat).fsmName.Value,
					getVarName = o => (o as GetFsmFloat).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmGameObject),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmGameObject).gameObject,
					getVarFsmName = o => (o as GetFsmGameObject).fsmName.Value,
					getVarName = o => (o as GetFsmGameObject).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmInt),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmInt).gameObject,
					getVarFsmName = o => (o as GetFsmInt).fsmName.Value,
					getVarName = o => (o as GetFsmInt).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmMaterial),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmMaterial).gameObject,
					getVarFsmName = o => (o as GetFsmMaterial).fsmName.Value,
					getVarName = o => (o as GetFsmMaterial).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmObject),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmObject).gameObject,
					getVarFsmName = o => (o as GetFsmObject).fsmName.Value,
					getVarName = o => (o as GetFsmObject).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmQuaternion),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmQuaternion).gameObject,
					getVarFsmName = o => (o as GetFsmQuaternion).fsmName.Value,
					getVarName = o => (o as GetFsmQuaternion).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmRect),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmRect).gameObject,
					getVarFsmName = o => (o as GetFsmRect).fsmName.Value,
					getVarName = o => (o as GetFsmRect).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmString),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmString).gameObject,
					getVarFsmName = o => (o as GetFsmString).fsmName.Value,
					getVarName = o => (o as GetFsmString).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmTexture),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmTexture).gameObject,
					getVarFsmName = o => (o as GetFsmTexture).fsmName.Value,
					getVarName = o => (o as GetFsmTexture).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmVector2),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmVector2).gameObject,
					getVarFsmName = o => (o as GetFsmVector2).fsmName.Value,
					getVarName = o => (o as GetFsmVector2).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			},
			{
				typeof(GetFsmVector3),
				new ExtractVarAccessData()
				{
					getVarOwner = o => (o as GetFsmVector3).gameObject,
					getVarFsmName = o => (o as GetFsmVector3).fsmName.Value,
					getVarName = o => (o as GetFsmVector3).variableName.Value,
					getLinkType = () => LinkType.GetVariable
				}
			}
			#endregion
		};

		public static IEnumerable<Relation<PlayMakerFSM, FsmLink>> GetCommunicationRelations( PlayMakerFSM entity, bool showEvents, bool showVariables )
		{
			if ( showEvents )
			{
				foreach ( var relation in GetEventRelations( entity ) )
					yield return relation;
			}

			if ( showVariables )
			{
				foreach ( var relation in GetVariableRelations( entity ) )
					yield return relation;
			}
		}

		public static IEnumerable<Relation<PlayMakerFSM, FsmLink>> GetEventRelations( PlayMakerFSM entity )
		{
			var eventInfos = entity.Fsm.States.SelectMany( state => GetEventInfos( state ) );

			foreach ( var eventInfo in eventInfos )
			{
				if ( string.IsNullOrEmpty( eventInfo.senderInfo.eventName ) )
					continue;

				foreach ( var receiverInfo in GetEventReceivers( eventInfo, entity ) )
				{
					receiverInfo.fsmComponent = receiverInfo.fsm.Owner as PlayMakerFSM;
					if ( receiverInfo.fsmComponent == entity )
						continue;

					var link = new FsmLink() { senderInfo = eventInfo.senderInfo, receiverInfo = receiverInfo, linkType = LinkType.SendEvent };

					yield return new Relation<PlayMakerFSM, FsmLink>( entity, receiverInfo.fsmComponent, link );
				}
			}
		}

		public static IEnumerable<Relation<PlayMakerFSM, FsmLink>> GetVariableRelations( PlayMakerFSM entity )
		{
			var variableInfos = entity.Fsm.States.SelectMany( state => GetForeignVariableAccessInfos( state ) );
			foreach ( var variableInfo in variableInfos )
			{

				variableInfo.owner.fsmComponent = variableInfo.owner.fsm.Owner as PlayMakerFSM;
				if ( variableInfo.owner.fsmComponent == entity )
					continue;

				var link = new FsmLink()
				{
					senderInfo = variableInfo.accessor,
					receiverInfo = variableInfo.owner,
					linkType = variableInfo.linkType,
					variableName = variableInfo.variableName
				};

				yield return new Relation<PlayMakerFSM, FsmLink>( entity, variableInfo.owner.fsmComponent, link );
			}
		}

		public static object[] GetSceneTargets( bool showEvents, bool showVariables, bool hideUnconnected )
		{
			FsmEditor.RebuildFsmList();
			var sceneFSMs = GetAllFSMs()
				.Where( pmFSM => !IsPrefab( pmFSM.gameObject ) );

			var sceneTargets = hideUnconnected ? sceneFSMs.Where( pmFSM => GetCommunicationRelations( pmFSM, showEvents, showVariables ).Any() ) : sceneFSMs;
			return sceneTargets.ToArray();
		}

		// find FSMs that have a transition using the given event
		public static IEnumerable<FsmInfo> GetEventReceivers( EventActionInfo eventInfo, PlayMakerFSM sender )
		{
			switch ( eventInfo.target.target )
			{
				case FsmEventTarget.EventTarget.GameObject:
				case FsmEventTarget.EventTarget.GameObjectFSM:
					return GetGameObjectEventReceivers( eventInfo.target.gameObject, eventInfo );

				case FsmEventTarget.EventTarget.BroadcastAll:
					return GetAllFSMs().SelectMany( pmFSM => FsmInfo.FindTransitionsUsingEvent( pmFSM.Fsm, eventInfo.senderInfo.eventName ) );

				case FsmEventTarget.EventTarget.FSMComponent:
					return ( eventInfo.target.fsmComponent == null ) ?
						Enumerable.Empty<FsmInfo>() :
						FsmInfo.FindTransitionsUsingEvent( eventInfo.target.fsmComponent.Fsm, eventInfo.senderInfo.eventName );

				case FsmEventTarget.EventTarget.Self:
				case FsmEventTarget.EventTarget.HostFSM:
				case FsmEventTarget.EventTarget.SubFSMs:
					break;
			}
			return Enumerable.Empty<FsmInfo>();
		}

		// find those among gameObject's FSMs that have a transition using the given event.
		public static IEnumerable<FsmInfo> GetGameObjectEventReceivers( FsmOwnerDefault gameObject, EventActionInfo eventInfo )
		{
			switch ( eventInfo.target.gameObject.OwnerOption )
			{
				case OwnerDefaultOption.SpecifyGameObject:
					return GetGameObjectEventReceivers( gameObject.GameObject.Value, eventInfo );

				case OwnerDefaultOption.UseOwner:
				default:
					return Enumerable.Empty<FsmInfo>();    // ignore fsm-internal event
			}
		}

		// find those among gameObject's FSMs that that have a transition using the given event.
		public static IEnumerable<FsmInfo> GetGameObjectEventReceivers( GameObject gameObject, EventActionInfo eventInfo )
		{
			if ( gameObject == null )
				return Enumerable.Empty<FsmInfo>();

			var fsmComponents = gameObject.GetComponents<PlayMakerFSM>();

			var matchingComponents = string.IsNullOrEmpty( eventInfo.target.fsmName.Value ) ?
				fsmComponents :
				fsmComponents.Where( c => c.FsmName == eventInfo.target.fsmName.Value );

			return matchingComponents.SelectMany( pmFSM => FsmInfo.FindTransitionsUsingEvent( pmFSM.Fsm, eventInfo.senderInfo.eventName ) );
		}

		public static IEnumerable<EventActionInfo> GetEventInfos( FsmState fsmState )
		{
			return fsmState.Actions.SelectMany( action => GetEventInfos( action, fsmState ) );
		}

		public static IEnumerable<VariableAccessInfo> GetForeignVariableAccessInfos( FsmState fsmState )
		{
			return fsmState.Actions
				.Select( action => GetForeignVariableAccessInfos( action, fsmState ) )
				.Where( info => info != null );
		}

		// find the events sent by the given action of the given state
		public static IEnumerable<EventActionInfo> GetEventInfos( FsmStateAction action, FsmState state )
		{
			var actionType = action.GetType();
			if ( !actionParamExtraction.ContainsKey( actionType ) )
				yield break;

			action.Init( state );
			string eventName = actionParamExtraction[ actionType ].getEventName( action );
			var eventTarget = actionParamExtraction[ actionType ].getEventTarget( action );

			var senderInfo = new FsmInfo();
			senderInfo.action = action;
			senderInfo.state = state;
			senderInfo.fsm = state.Fsm;
			senderInfo.eventName = eventName;

			yield return new EventActionInfo()
			{
				target = eventTarget,
				senderInfo = senderInfo
			};
		}

		public static VariableAccessInfo GetForeignVariableAccessInfos( FsmStateAction action, FsmState state )
		{
			var actionType = action.GetType();
			ExtractVarAccessData extractor;

			if ( !Utility.variableAccessActions.TryGetValue( actionType, out extractor ) )
				return null;

			var variableOwnerGO = extractor.getVarOwner( action );
			var variableOwnerFsmName = extractor.getVarFsmName( action );
			var variableName = extractor.getVarName( action );
			var variableLinkType = extractor.getLinkType();

			return GetVariableAccessInfo( state, action, variableOwnerGO, variableOwnerFsmName, variableName, variableLinkType );
		}

		public static VariableAccessInfo GetVariableAccessInfo( FsmState state, FsmStateAction action, FsmOwnerDefault varOwner, string ownerFsmName, string varName, LinkType linkType )
		{
			if ( varOwner.OwnerOption == OwnerDefaultOption.UseOwner )
				return null;

			var ownerGO = varOwner.GameObject.Value;
			if ( ownerGO == null )
				return null;

			var ownerFsm = ActionHelpers.GetGameObjectFsm( ownerGO, ownerFsmName );
			if ( ownerFsm == null )
				return null;

			var ownerInfo = new FsmInfo();
			ownerInfo.fsm = ownerFsm.Fsm;

			var accessorInfo = new FsmInfo();
			accessorInfo.action = action;
			accessorInfo.state = state;
			accessorInfo.fsm = state.Fsm;

			return new VariableAccessInfo() { accessor = accessorInfo, owner = ownerInfo, variableName = varName, linkType = linkType };
		}

		public static IEnumerable<PlayMakerFSM> GetAllFSMs()
		{
			return FsmEditor.FsmList.Select( fsm => fsm.Owner as PlayMakerFSM ).Where( pmFsm => pmFsm != null );
		}

		// select fsm, state and action in Playmaker's FSM window
		public static void SelectAction( FsmInfo info )
		{
			FsmEditor.SelectFsm( info.fsm );
			FsmEditor.SelectState( info.state, true );
			FsmEditor.SelectAction( info.action, true );
		}

		public static bool IsPrefab( GameObject go )
		{
			return PrefabUtility.GetPrefabType( go ) == PrefabType.Prefab;
		}
	}
}
