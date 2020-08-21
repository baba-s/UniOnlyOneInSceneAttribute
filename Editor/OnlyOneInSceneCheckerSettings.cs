using System;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// 設定を管理するクラス
	/// </summary>
	[Serializable]
	internal sealed class OnlyOneInSceneCheckerSettings
	{
		//================================================================================
		// 定数
		//================================================================================
		private const string KEY = "UniOnlyOneInSceneChecker";

		//================================================================================
		// 変数(SerializeField)
		//================================================================================
		[SerializeField] private bool   m_isEnable  = false;
		[SerializeField] private string m_logFormat = "OnlyOneInScene 属性が適用されているコンポーネントがシーン内に複数存在します：[ComponentName]";

		//================================================================================
		// プロパティ
		//================================================================================
		public bool IsEnable
		{
			get => m_isEnable;
			set => m_isEnable = value;
		}

		public string LogFormat
		{
			get => m_logFormat;
			set => m_logFormat = value;
		}

		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// EditorPrefs から読み込みます
		/// </summary>
		public static OnlyOneInSceneCheckerSettings LoadFromEditorPrefs()
		{
			var json = EditorPrefs.GetString( KEY );
			var settings = JsonUtility.FromJson<OnlyOneInSceneCheckerSettings>( json ) ??
			               new OnlyOneInSceneCheckerSettings();

			return settings;
		}

		/// <summary>
		/// EditorPrefs に保存します
		/// </summary>
		public static void SaveToEditorPrefs( OnlyOneInSceneCheckerSettings setting )
		{
			var json = JsonUtility.ToJson( setting );

			EditorPrefs.SetString( KEY, json );
		}
	}
}