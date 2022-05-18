/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using Shapes;
using UnityEditor;
#endif

namespace FFStudio
{
    public class TweenChain : MonoBehaviour
    {
#region Fields (Inspector Interface)
	[ Title( "Setup" ) ]
		[ InfoBox( "Transform of this GO will be used unless another one is provided.", "TransformIsNull" ) ]
		[ SerializeField, LabelText( "Target Transform" ) ] Transform transform_target;

    [ Title( "Start Options" ) ]
        [ LabelText( "Indices to Play on Start" ) ] public int[] indices_toPlayOnStart;
        
    [ Title( "Tween Data" ) ]
#if UNITY_EDITOR
	[ TableList( ShowIndexLabels = true ) ]
#endif
	[ SerializeReference ]
        public List< TweenData > tweenDatas = new List< TweenData >();
		
		Vector3 localPosition_original;
		Vector3 localRotation_original;
		
		Transform transform_ToTween;
#endregion

#region Properties
        [ ShowInInspector, ReadOnly ]
		public List< int > PlayingIndices => indices_playing;
		public bool IsPlaying => indices_playing != null && indices_playing.Count > 0;

		List< int > indices_playing;
#endregion

#region Unity API
        void Awake()
        {
			indices_playing = new List< int >();

			transform_ToTween = transform_target == null ? transform : transform_target;

			foreach( var tweenData in tweenDatas )
				tweenData.Initialize( transform_ToTween );

			localPosition_original = transform_ToTween.localPosition;
			localRotation_original = transform_ToTween.localRotation.eulerAngles;
		}

        void Start()
        {
            if( !enabled )
                return;

            foreach( var index in indices_toPlayOnStart )
                Play( index );
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
			indices_playing.Add( index );
			var tweenData = tweenDatas[ index ];
            
            if( tweenData.indices_nextUp.Length > 0 )
			    tweenData.Play( () => ChainNext( index ) );
            else
			    tweenData.Play();
		}
        
        [ Button(), EnableIf( "IsPlaying" ) ]
        public void Stop()
        {
			for( int i = 0; i < indices_playing.Count; i++ )
			{
				int index = indices_playing[ i ];
				tweenDatas[ index ].Stop();
			}

			indices_playing.Clear();
		}

        [ Button(), EnableIf( "IsPlaying" ) ]
        public void Pause()
        {
			for( int i = 0; i < indices_playing.Count; i++ )
			{
				int index = indices_playing[ i ];
				tweenDatas[ index ].Stop();
			}

			indices_playing.Clear();

		}

		public void ResetTransform()
		{
			ResetLocalPosition();
			ResetLocalRotation();
		}
		
		public void ResetLocalPosition()
		{
			transform_ToTween.localPosition = localPosition_original;
		}
		
		public void ResetLocalRotation()
		{
			var localRotation		  = transform_ToTween.localRotation;
			localRotation.eulerAngles = localRotation_original;
			transform_ToTween.localRotation   = localRotation;
		}
#endregion

#region Implementation
        void ChainNext( int indexOfPlayingTweenData )
        {
#if UNITY_EDITOR
			if( IsPlaying )
				inPlayMode_currentStartPos = transform_ToTween.position;
#endif
			if( IsPlaying )
			{
				indices_playing.Remove( indexOfPlayingTweenData );
				var indices_nextUp = tweenDatas[ indexOfPlayingTweenData ].indices_nextUp;
				for( int i = 0; i < indices_nextUp.Length; i++ )
					Play( indices_nextUp[ i ] );
			}
				
		}
#endregion

#region EditorOnly
#if UNITY_EDITOR
		bool TransformIsNull => transform_target == null;

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
			var theTransform = transform_target == null ? transform : transform_target;

			style = new GUIStyle { normal = new GUIStyleState { textColor = Color.red }, fontSize = 20 };

			Vector3 lastPos = theTransform.position;
			if( Application.isPlaying )
			{
				if( IsPlaying == false )
					return;

				for( var i = 0; i < indices_playing.Count; i++ )
				{
					var index = indices_playing[ i ];
					if( tweenDatas[ index ] is MovementTweenData )
						DrawMovementTweenGizmo( tweenDatas[ index ] as MovementTweenData, ref lastPos, Vector3.zero, index + 1 );
				}
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
