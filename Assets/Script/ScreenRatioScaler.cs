/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class ScreenRatioScaler : MonoBehaviour
{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
#if UNITY_EDITOR
		var screen      = GetMainGameViewSize();
		var screenRatio = screen.x / screen.y;
#else
		var screen      = new Vector2( Screen.width, Screen.height );
		var screenRatio = screen.x / screen.y;
#endif
		Vector3 scale = Vector3.one;

		if( screenRatio > GameSettings.Instance.game_reference_screenRatio )
            scale = new Vector3( screenRatio / GameSettings.Instance.game_reference_screenRatio, 1, screenRatio / GameSettings.Instance.game_reference_screenRatio );

        transform.localScale = scale;
        FFLogger.Log( $"Screen({screen}) - Ratio:{screenRatio} - GameSetting.Ratio:{GameSettings.Instance.game_reference_screenRatio} - New Scale:{transform.localScale}", transform );
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
	// C#
	public Vector2 GetMainGameViewSize()
	{
		System.Type T = System.Type.GetType( "UnityEditor.GameView,UnityEditor" );
		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod( "GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static );
		System.Object Res = GetSizeOfMainGameView.Invoke( null, null );
		return ( Vector2 )Res;
	}
#endif
#endregion
}
