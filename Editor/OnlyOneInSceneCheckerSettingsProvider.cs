using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// Preferences における設定画面を管理するクラス
	/// </summary>
	internal sealed class OnlyOneInSceneCheckerSettingsProvider : SettingsProvider
	{
		//================================================================================
		// 関数
		//================================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public OnlyOneInSceneCheckerSettingsProvider( string path, SettingsScope scope )
			: base( path, scope )
		{
		}

		/// <summary>
		/// GUI を描画する時に呼び出されます
		/// </summary>
		public override void OnGUI( string searchContext )
		{
			var settings = OnlyOneInSceneCheckerSettings.LoadFromEditorPrefs();

			using ( var checkScope = new EditorGUI.ChangeCheckScope() )
			{
				settings.IsEnable  = EditorGUILayout.Toggle( "Enabled", settings.IsEnable );
				settings.LogFormat = EditorGUILayout.TextField( "Log Format", settings.LogFormat );

				if ( checkScope.changed )
				{
					OnlyOneInSceneCheckerSettings.SaveToEditorPrefs( settings );
				}

				EditorGUILayout.HelpBox( "Log Format で使用できるタグ", MessageType.Info );

				EditorGUILayout.TextArea
				(
					@"[ComponentName]"
				);

				if ( GUILayout.Button( "Use Default" ) )
				{
					settings = new OnlyOneInSceneCheckerSettings();
					OnlyOneInSceneCheckerSettings.SaveToEditorPrefs( settings );
				}
			}
		}

		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// Preferences にメニューを追加します
		/// </summary>
		[SettingsProvider]
		private static SettingsProvider Create()
		{
			var path     = "Preferences/UniOnlyOneInSceneChecker";
			var provider = new OnlyOneInSceneCheckerSettingsProvider( path, SettingsScope.User );

			return provider;
		}
	}
}