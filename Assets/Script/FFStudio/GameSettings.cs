/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;

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
        [ Range( 0, 1 ) ] public float velocity_decelerate_endBold = 0.5f;
        public float velocity_rotate_cofactor;
        [ Range( 0, 1 ) ] public float velocity_movement_cofactor;

    [ Title( "Velocity" ) ]
        public Vector2 shatter_force;
        public Vector2 shatter_force_up;
        public Vector2 shatter_torque;
        public float shatter_duration;

    [ Title( "PFX" ) ]
        public float pfx_nut_input_VelocityActivateRatio = 0.5f;
        public float pfx_nut_input_SpawnOffset = 0.75f;

    [ Title( "Collectable" ) ]
        public float collectable_text_offset;
        public float collectable_text_size;
        public Color collectable_text_color;
        public string collectable_text_prefix;

    [ Title( "Camera" ) ]
        public float camera_velocity;
        public Vector3 camera_offset;
        public Vector3 camera_offset_start;
        public float camera_shake_duration;
        public float camera_shake_strength = 1f;

    [ Title( "Pop Up UI" ) ]
		[ Tooltip( "Pop Up Text Pop In Ease"   ) ] public Ease ui_PopUp_In_ease;
		[ Tooltip( "Pop Up Text Pop Out Ease"  ) ] public Ease ui_PopUp_Out_ease;
		[ Tooltip( "Pop Up Text duration"      ) ] public float ui_PopUp_duration;
		[ Tooltip( "Pop Up Text wait duration" ) ] public float ui_PopUp_wait;

    [ Title( "Game" ) ]
        public EndLevelText[] endLevelTexts;
        public SharedFloatNotifier notif_level_progress;
        public float endBolt_height;
        public float game_reference_screenRatio;

    // Info: 3 groups below (coming from template project) are foldout by design: They should remain hidden.
		[ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_GameSettings;
        [ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_Components;

        public int maxLevelCount;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Joy Stick"                                        ) ] public float ui_Entity_JoyStick_Gap;
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