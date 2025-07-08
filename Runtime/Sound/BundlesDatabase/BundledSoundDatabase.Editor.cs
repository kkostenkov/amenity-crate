#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Amenity.Sound
{
    [CustomEditor(typeof(BundledSoundDatabase))]
    public class BundledSoundDatabaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var database = target as BundledSoundDatabase;

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
            SerializedProperty categories = serializedObject.FindProperty(nameof(BundledSoundDatabase.categories));
            EditorGUILayout.PropertyField(categories, includeChildren: true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif