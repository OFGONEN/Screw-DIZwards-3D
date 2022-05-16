/* Created by and for usage of FF Studios (2021). */

using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace FFStudio
{
	public class DelayedCallTweenData : TweenData
	{
#region Fields
	[ Title( "Delayed Call Tween" ) ]
        [ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ), SuffixLabel( "Seconds" ) ] public float duration;
        [ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public UnityEvent onDelayComplete;
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
			recycledTween.Recycle( DOVirtual.DelayedCall( duration, onDelayComplete.Invoke ), onComplete );

#if UNITY_EDITOR
			recycledTween.Tween.SetId( "_ff_delayedCall_tween___" + description );
#endif
		}
#endregion

#region EditorOnly
#if UNITY_EDITOR
#endif
#endregion
	}
}