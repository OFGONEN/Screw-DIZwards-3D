/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class TweenChain : MonoBehaviour
    {
#region Fields (Inspector Interface)
    [ Title( "Start Options" ) ]
		public bool playOnStart;
        [ ShowIf( "playOnStart" ), LabelText( "Index to Play on Start" ) ] public int index_playOnStart = 0;
        
    [ Title( "Tween Data" ) ]
	[ TableList( ShowIndexLabels = true ) ]
	[ SerializeReference ]
        public List< TweenData > tweenDatas = new List< TweenData >();
#endregion

#region Properties
        [ field: SerializeField, ReadOnly ]
        public int PlayingIndex { get; private set; }
		public bool IsPlaying => PlayingIndex > -1;

		public Tween PlayingTween => PlayingIndex >= 0 ? tweenDatas[ PlayingIndex ].Tween : null;
		public TweenData PlayingTweenData => PlayingIndex >= 0 ? tweenDatas[ PlayingIndex ] : null;
#endregion

#region Unity API
        void Awake()
        {
			PlayingIndex = -1;
			
			foreach( var tweenData in tweenDatas )
				tweenData.Initialize( transform );
		}

        void Start()
        {
            if( !enabled )
                return;

            if( playOnStart )
                Play( index_playOnStart );
        }
#endregion

#region API
        [ Button() ]
        public void Play( int index )
        {
        #if UNITY_EDITOR
            if( tweenDatas == null || tweenDatas.Count == 0 )
                FFLogger.LogError( name + ": Tween data array is null or has no elements! Fix this before build!", this );
            else if( index < 0 || index > tweenDatas.Count - 1 )
				FFLogger.LogError( name + ": Given index {index} is outside tween data array's range! Fix this before build!", this );
        #endif
			PlayingIndex = index;
			var tweenData = tweenDatas[ index ];
            
            if( tweenData.chain )
			    tweenData.Play( ChainNext );
            else
			    tweenData.Play();
		}
        
        [ Button(), EnableIf( "IsPlaying" ) ]
        public void Stop()
        {
			PlayingTweenData.Stop();

			PlayingIndex = -1;
        }
#endregion

#region Implementation
        void ChainNext()
        {
            if( PlayingIndex >= 0 )
				Play( PlayingTweenData.index_nextTweenToChainInto );
		}
#endregion

#region EditorOnly
#if UNITY_EDITOR
#endif
#endregion
	}
}
