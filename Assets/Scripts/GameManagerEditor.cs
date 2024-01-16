using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draw the default inspector

        if(GUILayout.Button("Spawn Cards"))
        {
            GameManager.Singleton.SpawnCards();
        }
    }
}

#endif