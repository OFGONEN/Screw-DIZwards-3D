/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Shapes;
using UnityEditor;
#endif

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
		
		Vector3 localPosition_original;
		Vector3 localRotation_original;
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

			localPosition_original = transform.localPosition;
			localRotation_original = transform.localRotation.eulerAngles;
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
		
		public void ResetLocalPosition()
		{
			transform.localPosition = localPosition_original;
		}
		
		public void ResetLocalRotation()
		{
			var localRotation		  = transform.localRotation;
			localRotation.eulerAngles = localRotation_original;
			transform.localRotation   = localRotation;
		}
#endregion

#region Implementation
        void ChainNext()
        {
#if UNITY_EDITOR
			if( PlayingIndex >= 0 )
				inPlayMode_currentStartPos = transform.position;
#endif
			if( PlayingIndex >= 0 )
				Play( PlayingTweenData.index_nextTweenToChainInto );
		}
#endregion

#region EditorOnly
#if UNITY_EDITOR
		Vector3 inPlayMode_currentStartPos;
		GUIStyle style;

		void DrawMovementTweenGizmo( MovementTweenData tweenData, ref Vector3 lastPos, Vector3 verticalOffset, int tweenNo )
		{
			Vector3 startPos = lastPos;

			if( Application.isPlaying )
				startPos = inPlayMode_currentStartPos;

			Color color = new Color( 1.0f, 0.75f, 0.0f );

			Draw.UseDashes = true;
			Draw.DashStyle = DashStyle.RelativeDashes( DashType.Basic, 1, 1 );

			var deltaPosition = tweenData.deltaPosition;

			lastPos = startPos + deltaPosition;

			Draw.Line( startPos + verticalOffset, lastPos + verticalOffset, 0.1f, LineEndCap.None, color );
			var direction = deltaPosition.normalized;
			var deltaMagnitude = deltaPosition.magnitude;
			var coneLength = 0.2f;
			var conePos = Vector3.Lerp( startPos, lastPos, 1.0f - coneLength / deltaMagnitude );
			Draw.Cone( conePos + verticalOffset, deltaPosition.normalized, 0.1f, 0.2f, color );
			Handles.Label( ( lastPos + startPos ) / 2 + verticalOffset, tweenNo.ToString() + ": " + tweenData.description, style );
		}

		void OnDrawGizmosSelected()
		{
			style = new GUIStyle { normal = new GUIStyleState { textColor = Color.red }, fontSize = 20 };

			Vector3 lastPos = transform.position;
			if( Application.isPlaying )
			{
				if( PlayingIndex < 0 )
					return;

				if( tweenDatas[ PlayingIndex ] is MovementTweenData )
					DrawMovementTweenGizmo( tweenDatas[ PlayingIndex ] as MovementTweenData, ref lastPos, Vector3.zero, PlayingIndex + 1 );
			}
			else
				for( var i = 0; i < tweenDatas.Count; i++ )
					if( tweenDatas[ i ] is MovementTweenData )
						DrawMovementTweenGizmo( tweenDatas[ i ] as MovementTweenData, ref lastPos, Vector3.up * i * 0.3f, i + 1 );
		}
#endif
#endregion
	}
}
