/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Nut : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] Velocity nut_velocity;
    [ SerializeField ] Movement nut_movement;

    // Delegates
    UnityMessage onUpdate;
    UnityMessage onInput;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		onUpdate = ExtensionMethods.EmptyMethod;
		onInput  = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		onUpdate();
	}
#endregion

#region API
    public void OnInputChange()
    {
		onInput();
	}

    public void OnIsNutOnBoltChange( bool value )
    {
        if( value )
        {
			onInput  = IncreaseVelocity;
			onUpdate = MovementOnBolt;
		}
		else
		{
			onInput  = ExtensionMethods.EmptyMethod;
			onUpdate = MovementOnAir;
		}
	}

	public void OnFingerDown()
	{
		nut_velocity.Clear();
	}

	public void OnFingerUp()
	{
	}
#endregion

#region Implementation
    void IncreaseVelocity()
    {
		nut_velocity.OnIncrease();
	}

    void MovementOnBolt()
    {
		nut_movement.OnMovement();
		nut_velocity.OnDecrease_Zero();
	}

    void MovementOnAir()
    {
		nut_movement.OnMovement();
		nut_velocity.OnDecrease_Min();
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}