/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace FFStudio
{
	public class PFX_SpeedTweenData : TweenData
	{
#region Fields
	[ Title( "PFX (Speed) Tween" ) ]
    	[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ), LabelText( "Target Value" ) ] public float value_target;
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public float duration;
        
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public ParticleSystem particleSystem;

		float speed_tweened;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
		protected override void CreateAndStartTween( UnityMessage onComplete, bool isReversed = false )
		{
			speed_tweened = particleSystem.main.startSpeed.constant;

			recycledTween.Recycle( DOTween.To( GetStartSpeed, SetStartSpeed, value_target, duration ), onComplete );

			recycledTween.Tween
                .SetEase( easing )
                .SetLoops( loop ? -1 : 0, loopType );

#if UNITY_EDITOR
			recycledTween.Tween.SetId( "_ff_pfx(speed)_tween___" + description );
#endif

			base.CreateAndStartTween( onComplete, isReversed );
		}
        
        float GetStartSpeed() => particleSystem.main.startSpeed.constant;
        void SetStartSpeed( float newValue )
        {
#if UNITY_EDITOR
			if( particleSystem == null )
			{
				FFLogger.LogError( "PFX (Speed) Tween Data [" + description + "]: No particle system is provided. Will not set start speed." );
				return;
			}
#endif

			var mainModule = particleSystem.main;
			var startSpeedStruct = mainModule.startSpeed;
			startSpeedStruct.constant = newValue;

			mainModule.startSpeed = startSpeedStruct;
		}
#endregion

#region EditorOnly
#if UNITY_EDITOR
#endif
#endregion
	}
}