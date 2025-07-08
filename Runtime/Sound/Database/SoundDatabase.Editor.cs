using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Amenity.Sound
{
    public partial class SoundDatabase
    {
#if UNITY_EDITOR
        [CustomEditor(typeof(SoundDatabase))]
        public class SoundDatabaseEditor : Editor
        {   
            public override void OnInspectorGUI()
            {
                SoundDatabase database = target as SoundDatabase;
                
                EditorGUI.BeginChangeCheck();
                DrawCategories();
                if (EditorGUI.EndChangeCheck()) {
                    EditorUtility.SetDirty(database);
                }

                EditorUtility.SetDirty(database);
                
                GUILayout.Space(10);
            }
            
            private void DrawCategories()
            {
                SerializedProperty categories = serializedObject.FindProperty(nameof(SoundDatabase.categories));
                EditorGUILayout.PropertyField(categories, includeChildren: true);
                serializedObject.ApplyModifiedProperties();
            }
        }
#endif       
    }
}