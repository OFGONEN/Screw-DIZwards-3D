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
    [ ShowInInspector, ReadOnly ] float movement_fallDownPoint;
    [ ShowInInspector, ReadOnly ] Transform movement_transform;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void OnMovement()
    {
		var position   = movement_transform.position;
		    position.y = Mathf.Max( movement_fallDownPoint, position.y + velocity.CurrentVelocity * Time.deltaTime );

		movement_transform.position = position;
	}

    // Editor Call
    public void OnMovementTransformChange( SharedReferenceNotifier sharedReferenceNotifier )
    {
		movement_transform = sharedReferenceNotifier.SharedValue as Transform;
	}

    // Editor Call
    public void OnFallDownPointChange( SharedReferenceNotifier sharedReferenceNotifier )
    {
		movement_fallDownPoint = ( sharedReferenceNotifier.SharedValue as Transform ).position.y;
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
