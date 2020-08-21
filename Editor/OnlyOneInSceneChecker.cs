using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// OnlyOneInScene 属性が適用されているコンポーネントがシーン内に複数存在したら Unity を再生できなくするエディタ拡張
	/// </summary>
	[InitializeOnLoad]
	internal static class OnlyOneInSceneChecker
	{
		//================================================================================
		// クラス
		//================================================================================
		/// <summary>
		/// OnlyOneInScene 属性が適用されているコンポーネントの情報を管理するクラス
		/// </summary>
		internal sealed class OnlyOneInSceneData
		{
			/// <summary>
			/// OnlyOneInScene 属性が適用されたコンポーネント
			/// </summary>
			public Component Component { get; }

			/// <summary>
			/// OnlyOneInScene 属性が適用されたコンポーネントの名前
			/// </summary>
			public string ComponentName { get; }

			/// <summary>
			/// コンストラクタ
			/// </summary>
			public OnlyOneInSceneData
			(
				Component component,
				string    componentName
			)
			{
				Component     = component;
				ComponentName = componentName;
			}
		}

		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		static OnlyOneInSceneChecker()
		{
			EditorApplication.playModeStateChanged += OnChange;
		}

		/// <summary>
		/// Unity のプレイモードの状態が変化した時に呼び出されます
		/// </summary>
		private static void OnChange( PlayModeStateChange state )
		{
			if ( state != PlayModeStateChange.ExitingEditMode ) return;

			var settings = OnlyOneInSceneCheckerSettings.LoadFromEditorPrefs();

			if ( !settings.IsEnable ) return;

			var list = Validate().ToArray();

			if ( list.Length <= 0 ) return;

			var logFormat = settings.LogFormat;

			foreach ( var n in list )
			{
				var message = logFormat;
				message = message.Replace( "[ComponentName]", n.ComponentName );

				Debug.LogError( message, n.Component );
			}

			EditorApplication.isPlaying = false;
		}

		/// <summary>
		/// OnlyOneInScene 属性が適用されており、シーン内に複数存在するコンポーネントの一覧を返します
		/// </summary>
		private static IEnumerable<OnlyOneInSceneData> Validate()
		{
			var gameObjects = Resources
					.FindObjectsOfTypeAll<GameObject>()
					.Where( c => c.scene.isLoaded )
					.Where( c => c.hideFlags == HideFlags.None )
				;

			var onlyOneInSceneMonoBehaviours = new HashSet<Type>();

			foreach ( var gameObject in gameObjects )
			{
				var monoBehaviours = gameObject.GetComponents<MonoBehaviour>();

				foreach ( var monoBehaviour in monoBehaviours )
				{
					if ( monoBehaviour == null ) continue;

					var type  = monoBehaviour.GetType();
					var attrs = type.GetCustomAttributes( typeof( OnlyOneInSceneAttribute ), true );

					if ( attrs.Length <= 0 ) continue;

					if ( onlyOneInSceneMonoBehaviours.Contains( type ) )
					{
						var data = new OnlyOneInSceneData
						(
							component: monoBehaviour,
							componentName: type.Name
						);

						yield return data;
					}
					else
					{
						onlyOneInSceneMonoBehaviours.Add( type );
					}
				}
			}
		}
	}
}