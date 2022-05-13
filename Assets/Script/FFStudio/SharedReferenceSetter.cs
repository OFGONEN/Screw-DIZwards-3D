/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

namespace FFStudio
{
	public class SharedReferenceSetter : MonoBehaviour
	{
#region Fields
		public SharedReferenceNotifier sharedReferenceProperty;
		public Component referenceComponent;
		public bool setByDefault = true;
#endregion

#region UnityAPI
		private void OnEnable()
		{
			if( setByDefault )
				sharedReferenceProperty.SharedValue = referenceComponent;
		}

		private void OnDisable()
		{
			if( setByDefault )
				sharedReferenceProperty.SharedValue = null;
		}
#endregion

#region UnityAPI
		public void SetReference()
		{
			sharedReferenceProperty.SharedValue = referenceComponent;
		}

		public void SetReference( Component component )
		{
			sharedReferenceProperty.SharedValue = component;
		}

		public void SetReferenceNULL()
		{
			sharedReferenceProperty.SharedValue = null;
		}
#endregion
	}
}