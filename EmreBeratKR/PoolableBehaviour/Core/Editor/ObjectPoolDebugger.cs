using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EmreBeratKR.ObjectPool.Editor
{
    public class ObjectPoolDebugger : EditorWindow
    {
        private const string Title = "Object Pool Debugger";


        private static readonly List<bool> PoolFoldoutValue = new();
        private static readonly List<bool> PoolObjectsFoldoutValue = new();
        

        [MenuItem("Tools/EmreBeratKR/Object Pool/Debugger")]
        private static void ShowWindow() 
        { 
            GetWindow<ObjectPoolDebugger>(Title);
        }
        
        private void OnGUI()
        {
            //return;
            
            if (!Application.isPlaying) return;

            var pools = GetPools();
            var poolCount = pools.Count;

            for (var i = 0; i < poolCount - PoolFoldoutValue.Count; i++)
            {
                PoolFoldoutValue.Add(false);
            }
            
            for (var i = 0; i < poolCount - PoolObjectsFoldoutValue.Count; i++)
            {
                PoolObjectsFoldoutValue.Add(false);
            }
            
            var index = 0;
            foreach (var (key, value) in GetPools())
            {
                try
                {
                    OnSubPoolGUI(index, key, value);
                    index += 1;
                }
                catch (Exception) {/*ignored*/}
            }
        }


        private static void OnSubPoolGUI(int index, int prefabID, Stack<Object> pool)
        {
            var prefab = EditorUtility.InstanceIDToObject(prefabID);
            var title = $"[{index}]: {prefab.name}";
            PoolFoldoutValue[index] = EditorGUILayout.Foldout(PoolFoldoutValue[index], title);
            
            if (!PoolFoldoutValue[index]) return;

            EditorGUI.indentLevel += 1;

            ObjectFieldGUI("Prefab", prefab, true);
            
            EditorGUILayout.BeginHorizontal();
            
            PoolObjectsFoldoutValue[index] = EditorGUILayout.Foldout(PoolObjectsFoldoutValue[index], "Objects");

            if (GUILayout.Button("Clear"))
            {
                ObjectPool.Clear(prefab);
            }

            DisableGUI(true, () =>
            {
                EditorGUILayout.IntField(pool.Count());
            });

            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel += 1;
            
            if (PoolObjectsFoldoutValue[index])
            {
                var counter = 0;
                foreach (var gameObject in pool)
                {
                    ObjectFieldGUI($"object ({counter})", gameObject, true);
                    counter += 1;
                }
            }
            
            EditorGUI.indentLevel -= 1;
            
            EditorGUI.indentLevel -= 1;
        }

        private static T ObjectFieldGUI<T>(string content, T obj, bool isDisabled)
            where T : Object
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(content);
            EditorGUI.BeginDisabledGroup(isDisabled);
            var modifiedObj = (T) EditorGUILayout.ObjectField(obj, typeof(T), true);
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.EndHorizontal();
            return modifiedObj;
        }

        private static void DisableGUI(bool isDisabled, Action action)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            action?.Invoke();
            EditorGUI.EndDisabledGroup();
        }
        
        private static Dictionary<int, Stack<Object>> GetPools()
        {
            return (Dictionary<int, Stack<Object>>) typeof(ObjectPool)
                .GetField("POOLS", BindingFlags.NonPublic | BindingFlags.Static)
                ?.GetValue(null);
        }
    }
}