/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace FFStudio
{
	public class MovementTweenData : TweenData
	{
		public enum MovementMode { Local, World }

#region Fields
	[ Title( "Movement Tween" ) ]
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public Vector3 deltaPosition;
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public float velocity;
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public MovementMode movementMode;

		float Duration => Mathf.Abs( deltaPosition.magnitude / velocity );
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
		protected override void CreateAndStartTween( UnityMessage onComplete = null, bool isReversed = false )
		{
			if( movementMode == MovementMode.Local )
				recycledTween.Recycle( transform.DOLocalMove( isReversed ? -deltaPosition : deltaPosition, Duration ), onComplete );
			else
				recycledTween.Recycle( transform.DOMove( isReversed ? -deltaPosition : deltaPosition, Duration ), onComplete );

			recycledTween.Tween
				.SetRelative()
				.SetLoops( loop ? -1 : 0, loopType )
				.SetEase( easing );

#if UNITY_EDITOR
			recycledTween.Tween.SetId( "_ff_movement_tween___" + description );
#endif

			base.CreateAndStartTween( onComplete, isReversed );
		}
#endregion

#region EditorOnly
#if UNITY_EDITOR
#endif
#endregion
	}
}