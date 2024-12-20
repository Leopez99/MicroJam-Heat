using UnityEngine;
using UnityEditor;
using RelationsInspector;
using RelationsInspector.Backend;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace RelationsInspector.Backend.UGUIEvents
{
	// shows the Components that implement a UI event handler interface as well as the objects that listen to them
	[Title("uGUI events")]
	[Version("1.0.1")]
	[Description( "Shows all components of the active scene which handle uGUI events.\n" + "Connected to each component node are nodes for its listener methods." )]
	[Documentation(@"https://seldomu.github.io/riBackends/UGUIeventBackend/")]
	public class UIEventBackend : MinimalBackend<object, string>
	{
		static Object sceneObj = EditorGUIUtility.whiteTexture;    // representing the scene, as a parent for all the UI GameObjects
		
		// how to react to node-selection events
		public enum OnNodeClick { Ignore, SelectComponent, SelectGameObject };
		static OnNodeClick onNodeClick = OnNodeClick.SelectGameObject;

		bool ignoreNextSelectionChange;

		// derive graph nodes from target object
		public override IEnumerable<object> Init( object target )
		{
			if ( target == sceneObj )
			{
				yield return sceneObj;
				yield break;
			}

			// for GameObject targets, use only the event handler components inside them as seed nodes
			var asGameObject = target as GameObject;
			if ( asGameObject == null )
				yield break;

			var handlers = asGameObject.GetComponentsInChildren<IEventSystemHandler>();
			foreach ( var handler in handlers )
				yield return handler;
		}

		// connect the scene to all its event handlers
		// connect the event handlers to all their listeners
		public override IEnumerable<Relation<object, string>> GetRelations( object entity )
		{
			if ( entity == sceneObj )
			{
				foreach ( var component in GetUIEventHandlers() )
					yield return new Relation<object, string>( entity, component, string.Empty );

				yield break;
			}

			var asComponent = entity as Component;
			if ( asComponent == null )
				yield break;

			// get all of the component's UI events, without duplicates
			var eventReferences = UIEventUtility
				.GetEventRefs( asComponent )
				.GroupBy( r => r.name )
				.Select( gr => gr.First() );

			foreach ( var eventRef in eventReferences )
			{
				// extract event listener data
				var listenerData = EventUtility
					.GetListenerData( eventRef.value, eventRef.prop )
					.Where( record => record.IsValid() );

				// add it to the graph
				foreach ( var record in listenerData )
					yield return new Relation<object, string>( entity, record, eventRef.name );
			}
		}

		public override Rect OnGUI()
		{
			// toolbar
			GUILayout.BeginHorizontal( EditorStyles.toolbar );
			{
				// button: generate a new graph from the active scene
				if ( GUILayout.Button( "Show scene UI events", EditorStyles.toolbarButton, GUILayout.ExpandWidth( false ) ) )
				{
					api.ResetTargets( new object[] { sceneObj } );
				}

				GUILayout.Space( 35 );

				// choice: how to handle node selection
				EditorGUIUtility.labelWidth = 80;
				GUILayout.Label( "On node click:", EditorStyles.miniLabel );
				onNodeClick = (OnNodeClick) EditorGUILayout.EnumPopup( onNodeClick, EditorStyles.toolbarPopup, GUILayout.Width(120) );

				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			return base.OnGUI();
		}

		// node context menu
		// if a listener calls code in the game dlls, add the option to open the source file
		public override void OnEntityContextClick( IEnumerable<object> entities, GenericMenu menu )
		{
			if ( entities.Count() == 1 )
			{
				var single = entities.First();
				var asListener = single as ListenerData;
				if ( asListener != null )
				{
					var targetMb = asListener.target as MonoBehaviour;
					if ( targetMb != null )
					{
						var targetMbScript = MonoScript.FromMonoBehaviour( targetMb );
						menu.AddItem(
							new GUIContent( "open " + targetMbScript.name ),
							false,
							() => AssetDatabase.OpenAsset( targetMbScript )
							);
					}
				}
			}
		}

		// returns all scene components that implement a UI event interface
		IEnumerable<Component> GetUIEventHandlers()
		{
			// this way we also get the inactive gameobjects
			return Resources
				.FindObjectsOfTypeAll<GameObject>()
				.Where( go => !IsPrefab( go ) )
				.SelectMany( go => go.GetComponents<IEventSystemHandler>() )
				.Cast<Component>();
		}

		bool IsPrefab( GameObject go )
		{
			var type = PrefabUtility.GetPrefabType(go);
			return type != PrefabType.None && type != PrefabType.PrefabInstance;
		}

		// node tooltip
		public override string GetEntityTooltip( object entity )
		{
			if ( entity == sceneObj )
				return "Scene";

			var asObject = entity as Object;
			if(asObject != null )
				return asObject.name;

			var asRecord = entity as ListenerData;
			if ( asRecord != null )
			{
				return 
					"<b>Target</b>: " + asRecord.target + 
					"\n<b>Method</b>: " + asRecord.method + 
					"\n<b>Argument</b>: " + asRecord.argument;
			}

			return entity.ToString();
		}

		// handle node selection change, depending on the users choice (onNodeClick)
		public override void OnEntitySelectionChange( object[] selection )
		{
			if ( ignoreNextSelectionChange )
			{
				ignoreNextSelectionChange = false;
				return;
			}

			var selectedObjects = selection
				.Except( new[] { sceneObj } )
				.Select( x => GetEntityObject( x ) )
				.Where( x => x != null )
				.ToArray();

			switch ( onNodeClick )
			{
				case OnNodeClick.Ignore:
					return;
				case OnNodeClick.SelectComponent:
					Selection.objects = selectedObjects;
					return;
				case OnNodeClick.SelectGameObject:
					base.OnEntitySelectionChange( selectedObjects );
					return;
			}
		}

		// adjust to object selection changes that happen outside the window
		public override void OnUnitySelectionChange()
		{
			ignoreNextSelectionChange = true;
			api.SelectEntityNodes( node =>
				{
					var asComponent = node as Component;
					return asComponent == null ? false : Selection.objects.Contains( asComponent.gameObject );
				} );
		}

		// returns GUIContent to be shown in the node's widget
		public override GUIContent GetContent( object entity )
		{
			if ( entity == sceneObj )
				return new GUIContent( "Scene", null, "Scene" );

			var asObject = GetEntityObject( entity );

			return base.GetContent( ( asObject != null ) ? asObject : entity );
		}

		Object GetEntityObject( object entity )
		{
			var asObject = entity as Object;
			if ( asObject != null )
				return asObject;

			var asRecord = entity as ListenerData;
			if ( asRecord != null )
				return asRecord.target;

			return null;
		}
	}
}

