using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace RelationsInspector.Backend.PlayMaker
{
	[Title("Playmaker FSM communication")]
	[Version("1.0.0")]
	[Description( "Displays all scene FSMs,\nconnected where a FSM sends an event to another\nor accesses another's variables." )]
	[Documentation( @"https://seldomu.github.io/riBackends/PlayMakerFsmCommunicationBackend/" )]
	[Icon(@"Assets/Gizmos/PlayMakerGUI Icon.tiff")]
	public class PlayMakerInterFSMCommunicationBackend : MinimalBackend<PlayMakerFSM, FsmLink>
	{
		//static bool autoRefreshGraph;

		static ColorLegendEntry[] colorLegend = Utility.linkTypeData.Select( pair => pair.Value ).ToArray();

		static bool showVariables = true;
		static bool showEvents = true;
		static bool showLegend = false;
		static bool hideUnconnected = false;

		// get the state machines that the given one is connected to via its event actions
		public override IEnumerable<Relation<PlayMakerFSM, FsmLink>> GetRelations( PlayMakerFSM entity )
		{
			return Utility.GetCommunicationRelations( entity, showEvents, showVariables );
		}

		// toolbar UI
		// Lets the user init a graph from the scene's FSMs
		public override Rect OnGUI()
		{
			GUILayout.BeginHorizontal( EditorStyles.toolbar );
			{
				if ( GUILayout.Button( "Show active scene", EditorStyles.toolbarButton, GUILayout.ExpandWidth( false ) ) )
				{
					api.ResetTargets( Utility.GetSceneTargets(showEvents, showVariables, hideUnconnected) );
				}

				GUILayout.Space( 20 );
				EditorGUI.BeginChangeCheck();
				showEvents = GUILayout.Toggle( showEvents, "Show events", EditorStyles.toolbarButton );
				if ( EditorGUI.EndChangeCheck() )
				{
					api.Rebuild();
				}

				EditorGUI.BeginChangeCheck();
				showVariables = GUILayout.Toggle( showVariables, "Show variables", EditorStyles.toolbarButton );
				if ( EditorGUI.EndChangeCheck() )
				{
					api.Rebuild();
				}

				EditorGUI.BeginChangeCheck();
				hideUnconnected = GUILayout.Toggle( hideUnconnected, "Hide unconnected", EditorStyles.toolbarButton );
				if ( EditorGUI.EndChangeCheck() )
				{
					api.ResetTargets( Utility.GetSceneTargets(showEvents, showVariables, hideUnconnected) );
				}
				/*GUILayout.Space( 20 );
				EditorGUI.BeginChangeCheck();
				autoRefreshGraph = GUILayout.Toggle( autoRefreshGraph, "Auto-refresh", EditorStyles.toolbarButton, GUILayout.ExpandWidth( false ) );
				if ( EditorGUI.EndChangeCheck() )
				{
					if ( autoRefreshGraph )
					{
						EditorApplication.hierarchyWindowChanged += () => api.ResetTargets( GetSceneTargets() );
						EditorApplication.projectWindowChanged += () => api.ResetTargets( GetSceneTargets() );
					}
					else
					{
						EditorApplication.hierarchyWindowChanged -= () => api.ResetTargets( GetSceneTargets() );
						EditorApplication.projectWindowChanged -= () => api.ResetTargets( GetSceneTargets() );
					}
				}*/

				GUILayout.FlexibleSpace();
				showLegend = GUILayout.Toggle( showLegend, "Legend", EditorStyles.toolbarButton, GUILayout.ExpandWidth( false ) );
				if ( showLegend )
				{
					string title = "Relation types";
					var boxSize = ColorLegendBox.GetSize( title, colorLegend );
					float boxPosX = EditorGUIUtility.currentViewWidth - boxSize.x - 10;
					float boxPosY = 42;
					ColorLegendBox.Draw( new Rect( boxPosX, boxPosY, boxSize.x, boxSize.y ), title, colorLegend );
				}
			}
			GUILayout.EndHorizontal();
			return base.OnGUI();
		}

		// tooltip content for an arrow representing an event link. show the event's name
		public override string GetTagTooltip( FsmLink tag )
		{
			switch ( tag.linkType )
			{
				case LinkType.SendEvent:
					return tag.senderInfo.eventName;

				case LinkType.GetVariable:
				case LinkType.SetVariable:
					return tag.variableName;
				default:
					return string.Empty;
			}
		}

		// graph node context menu entries:
		// - open the sending action in PlayMaker editor
		// - open receiving transition in PlayMaker editor
		public override void OnRelationContextClick( Relation<PlayMakerFSM, FsmLink> relation, GenericMenu menu )
		{
			switch ( relation.Tag.linkType )
			{
				case LinkType.SendEvent:
					menu.AddItem( new GUIContent( "Select sender" ), false, () => Utility.SelectAction( relation.Tag.senderInfo ) );
					menu.AddItem( new GUIContent( "Select receiver" ), false, () => Utility.SelectAction( relation.Tag.receiverInfo ) );
					break;
				case LinkType.GetVariable:
				case LinkType.SetVariable:
					menu.AddItem( new GUIContent( "Select accessor" ), false, () => Utility.SelectAction( relation.Tag.senderInfo ) );
					break;
				default:
					Debug.Log( "unhandled linktype " + relation.Tag.linkType );
					break;
			}
		}

		public override Color GetRelationColor( FsmLink tag )
		{
			if ( !Utility.linkTypeData.ContainsKey( tag.linkType ) )
				return Color.white;

			return Utility.linkTypeData[ tag.linkType ].color;
		}
	}
}
