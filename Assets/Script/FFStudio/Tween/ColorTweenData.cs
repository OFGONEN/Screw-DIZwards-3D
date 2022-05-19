/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace FFStudio
{
	public class ColorTweenData : TweenData
	{
#region Fields
	[ Title( "Color Tween" ) ]
    	[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public bool keepCurrentStartColor = true;
    	[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ), HideIf( "keepCurrentStartColor" ) ] public Color color_start;
    	[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public Color color_end;
    	[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public string name_colorParameter;
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public float duration;
        
        [ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public Renderer renderer_target;

		MaterialPropertyBlock propertyBlock;
		Color color_tweened;
        int shader_ID_color;
#endregion

#region API
        public override void Initialize( Transform transform )
        {
            propertyBlock = new MaterialPropertyBlock();
            
			base.Initialize( transform );
		}
#endregion

#region Implementation
		protected override void CreateAndStartTween( UnityMessage onComplete, bool isReversed = false )
		{
			shader_ID_color = Shader.PropertyToID( name_colorParameter );

			if( keepCurrentStartColor )
				color_tweened = renderer_target.material.color;
			else
			{
				renderer_target.GetPropertyBlock( propertyBlock );
				propertyBlock.SetColor( shader_ID_color, color_start );
				renderer_target.SetPropertyBlock( propertyBlock );
				color_tweened = color_start;
			}

			recycledTween.Recycle( DOTween.To( GetColor, SetColor, color_end, duration ).OnUpdate( OnColorUpdate ), onComplete );

			recycledTween.Tween.SetEase( easing )
				 .SetLoops( loop ? -1 : 0, loopType );

#if UNITY_EDITOR
			recycledTween.Tween.SetId( "_ff_color_tween___" + description );
#endif

			base.CreateAndStartTween( onComplete, isReversed );
		}
        
        void OnColorUpdate()
        {
			renderer_target.GetPropertyBlock( propertyBlock );
			propertyBlock.SetColor( shader_ID_color, color_tweened );
			renderer_target.SetPropertyBlock( propertyBlock );
        }
        
        Color GetColor() => color_tweened;
        void SetColor( Color newColor ) => color_tweened = newColor;
#endregion

#region EditorOnly
#if UNITY_EDITOR
#endif
#endregion
	}
}