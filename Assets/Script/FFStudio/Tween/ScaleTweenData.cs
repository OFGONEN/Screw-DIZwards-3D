/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace FFStudio
{
	public class ScaleTweenData : TweenData
	{
#region Fields
	[ Title( "Scale Tween" ) ]
    	[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public Vector3 targetScale;
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public float duration;
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
			recycledTween.Recycle( transform.DOScale( targetScale, duration ), onComplete );

			recycledTween.Tween.SetEase( easing )
				 .SetLoops( loop ? -1 : 0, loopType );

#if UNITY_EDITOR
			recycledTween.Tween.SetId( "_ff_scale_tween___" + description );
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