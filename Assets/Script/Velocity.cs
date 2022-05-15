/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "property_nut_velocity", menuName = "FF/Game/Velocity" ) ]
public class Velocity : ScriptableObject
{
#region Fields
  [ Title( "Shared Value" ) ]
	[ SerializeField ] SharedVector2Notifier notif_input;
	[ ShowInInspector, ReadOnly ] float velocity_current;
#endregion

#region Properties
    public float CurrentVelocity => velocity_current;
#endregion

#region Unity API
#endregion

#region API
	public void OnIncrease()
    {
        velocity_current = Mathf.Min( velocity_current + GameSettings.Instance.velocity_accelerate * notif_input.sharedValue.x * Time.deltaTime, GameSettings.Instance.velocity_max );
    }

	// Regardless of the velocity's sign, velocity
    public void OnDecrease_Zero()
    {
		velocity_current = Mathf.Max( 0, Mathf.Abs( velocity_current ) + GameSettings.Instance.velocity_decelerate * Time.deltaTime ) * Mathf.Sign( velocity_current );
	}

    public void OnDecrease_Min()
    {
        velocity_current = Mathf.Max( velocity_current + GameSettings.Instance.velocity_decelerate * Time.deltaTime, GameSettings.Instance.velocity_min );
    }

    // Editor Call
	public void Clear()
	{
		velocity_current = 0;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}