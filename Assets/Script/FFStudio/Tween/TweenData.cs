/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
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
		[ BoxGroup( "Other", false ),  ] public bool chain = false;
		[ BoxGroup( "Other", false ), ShowIf( "chain" ), LabelText( "Next Index" ) ] public int index_nextTweenToChainInto = 0;

	[ Title( "Start" ) ]
		[ BoxGroup( "Other", false ), LabelText( "Delay" ) ] public bool hasDelay;
		[ BoxGroup( "Other", false ), ShowIf( "hasDelay" ) ] public float delayAmount;
	
	[ Title( "Events" ) ]
		[ BoxGroup( "Other", false ) ] public TweenEventData[] tweenEventDatas;
		
		[ BoxGroup( "Tween", false ), DisableIf( "IsPlaying" ) ] public bool loop;
		[ BoxGroup( "Tween", false ), ShowIf( "loop" ) ] public LoopType loopType = LoopType.Restart;
		[ BoxGroup( "Tween", false ) ] public Ease easing = Ease.Linear;

		public Tween Tween => recycledTween.Tween;
		
		protected RecycledTween recycledTween = new RecycledTween();
		
		protected Transform transform;
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
				DOVirtual.DelayedCall( delayAmount, () => CreateAndStartTween( onComplete ) );
			else
				CreateAndStartTween( onComplete );
		}
		
		public void Stop()
		{
			if( IsPlaying )
				Tween.Rewind();

			IsPlaying = false;
		}
#endregion

#region Implementation
		protected virtual void CreateAndStartTween( UnityMessage onComplete = null, bool isReversed = false )
		{
			IsPlaying = true;

			if( tweenEventDatas != null && tweenEventDatas.Length > 0 )
			{
				for( int i = 0; i < tweenEventDatas.Length; i++ )
					tweenEventDatas[ i ].isInvoked = false;
				Tween.OnUpdate( OnUpdate );
			}
		}
		
		void OnUpdate()
		{
			for( int i = 0; i < tweenEventDatas.Length; i++ )
			{
				TweenEventData tweenEventData = tweenEventDatas[ i ];
				if( tweenEventData.isInvoked == false &&
				    ( (  tweenEventData.isPercentage && Tween.ElapsedPercentage() > tweenEventData.delay_percentage.value ) ||
				      ( !tweenEventData.isPercentage && Tween.Elapsed() >           tweenEventData.delay_seconds          ) ) )
				{
					tweenEventData.onElapseComplete.Invoke();
					tweenEventData.isInvoked = true;
					tweenEventDatas[ i ] = tweenEventData;
				}
			}
		}
#endregion
	}
}