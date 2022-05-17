/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "behaivour_nut_movement", menuName = "FF/Game/Movement" ) ]
public class Movement : ScriptableObject
{
#region Fields
    [ SerializeField ] Velocity velocity;
    [ SerializeField ] SharedFloatNotifier notif_level_progress;
    [ ShowInInspector, ReadOnly ] float movement_fallDownPoint;
    [ ShowInInspector, ReadOnly ] Transform movement_transform;
	[ ShowInInspector, ReadOnly ] Transform rotate_transform;
	[ ShowInInspector, ReadOnly ] Transform endPoint_transform;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void OnMovement( float cofactor = 1f )
    {
		var position   = movement_transform.position;
		    position.y = Mathf.Max( movement_fallDownPoint, position.y + velocity.CurrentVelocity * cofactor * Time.deltaTime );

		movement_transform.position = position;
		rotate_transform.Rotate( Vector3.up * velocity.CurrentVelocity * cofactor * Time.deltaTime * GameSettings.Instance.velocity_rotate_cofactor, Space.Self );

		// Since level starts at position ZERO
		// notif_level_progress.SharedValue = ( endPoint_transform.position.y - movement_transform.position.y ) / endPoint_transform.position.y;
	}

    // Editor Call
    public void OnMovementTransformChange( SharedReferenceNotifier sharedReferenceNotifier )
    {
		movement_transform = sharedReferenceNotifier.SharedValue as Transform;
	}

    // Editor Call
    public void OnRotateTransformChange( SharedReferenceNotifier sharedReferenceNotifier )
    {
		rotate_transform = sharedReferenceNotifier.SharedValue as Transform;
	}

    // Editor Call
    public void OnFallDownPointChange( SharedReferenceNotifier sharedReferenceNotifier )
    {
		var fallDownPoint = sharedReferenceNotifier.SharedValue as Transform;

		if( fallDownPoint )
			movement_fallDownPoint = fallDownPoint.position.y;
		else
			movement_fallDownPoint = 0;
	}

    // Editor Call
    public void OnEndPointTransformChange( SharedReferenceNotifier sharedReferenceNotifier )
    {
		endPoint_transform = sharedReferenceNotifier.SharedValue as Transform;
	}

    public void Clear()
    {
		movement_transform = null;
		movement_fallDownPoint = 0;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
