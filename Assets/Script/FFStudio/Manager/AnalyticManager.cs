using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Facebook.Unity;
using ElephantSDK;

namespace FFStudio
{
	public class AnalyticManager : MonoBehaviour
	{
#region Fields
		public SharedStringNotifier build_string;
#endregion

#region UnityAPI
		private void Start()
		{
			LoadRemoteConfigs();

			var param = Params.New();
			param.customData = build_string.SharedValue;
			param.CustomString( build_string.SharedValue );

			Elephant.Event( "build_string", 0, param );
		}
#endregion

#region API
		public void ElephantRemoteConfigResponse( ElephantConfigEvent configEvent )
		{
			var value = RemoteConfig.GetInstance().Get( configEvent.configKeyName );

			if( value != null )
				configEvent.source.SetFieldValue( configEvent.fieldName, value );
		}

		public void ElephantLevelEventResponse( ElephantLevelEvent levelEvent )
		{
			switch( levelEvent.elephantEventType )
			{
				case ElephantEvent.LevelStarted:
					Elephant.LevelStarted( levelEvent.level );
					FFLogger.Log( "FFAnalytic Elephant LevelStarted: " + levelEvent.level );
					break;
				case ElephantEvent.LevelCompleted:
					Elephant.LevelCompleted( levelEvent.level );
					FFLogger.Log( "FFAnalytic Elephant LevelFinished: " + levelEvent.level );
					break;
				case ElephantEvent.LevelFailed:
					Elephant.LevelFailed( levelEvent.level );
					FFLogger.Log( "FFAnalytic Elephant LevelFailed: " + levelEvent.level );
					break;
			}
		}

		public void ElephantBasicEventResponse( ElephantBasicEvent basicEvent )
		{
			FFLogger.Log( "Basic Event: " + basicEvent.eventValue_key );
			Elephant.Event( basicEvent.eventValue_key, CurrentLevelData.Instance.currentLevel_Shown );
		}
#endregion

#region Implementation
		public void LoadRemoteConfigs()
		{
			var gameSettings = GameSettings.Instance;
			var useRemoteGameSettings = gameSettings.useRemoteConfig_GameSettings;

			if( !useRemoteGameSettings )
				return;

			var remote = RemoteConfig.GetInstance();
			var settings = remote.Get( "game_settings", "null" );

			if( settings == null )
			{
				Debug.Log( "Remote GameSettings could not configured" );
				return;
			}

			FFLogger.Log( "game_settings\n" + settings );
			var setting_keys = settings.Split( ',' );

			foreach( var settingName in setting_keys )
			{
				var value = remote.Get( settingName );

				if( value != null )
					gameSettings.SetFieldValue( settingName, value );
			}
		}
#endregion
	}
}