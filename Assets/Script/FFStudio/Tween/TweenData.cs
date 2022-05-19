/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace FFStudio
{
	[ System.Serializable ]
	public abstract class TweenData
	{		
#region Fields (Inspector Interface)
		[ BoxGroup( "Other", false ) ] public string description;
		
	[ Title( "Chain" ) ]
		[ BoxGroup( "Other", false ), LabelText( "Next Indices To Play" ) ] public int[] indices_nextUp;

	[ Title( "Start" ) ]
		[ BoxGroup( "Other", false ), LabelText( "Delay" ) ] public bool hasDelay;
		[ BoxGroup( "Other", false ), ShowIf( "hasDelay" ) ] public float delayAmount;
	
	[ Title( "Events" ), SerializeReference ]
		[ BoxGroup( "Other", false ) ] public TweenEventData[] tweenEventDatas;
		
		[ BoxGroup( "Tween", false ), DisableIf( "IsPlaying" ) ] public bool loop;
		[ BoxGroup( "Tween", false ), ShowIf( "loop" ) ] public LoopType loopType = LoopType.Restart;
		[ BoxGroup( "Tween", false ) ] public Ease easing = Ease.Linear;
		[ BoxGroup( "Tween", false ) ] public UnityEvent onCompleteEvent;

		public Tween Tween => recycledTween.Tween;
		
		protected RecycledTween recycledTween = new RecycledTween();
		protected RecycledTween recycledTween_Delay = new RecycledTween();
		protected Transform transform;

		UnityMessage onComplete;
#endregion

#region Properties
		public bool IsPlaying { get; set; }
#endregion

#region API
		public virtual void Initialize( Transform transform )
		{
			this.transform = transform;
		}

		public void Play( UnityMessage onComplete = null )
		{
			if( hasDelay )
				recycledTween_Delay.Recycle( DOVirtual.DelayedCall( delayAmount, () => CreateAndStartTween( onComplete ) ) );
			else
				CreateAndStartTween( onComplete );
		}
		
		public void Stop()
		{
			recycledTween_Delay.Kill();

			if( IsPlaying )
				Tween.Rewind();

			IsPlaying = false;
		}
		
		// Info: This method name should be _Kill_ and there should also be a separate _Pause_ method.
		public void Pause()
		{
			recycledTween.Kill();
			recycledTween_Delay.Kill();

			IsPlaying = false;
		}
#endregion

#region Implementation
		protected virtual void CreateAndStartTween( UnityMessage onComplete, bool isReversed = false )
		{
			IsPlaying = true;

			this.onComplete = onComplete;
			recycledTween.OnComplete( OnComplete );

			if( tweenEventDatas != null && tweenEventDatas.Length > 0 )
			{
				for( int i = 0; i < tweenEventDatas.Length; i++ )
					tweenEventDatas[ i ].isConsumed = false;
				Tween.OnUpdate( OnUpdate );
			}
		}
		
		void OnUpdate()
		{
			for( int i = 0; i < tweenEventDatas.Length; i++ )
			{
				TweenEventData tweenEventData = tweenEventDatas[ i ];
				
				if( tweenEventData.isConsumed == false )
					tweenEventData.InvokeEventIfThresholdIsPassed( Tween, easing );
			}
		}

		void OnComplete()
		{
			onCompleteEvent.Invoke();
			onComplete?.Invoke();
		}
#endregion
	}
}