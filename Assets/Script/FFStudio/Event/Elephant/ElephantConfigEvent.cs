/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
    [ CreateAssetMenu( fileName = "event_elephant_config", menuName = "FF/Event/Elephant Config Event" ) ]
	public class ElephantConfigEvent : GameEvent
	{
		public object source;
		public string fieldName;
		public string configKeyName;

		public void Raise( object targetSource, string targetFieldName, string configKeyName )
		{
			source             = targetSource;
			fieldName          = targetFieldName;
			this.configKeyName = configKeyName;

			Raise();
		}
	}
}