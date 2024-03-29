/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Nut : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] RandomShatterPool pool_random_shatter;
    [ SerializeField ] Velocity nut_velocity;
    [ SerializeField ] Movement nut_movement;
    [ SerializeField ] SharedBoolNotifier notif_nut_isOnBolt;
    [ SerializeField ] SharedBool shared_checkpoint;
    [ SerializeField ] SharedReferenceNotifier notif_checkpoint_transform;
  [ Title( "Fired Events" ) ]
    [ SerializeField ] ElephantBasicEvent event_elephant_basic;
    [ SerializeField ] ParticleSpawnEvent event_pfx_nut_input;
    [ SerializeField ] GameEvent event_level_complete;
    [ SerializeField ] GameEvent event_level_failed;
    [ SerializeField ] GameEvent event_respawn;

	// Private

	// Delegates
	Vector2Delegate onInput;
    UnityMessage onInput_FingerDown;
    UnityMessage onUpdate;
#endregion

#region Properties
#endregion
    UnityMessage onInput_FingerUp;

#region Unity API
	private void OnDisable()
	{
		EmptyDelegates();
	}

    private void Awake()
    {
		EmptyDelegates();
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
			onInput_FingerUp   = ExtensionMethods.EmptyMethod;
			onInput_FingerDown = FingerDown;
			onInput            = ExtensionMethods.EmptyMethod;
			onUpdate           = MovementOnBolt;
		}
		else
		{
			onInput_FingerUp   = ExtensionMethods.EmptyMethod;
			onInput_FingerDown = ExtensionMethods.EmptyMethod;
			onInput            = ExtensionMethods.EmptyMethod;
			onUpdate           = MovementOnAir;
		}
	}

	// EditorCall
	public void OnFingerUp()
	{
		onInput_FingerUp();
	}

	// EditorCall
	public void OnFingerDown()
	{
		onInput_FingerDown();
	}

	// EditorCall
	public void OnLevelStarted()
	{
		onInput_FingerUp   = ExtensionMethods.EmptyMethod;
		onInput_FingerDown = FingerDown;
	}

	// EditorCall: event_bolt_end
	public void OnLevelEndBolt()
	{
		EmptyDelegates();
		onUpdate = MovementOnEndBolt;
	}

	public void OnCollisionObstacle()
	{
		EmptyDelegates();
		gameObject.SetActive( false );
		nut_velocity.Clear();

		var shatter = pool_random_shatter.GetRandomEntity();

		shatter.transform.position = transform.position;
		shatter.DoShatter();

		if( shared_checkpoint.sharedValue )
			DOVirtual.DelayedCall( GameSettings.Instance.shatter_duration, OnRespawn );
		else
			event_level_failed.Raise();
	}
#endregion

#region Implementation
	void OnRespawn()
	{
		transform.position = Vector3.up * ( notif_checkpoint_transform.SharedValue as Transform ).position.y;
		gameObject.SetActive( true );

		event_respawn.Raise();
		OnLevelStarted();
	}

	void FingerUp()
	{
		if( nut_velocity.CurrentVelocity >= GameSettings.Instance.velocity_max * GameSettings.Instance.pfx_nut_input_VelocityActivateRatio )
			event_pfx_nut_input.Raise( "nut_input", transform.position + Vector3.forward * GameSettings.Instance.pfx_nut_input_SpawnOffset );

		onInput          = ExtensionMethods.EmptyMethod;
		onInput_FingerUp = ExtensionMethods.EmptyMethod;
		onUpdate         = MovementOnBolt;

		if( notif_nut_isOnBolt.SharedValue  )
			onInput_FingerDown = FingerDown;
		else
			onInput_FingerDown = ExtensionMethods.EmptyMethod;
	}

	void FingerDown()
	{
		nut_velocity.Clear();

		onInput          = IncreaseVelocity;
		onUpdate         = ExtensionMethods.EmptyMethod;
		onInput_FingerUp = FingerUp;
	}

    void IncreaseVelocity( Vector2 vector )
    {
		if( Mathf.Approximately( 0, vector.x ) )
			nut_velocity.Clear();
		else
		{
			nut_velocity.OnIncrease();
			nut_movement.OnMovement( GameSettings.Instance.velocity_movement_cofactor );

			if( vector.x > 0 )
				event_elephant_basic.Raise( "input_swipe_right" );
			else
				event_elephant_basic.Raise( "input_swipe_left" );
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

	void MovementOnEndBolt()
	{
		nut_velocity.OnDecrease_Zero( GameSettings.Instance.velocity_decelerate_endBold );
		var onEndPoint = nut_movement.OnMovementEndBolt();

		if( onEndPoint || Mathf.Approximately( nut_velocity.CurrentVelocity, 0 ) )
		{
			EmptyDelegates();
			event_level_complete.Raise();
		}
	}

	public void EmptyDelegates()
	{
		onUpdate           = ExtensionMethods.EmptyMethod;
		onInput            = ExtensionMethods.EmptyMethod;
		onInput_FingerUp   = ExtensionMethods.EmptyMethod;
		onInput_FingerDown = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}