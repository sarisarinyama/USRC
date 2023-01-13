using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WorkingSet
{
    public class WorkingSetData : ScriptableObject
    {
        public enum ObjectType
        {
            GameObject,
            Prefab,
            PrefabInstane,
            Other
        }

        public List<Item> datas = new();

        public void AddFirst(Object obj)
        {
            if (obj == null) return;
            var find_index = datas.FindIndex(item => item.obj == obj);
            if (find_index >= 0)
            {
                var it = datas[find_index];

                datas.RemoveAt(find_index);
                it.ResetInfo(it.obj);
                datas.Insert(0, it);
                return;
            }

            var it2 = new Item();
            it2.ResetInfo(obj);
            datas.Insert(0, it2);
        }

        public void AddLast(Object obj)
        {
            if (obj == null) return;
            var find_index = datas.FindIndex(item => item.obj == obj);
            if (find_index >= 0)
            {
                var it = datas[find_index];

                datas.RemoveAt(find_index);
                it.ResetInfo(it.obj);
                datas.Add(it);
                return;
            }

            var it2 = new Item();
            it2.ResetInfo(obj);
            datas.Add(it2);
        }

        public void InsertAt(int insert_index, Object obj)
        {
            if (obj == null) return;
            var find_index = datas.FindIndex(item => item.obj == obj);
            if (find_index >= 0)
            {
                var it = datas[find_index];
                if (find_index < insert_index)
                {
                    it.ResetInfo(it.obj);
                    datas.Insert(insert_index, it);
                    datas.RemoveAt(find_index);
                }
                else
                {
                    datas.RemoveAt(find_index);
                    it.ResetInfo(it.obj);
                    datas.Insert(insert_index, it);
                }

                return;
            }

            var it2 = new Item();
            it2.ResetInfo(obj);
            datas.Insert(insert_index, it2);
        }

        public bool Exist(Object obj)
        {
            var id = obj.GetInstanceID();
            return datas.Exists(item => item.instance_id == id);
        }

        public void Remove(Object obj)
        {
            var find_index = datas.FindIndex(item => item.obj == obj);
            if (find_index >= 0) datas.RemoveAt(find_index);
        }

        public void RemoveAt(int index)
        {
            datas.RemoveAt(index);
        }

        public void ResetAllInfo()
        {
            for (var i = datas.Count - 1; i >= 0; i--)
            {
                var it = datas[i];
                if (!Application.isPlaying && it.obj == null)
                {
                    datas.RemoveAt(i);
                    continue;
                }

                it.ResetInfo(it.obj);
            }
        }

        [Serializable]
        public class Item
        {
            public Object obj;

            //[System.NonSerialized]
            public string name = "";

            //[System.NonSerialized]
            public int instance_id;

            // [System.NonSerialized]
            public string path = "";

            //[System.NonSerialized]
            public GUIContent gui_content;

            //[System.NonSerialized]
            public ObjectType object_type;

            public void ResetInfo(Object obj)
            {
                this.obj = obj;
                if (Application.isPlaying && obj == null) return;

                instance_id = obj.GetInstanceID();
                path = AssetDatabase.GetAssetPath(instance_id);
                name = obj.name;
                obj = EditorUtility.InstanceIDToObject(instance_id);
                gui_content = new GUIContent(EditorGUIUtility.ObjectContent(obj, null));
                //gui_content.text+=" : GameObject";
                object_type = GetObjectType();
            }

            private ObjectType GetObjectType()
            {
                if (path == "") return ObjectType.GameObject;
                var type = PrefabUtility.GetPrefabType(obj);
                if (type == PrefabType.Prefab) return ObjectType.Prefab;
                if (type == PrefabType.PrefabInstance) return ObjectType.PrefabInstane;
                return ObjectType.Other;
            }
        }
    }
}