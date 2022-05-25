/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
	public class ElephantParameter
	{
#region Fields
		Dictionary< string, string > dictionary_string = new Dictionary<string, string >();
		Dictionary< string, int > dictionary_int       = new Dictionary<string, int >();
		Dictionary< string, double > dictionary_double = new Dictionary<string, double >();
		string customData;
#endregion

#region Properties
#endregion

#region API
        public void Clear()
        {
			dictionary_string.Clear();
			dictionary_int.Clear();
			dictionary_double.Clear();
		}

		public ElephantParameter Set( string key, string value )
		{
			if( dictionary_string.Count >= 4 )
			{
				Debug.LogError( "You cannot set more than 4 string values for event parameters." );
				return this;
			}

			dictionary_string[ key ] = value;
			return this;
		}

		public ElephantParameter Set( string key, int value )
		{
			if( dictionary_int.Count >= 4 )
			{
				Debug.LogError( "You cannot set more than 4 string values for event parameters." );
				return this;
			}

			dictionary_int[ key ] = value;
			return this;
		}


		public ElephantParameter Set( string key, double value )
		{
			if( dictionary_double.Count >= 4 )
			{
				Debug.LogError( "You cannot set more than 4 string values for event parameters." );
				return this;
			}

			dictionary_double[ key ] = value;
			return this;
		}

		public ElephantParameter CustomString( string data )
		{
			this.customData = data;
			return this;
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