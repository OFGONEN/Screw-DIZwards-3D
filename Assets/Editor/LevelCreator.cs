/* Created by and for usage of FF Studios (2021). */

using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;
using FFEditor;
using FFStudio;

[ CreateAssetMenu( fileName = "level_creator", menuName = "FFEditor/LevelCreator" ) ]
public class LevelCreator : ScriptableObject
{
#region Fields
  [ Title( "Create" ) ]
    public string level_code;
  [ Title( "Create" ) ]
    public int level_start_bolt_length;
    public float level_start_bolt_space;

	// Childs of prefab_bolt: gfx, collider_bottom, collider_upper_in, collider_upper_out
	[ FoldoutGroup( "Setup" ) ] public GameObject prefab_bolt;
    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_bolt_start; 
    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_bolt_model; 
    [ FoldoutGroup( "Setup" ) ] public float bolt_model_height; 

    const char char_prefab_bolt = 'b';
    const char char_space = 'g';

	Transform spawnTransform;

	int   create_index      = 0; // Read index of level create code
	float create_length     = 0; // Current length for the new created object
	float create_position   = 0; // Current world position for the new created object to put in

	StringBuilder stringBuilder = new StringBuilder( 128 );
#endregion

#region Unity API
#endregion

#region API
    [ Button() ]
    public void CreateLevel()
    {
		EditorSceneManager.MarkAllScenesDirty();

		spawnTransform = GameObject.FindWithTag( "Respawn" ).transform;
		spawnTransform.DestoryAllChildren();

		create_index      = 0; 
		create_length     = 0; 
		create_position   = 0; 

		// Place Start Bolt Start
		var bolt_start = PrefabUtility.InstantiatePrefab( prefab_bolt_start ) as GameObject;
		bolt_start.transform.position = Vector3.up * create_position;
		bolt_start.transform.SetParent( spawnTransform );

		PlaceBoltModel( bolt_start, level_start_bolt_length );

		// Place collider bottom
		var collider_bottom = bolt_start.transform.GetChild( 1 ).GetComponent< BoxCollider >();
		collider_bottom.size = new Vector3( 1, bolt_model_height, 1 );
		collider_bottom.transform.localPosition = Vector3.up * bolt_model_height / -2f;

		// Place collider upper in
		var collider_upper_in = bolt_start.transform.GetChild( 2 ).GetComponent< BoxCollider >();
		collider_upper_in.size = new Vector3( 1, bolt_model_height, 1 );
		collider_upper_in.transform.localPosition = Vector3.up * bolt_model_height * level_start_bolt_length + Vector3.up * bolt_model_height / 2f;

		// Place collider upper out
		var collider_upper_out = bolt_start.transform.GetChild( 3 ).GetComponent< BoxCollider >();
		collider_upper_out.size = new Vector3( 1, bolt_model_height, 1 );
		collider_upper_out.transform.localPosition = Vector3.up * bolt_model_height * level_start_bolt_length - Vector3.up * bolt_model_height / 2f;


		create_position += level_start_bolt_space + level_start_bolt_length * bolt_model_height;
		// Place Start Bolt End

		while( create_index < level_code.Length - 1 )
        {
			PlaceObject();
		}

		EditorSceneManager.SaveOpenScenes();
	}
#endregion

#region Implementation
	void PlaceBoltModel( GameObject boltObject, int count )
	{
		var parent = boltObject.transform.GetChild( 0 );

		for( var i = 0; i < count; i++ )
		{
			var bolt = ( PrefabUtility.InstantiatePrefab( prefab_bolt_model ) as GameObject ).transform ;
			bolt.SetParent( parent );
			bolt.localPosition = Vector3.up * i * bolt_model_height;
		}
	}

    void PlaceObject()
    {
        if( level_code[ create_index ] == char_space ) // Place Space
        {
			create_index++;
			FindLength();
			create_position += create_length;
		}
        else if( level_code[ create_index ] == char_prefab_bolt ) // Place Bolt
        {
			create_index++;
			FindLength();
			PlaceBolt();
		}
    }

    void FindLength()
    {
		stringBuilder.Clear();

		for( var i = create_index; i < level_code.Length; i++ )
        {
			stringBuilder.Append( level_code[ i ] );
			create_index = i + 1;

            if( i < level_code.Length - 1 && IsSpecial( i + 1 ) )
				break;
		}

		create_length = float.Parse( stringBuilder.ToString() );
	}

    void PlaceBolt()
    {
		var bolt = PrefabUtility.InstantiatePrefab( prefab_bolt ) as GameObject;
		bolt.transform.position = Vector3.up * create_position;
		bolt.transform.SetParent( spawnTransform );

		PlaceBoltModel( bolt, Mathf.FloorToInt( create_length ) );
		create_position += create_length * bolt_model_height;

		var collider_bottom = bolt.transform.GetChild( 1 ).GetComponent< BoxCollider >();
		collider_bottom.size = new Vector3( 1, bolt_model_height, 1 );
		collider_bottom.transform.localPosition = Vector3.up * bolt_model_height / -2f;

		// Place collider upper in
		var collider_upper_in = bolt.transform.GetChild( 2 ).GetComponent< BoxCollider >();
		collider_upper_in.size = new Vector3( 1, bolt_model_height, 1 );
		collider_upper_in.transform.localPosition = Vector3.up * bolt_model_height * create_length + Vector3.up * bolt_model_height / 2f;

		// Place collider upper out
		var collider_upper_out = bolt.transform.GetChild( 3 ).GetComponent< BoxCollider >();
		collider_upper_out.size = new Vector3( 1, bolt_model_height, 1 );
		collider_upper_out.transform.localPosition = Vector3.up * bolt_model_height * create_length - Vector3.up * bolt_model_height / 2f;
	}

    bool IsCodeValid( out int errorIdex )
    {
		level_code = level_code.ToLower();

		     errorIdex        = -1;
		bool result           = true;

		for( var i = 0; i < level_code.Length; i++ )
        {
			errorIdex = i;
            if( IsSpecial( i ) )
            {
				i++;
				while( i < level_code.Length && !IsSpecial( i ) )
                {
                    if( IsNumber( i ) )
						i++;
                    else
                    {
                        FFLogger.LogError( "INVALID CODE: " + errorIdex );
						return false;
					}
				}

				i--;
			}
            else
            {
                FFLogger.LogError( "INVALID CODE: " + errorIdex );
				return false;
			}
		}

		return result;
	}

    bool IsNumber( int index )
    {
		var codeChar = level_code[ index ];
		return codeChar == '.' || ( codeChar <= 57 && codeChar >= 48 );
	}

    bool IsSpecial( int index )
    {
		var codeChar = level_code[ index ];
		return codeChar == 'b' || codeChar == 'c' || codeChar == 's' || codeChar == 'e';
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}