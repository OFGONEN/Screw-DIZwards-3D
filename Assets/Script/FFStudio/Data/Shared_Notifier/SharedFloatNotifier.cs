/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

namespace FFStudio
{
	[ CreateAssetMenu( fileName = "notif_", menuName = "FF/Data/Shared/Notifier/Float" ) ]
	public class SharedFloatNotifier : SharedDataNotifier< float >
	{
		public void Add( int value )
		{
			SharedValue += value;
		}
	}
}