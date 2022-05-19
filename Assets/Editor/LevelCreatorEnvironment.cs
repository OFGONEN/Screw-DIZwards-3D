/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "level_creator_environment", menuName = "FFEditor/Level Creator Environment" ) ]
public class LevelCreatorEnvironment : ScriptableObject
{
#region Fields
  [ Title( "Create" ) ]
    [ SerializeField ] public EnvironmentData[] environmentData;

    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_ground;
    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_background;
    [ FoldoutGroup( "Setup" ) ] public float environment_offset;
    [ FoldoutGroup( "Setup" ) ] public float prefab_background_depth;
    [ FoldoutGroup( "Setup" ) ] public float prefab_background_height;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
	// [ Button() ]
	// void CreateEnvironmentData()
	// {
	// 	environmentData = new EnvironmentData[ 20 ];

	// 	for( var i = 0; i < 20; i++ )
	// 	{
	// 		var data = new EnvironmentData();

	// 		data.level_height              = 200;
	// 		data.level_material_background = ( Material )AssetDatabase.LoadAssetAtPath( $"Assets/Material/mat_platform_game_{i + 1}.mat", typeof( Material ) );
	// 		data.level_material_ground     = ( Material )AssetDatabase.LoadAssetAtPath( $"Assets/Material/mat_ground_game_{i + 1}.mat", typeof( Material ) );

	// 		environmentData[ i ] = data;

	// 		var data = environmentData[ i ];
	// 		data.level_height = 160;
	// 		environmentData[ i ] = data;
	// 	}
	// }

    [ Button() ]
    public void CreateAllLevelEnvironment()
    {
        if( environmentData.Length != GameSettings.Instance.maxLevelCount )
        {
            FFLogger.LogError( "Environment Data count MUST BE EQUAL to Max Level Count" );
			return;
		}

        for( var i = 1; i <= GameSettings.Instance.maxLevelCount; i++ )
        {
			EditorSceneManager.OpenScene( $"Assets/Scenes/game_{i}.unity", OpenSceneMode.Single );
			CreateLevelEnvironment( i - 1 );
		}
    }

    public void CreateLevelEnvironment( int index )
    {
		EditorSceneManager.MarkAllScenesDirty();

		var environmentParent = GameObject.Find( "environment" ).transform;

		// Destory Objects
		environmentParent.DestoryAllChildren();
		DestroyImmediate( GameObject.Find( prefab_ground.name ) );

		var ground = PrefabUtility.InstantiatePrefab( prefab_ground ) as GameObject;
		ground.GetComponentInChildren< Renderer >().sharedMaterial = environmentData[ index ].level_material_ground;
		ground.transform.SetParent( null );
		ground.transform.localPosition    = Vector3.up * environment_offset;
		ground.transform.localEulerAngles = Vector3.zero;
		ground.transform.SetSiblingIndex( environmentParent.GetSiblingIndex() );

		var backgroundCount = environmentData[ index ].level_height / prefab_background_height;

        for( var i = 0; i < backgroundCount; i++ )
        {
			var background = PrefabUtility.InstantiatePrefab( prefab_background ) as GameObject;
            background.GetComponentInChildren< Renderer >().sharedMaterial = environmentData[ index ].level_material_background;

			background.transform.SetParent( environmentParent );
			background.transform.localPosition    = i * Vector3.up * prefab_background_height + Vector3.forward * prefab_background_depth;
			background.transform.localEulerAngles = Vector3.zero;
		}

		environmentParent.transform.position = Vector3.up * environment_offset;

		EditorSceneManager.SaveOpenScenes();
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}


[ Serializable ]
public struct EnvironmentData
{
    public float level_height;
	public Material level_material_ground;
	public Material level_material_background;
}