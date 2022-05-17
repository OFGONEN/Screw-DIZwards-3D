/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public class ColorSetter : MonoBehaviour
	{
#region Fields
		[ TitleGroup( "Setup" ), SerializeField ] Color color;

		static int SHADER_ID_COLOR = Shader.PropertyToID( "_BaseColor" );

		Renderer _renderer;
		MaterialPropertyBlock propertyBlock;
#endregion

#region Properties
#endregion

#region Unity API
		void Awake()
		{
			_renderer = GetComponent< Renderer >();

			propertyBlock = new MaterialPropertyBlock();
		}
#endregion

#region API
		public void SetColor( Color color )
		{
			this.color = color;

			SetColor();
		}

		[ Button ]
		public void SetColor()
		{
			_renderer.GetPropertyBlock( propertyBlock );
			propertyBlock.SetColor( SHADER_ID_COLOR, color );
			_renderer.SetPropertyBlock( propertyBlock );
		}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
	}
}