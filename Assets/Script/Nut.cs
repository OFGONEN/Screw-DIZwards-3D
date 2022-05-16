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
    Vector2Delegate onInput;
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
	// EditorCall
    public void OnInputChange( Vector2 vector )
    {
		onInput( vector );
	}

	// EditorCall
    public void OnIsNutOnBoltChange( bool value )
    {
        if( value )
        {
			FFLogger.Log( "Nut On Bolt: TRUE" );
			onInput  = ExtensionMethods.EmptyMethod;
			onUpdate = MovementOnBolt;
		}
		else
		{
			FFLogger.Log( "Nut On Bolt: FALSE" );
			onInput  = ExtensionMethods.EmptyMethod;
			onUpdate = MovementOnAir;
		}
	}

	// EditorCall
	public void OnFingerDown()
	{
		FFLogger.Log( "Finger Down" );
		nut_velocity.Clear();

		onInput  = IncreaseVelocity;
		onUpdate = ExtensionMethods.EmptyMethod;
	}

	// EditorCall
	public void OnFingerUp()
	{
		FFLogger.Log( "Finger UP" );
		onInput = ExtensionMethods.EmptyMethod;
		onUpdate = MovementOnBolt; // buralari delegate ile ayirmam lazim
	}

	// EditorCall
	public void OnLevelStarted()
	{
		onInput  = IncreaseVelocity;
		onUpdate = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Implementation
    void IncreaseVelocity( Vector2 vector )
    {
		if( Mathf.Approximately( 0, vector.x ) )
			nut_velocity.Clear();
		else
		{
			nut_velocity.OnIncrease();
			nut_movement.OnMovement( GameSettings.Instance.velocity_movement_cofactor );
		}
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