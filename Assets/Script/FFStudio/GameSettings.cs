﻿/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
#region Fields (Settings)
    // Info: Use Title() attribute ONCE for every game-specific group of settings.
    [ Title( "Velocity" ) ]
        public float velocity_max;
        public float velocity_min;
        public float velocity_accelerate;
        public float velocity_decelerate;
        public float velocity_rotate_cofactor;
        [ Range( 0, 1 ) ] public float velocity_movement_cofactor;


    [ Title( "Game" ) ]
        public EndLevelText[] endLevelTexts;
        public SharedFloatNotifier notif_level_progress;

    // Info: 3 groups below (coming from template project) are foldout by design: They should remain hidden.
		[ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_GameSettings;
        [ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_Components;

        public int maxLevelCount;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Joy Stick"                                        ) ] public float ui_Entity_JoyStick_Gap;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text relative float height"                ) ] public float ui_PopUp_height;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text float duration"                       ) ] public float ui_PopUp_duration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;

        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_height;
        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_duration;
#endregion

#region Fields (Singleton Related)
        private static GameSettings instance;

        private delegate GameSettings ReturnGameSettings();
        private static ReturnGameSettings returnInstance = LoadInstance;

		public static GameSettings Instance => returnInstance();
#endregion

#region API
        public string ReturnEndLevelText()
        {
            for( var i = 0; i < endLevelTexts.Length; i++ )
            {
                if( notif_level_progress.SharedValue >= ( endLevelTexts[ i ].endLevelText_percentage / 100f ) )
                {
					return endLevelTexts[ i ].endLevelText_text;
                }
			}

			return endLevelTexts[ 0 ].endLevelText_text;
		}
#endregion

#region Implementation
        private static GameSettings LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< GameSettings >( "game_settings" );

			returnInstance = ReturnInstance;

			return instance;
		}

		private static GameSettings ReturnInstance()
        {
            return instance;
        }
#endregion


#if UNITY_EDITOR
#region EditorOnly
        private void OnValidate()
        {
			Array.Sort<EndLevelText>( endLevelTexts, new Comparison<EndLevelText>(
				( i1, i2 ) => i2.endLevelText_percentage.CompareTo( i1.endLevelText_percentage )
			) );
        }
#endregion
#endif
    }
}

[ Serializable ]
public struct EndLevelText
{
	public float endLevelText_percentage;
	public string endLevelText_text;
}