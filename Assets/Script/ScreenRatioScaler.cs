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
		var screenRatio = Screen.height / ( float )Screen.width;
		Vector3 scale = Vector3.one;

		if( screenRatio > GameSettings.Instance.game_reference_screenRatio )
            scale = new Vector3( screenRatio / GameSettings.Instance.game_reference_screenRatio, 1, screenRatio / GameSettings.Instance.game_reference_screenRatio );

        transform.localScale = scale;
        FFLogger.Log( $"Screen Ratio:{screenRatio} - New Scale:{transform.localScale}", transform );
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
