/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Shatter : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] ShatterPool pool_shatter;

  [ Title( "Info" ) ]
	[ SerializeField, ReadOnly ] public Rigidbody[] shatter_rigidbodies;
    [ SerializeField, ReadOnly ] public Vector3[] shatter_positions;
    [ SerializeField, ReadOnly ] public Vector3[] shatter_rotations;

	// Private Field
	RecycledTween recycledTween = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
	[ Button() ]
	public void DoShatter()
	{
		gameObject.SetActive( true );

		for( var i = 0; i < shatter_rigidbodies.Length; i++ )
		{
			var rb = shatter_rigidbodies[ i ];

			rb.isKinematic = false;
			rb.useGravity  = true;

			rb.AddForce( GameSettings.Instance.shatter_force_up.ReturnRandom() * Vector3.up + GameSettings.Instance.shatter_force.ReturnRandom() * Random.onUnitSphere, ForceMode.Impulse );
			// Info: Shatter objects pivots must be on their on center, Enable this if models are correct
			rb.AddTorque( GameSettings.Instance.shatter_torque.ReturnRandom() * Random.onUnitSphere, ForceMode.Impulse );
		}

		recycledTween.Recycle( DOVirtual.DelayedCall( GameSettings.Instance.shatter_duration, ReturnDefault ) );
	}
#endregion

#region Implementation
	void ReturnDefault()
	{
		for( var i = 0; i < shatter_rigidbodies.Length; i++ )
		{
			var rb = shatter_rigidbodies[ i ];

			rb.isKinematic = true;
			rb.useGravity  = false;

			rb.transform.localPosition    = shatter_positions[ i ];
			rb.transform.localEulerAngles = shatter_rotations[ i ];
		}

		pool_shatter.ReturnEntity( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    [ Button() ]
    private void CacheComponents()
    {
        shatter_rigidbodies  = GetComponentsInChildren< Rigidbody >();

		shatter_positions = new Vector3[ shatter_rigidbodies.Length ];
		shatter_rotations = new Vector3[ shatter_rigidbodies.Length ];

		for( var i = 0; i < shatter_rigidbodies.Length; i++ )
        {
			shatter_rigidbodies[ i ].isKinematic = true;
			shatter_rigidbodies[ i ].useGravity  = false;

			shatter_positions[ i ] = shatter_rigidbodies[ i ].transform.localPosition;
			shatter_rotations[ i ] = shatter_rigidbodies[ i ].transform.localEulerAngles;
		}
    }
#endif
#endregion
}