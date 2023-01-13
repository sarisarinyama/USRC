/// version 1.1


using System.IO;
using UnityEditor;
using UnityEngine;

namespace WorkingSet
{
    internal class WorkingSetWindow : EditorWindow
    {
        public static string plugins_path = @"Assets\EditorWorkingSet\Editor\";

        private static WorkingSetData working_set_data;

        private Rect detial_rc;

        private bool drag_from_self;
        private int drag_item_index = -1;
        private float last_click_time;
        private Vector2 mouse_down_pos;


        private Vector2 scrol_pos;
        private Texture2D selected_background;
        private int selected_item_index = -1;

        private void OnGUI()
        {
            Init();

            DrawHead();

            var current = Event.current;

            var repaint = false;
            var list_modified = false;

            if (current.type == EventType.MouseDrag && drag_item_index >= 0 &&
                drag_item_index < working_set_data.datas.Count)
            {
                var it = working_set_data.datas[drag_item_index];

                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new Object[1] { it.obj };
                DragAndDrop.StartDrag(it.name);

                drag_from_self = true;
                drag_item_index = -1;
            }

            EditorGUIUtility.SetIconSize(new Vector2(24f, 24f));
            scrol_pos = EditorGUILayout.BeginScrollView(scrol_pos);

            if (working_set_data.datas.Count == 0)
            {
                GUILayout.Label("Drag some thing here");
                if (Event.current.type == EventType.DragPerform)
                {
                    for (var i = 0; i < DragAndDrop.objectReferences.Length; i++)
                        working_set_data.AddLast(DragAndDrop.objectReferences[i]);

                    DragAndDrop.AcceptDrag();
                    Event.current.Use();

                    repaint = true;

                    drag_from_self = false;
                }
            }
            else
            {
                for (var index = 0; index < working_set_data.datas.Count; index++)
                {
                    var item = working_set_data.datas[index];
                    //if (item.object_type == WorkingSetData.ObjectType.GameObject && Application.isPlaying) continue;
                    if (item.obj == null) continue;

                    GUIStyle style = null;
                    if (Selection.activeObject == item.obj)
                    {
                        style = new GUIStyle();
                        style.fontStyle = FontStyle.Bold;
                        style.normal.background = selected_background;
                    }

                    if (item.object_type == WorkingSetData.ObjectType.GameObject)
                    {
                        if (style == null) style = new GUIStyle();
                        style.normal.textColor = Color.yellow;
                    }
                    else if (item.object_type == WorkingSetData.ObjectType.Prefab)
                    {
                        if (style == null) style = new GUIStyle();
                        style.normal.textColor = Color.green;
                    }

                    if (style != null) GUILayout.Label(item.gui_content, style);
                    else GUILayout.Label(item.gui_content);

                    var rt = GUILayoutUtility.GetLastRect();

                    if (current.type == EventType.MouseDown)
                        if (rt.Contains(current.mousePosition))
                        {
                            selected_item_index = index;
                            mouse_down_pos = current.mousePosition;
                            drag_item_index = index;
                        }

                    if (current.type == EventType.MouseUp)
                    {
                        drag_item_index = -1;
                        if (current.button == 0 && index == selected_item_index)
                            if (rt.Contains(current.mousePosition))
                            {
                                var double_click = Time.realtimeSinceStartup - last_click_time < 0.3f;
                                HilightObject(item, double_click);
                                last_click_time = Time.realtimeSinceStartup;
                                current.Use();
                            }

                        if (current.button == 2 && index == selected_item_index)
                            if (rt.Contains(current.mousePosition))
                            {
                                working_set_data.RemoveAt(index);

                                DragAndDrop.AcceptDrag();
                                Event.current.Use();

                                repaint = true;
                                break;
                            }
                    }

                    if (Event.current.type == EventType.DragPerform)
                    {
                        var insert_index = GetInsertIndex(rt, mouse_down_pos, current.mousePosition, index,
                            working_set_data.datas.Count);
                        if (insert_index >= 0)
                        {
                            for (var i = 0; i < DragAndDrop.objectReferences.Length; i++)
                                working_set_data.InsertAt(insert_index, DragAndDrop.objectReferences[i]);
                            list_modified = true;

                            DragAndDrop.AcceptDrag();
                            Event.current.Use();

                            repaint = true;
                            break;
                        }

                        drag_from_self = false;
                    }
                    //EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
            GUILayout.Space(20);


            if (current.type == EventType.DragUpdated)
            {
                if (drag_from_self)
                    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                else
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
            }

            if (selected_item_index >= 0 && selected_item_index < working_set_data.datas.Count)
            {
                var it = working_set_data.datas[selected_item_index];
                var path = it.path;
                var asset_head = "Assets";

                var style = new GUIStyle();
                //style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.white;

                if (path.StartsWith(asset_head))
                    path = path.Substring(asset_head.Length, path.Length - asset_head.Length);
                if (path == "") path = it.name;

                if (it.object_type != WorkingSetData.ObjectType.Other)
                    GUI.Label(detial_rc, it.object_type + " : " + path, style);
                else GUI.Label(detial_rc, path, style);
            }

            if (list_modified)
            {
                selected_item_index = -1;
                working_set_data.ResetAllInfo();
                EditorUtility.SetDirty(working_set_data);
            }

            if (repaint || list_modified) Repaint();
        }

        private void OnHierarchyChange()
        {
            if (working_set_data != null)
            {
                working_set_data.ResetAllInfo();
                Repaint();
            }
        }

        private void OnProjectChange()
        {
            if (working_set_data != null)
            {
                working_set_data.ResetAllInfo();
                Repaint();
            }
        }


        private void OnSelectionChange()
        {
            if (working_set_data != null)
            {
                working_set_data.ResetAllInfo();
                Repaint();
            }
        }

        [MenuItem("Window/Working Set Window")]
        public static void ShowWindow()
        {
            GetWindow(typeof(WorkingSetWindow));
        }

        public static void LoadAll()
        {
            if (working_set_data == null) return;

            working_set_data.ResetAllInfo();
            EditorUtility.SetDirty(working_set_data);
        }

        private void Init()
        {
#if UNITY_5_3
            this.titleContent = new GUIContent("Working Set");
#else
            title = "Working Set";
#endif
            if (working_set_data == null)
            {
                var path = plugins_path + "working_set_data.asset";
                var data = AssetDatabase.LoadAssetAtPath(path, typeof(WorkingSetData)) as WorkingSetData;
                if (data == null)
                {
                    data = CreateInstance<WorkingSetData>();
                    AssetDatabase.CreateAsset(data, path);
                }

                data.ResetAllInfo();
                working_set_data = data;
            }

            if (selected_background == null)
            {
                selected_background = new Texture2D(10, 10);
                var color = new Color(0.2f, 0.4f, 0.6f);
                for (var i = 0; i < 10; i++)
                for (var j = 0; j < 10; j++)
                    selected_background.SetPixel(i, j, color);
                selected_background.Apply();
            }
        }

        private void DrawHead()
        {
            GUILayout.Space(10f);
            //GUILayout.BeginHorizontal();
            GUILayout.Label("");
            detial_rc = GUILayoutUtility.GetLastRect();

            var rt = detial_rc;
            rt.xMin = rt.xMax - 40f;
            if (GUI.Button(rt, "help")) GetWindow<WSHelpWindow>();
            //GUILayout.EndHorizontal();
        }

        private int GetInsertIndex(Rect rt, Vector2 mouse_start_pos, Vector2 mouse_current_pos, int index, int count)
        {
            var origin_rt = rt;
            rt.yMin -= 5;
            rt.yMax += 5;
            var insert_index = -1;
            if (mouse_current_pos.x >= rt.xMin && mouse_current_pos.x <= rt.xMax)
            {
                if (index == count - 1)
                {
                    if (mouse_current_pos.y >= rt.yMin)
                    {
                        insert_index = index;
                        if (mouse_current_pos.y > rt.center.y) insert_index++;
                    }
                }
                else
                {
                    if (mouse_current_pos.y >= rt.yMin && mouse_current_pos.y <= rt.yMax)
                    {
                        insert_index = index;
                        if (mouse_current_pos.y > rt.center.y) insert_index++;
                    }
                }
            }

            if (insert_index >= 0 && origin_rt.Contains(mouse_start_pos))
            {
                var offset = mouse_current_pos - mouse_start_pos;
                if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
                {
                    if (offset.x > rt.width * 0.2f) insert_index = count;
                    else if (offset.x < -rt.width * 0.2f) insert_index = 0;
                }
            }

            return insert_index;
        }

        private static string ExtRemoveEnd(string text, string sub)
        {
            var i = text.LastIndexOf(sub);
            if (i >= 0) text = text.Substring(0, text.Length - (text.Length - i));
            return text;
        }

        private void HilightObject(WorkingSetData.Item item, bool double_click)
        {
            if (Event.current.shift)
            {
                if (item.path == "")
                    //EditorApplication.ExecuteMenuItem("Edit/Frame Selected");
                    SceneView.FrameLastActiveSceneView();
                else EditorApplication.ExecuteMenuItem("Window/Project");
            }

            var paser = PathParser.Parse(item.path);
            var is_scene_object = paser.FileName == "";
            var is_folder = false;
            var abusolute_path = "";
            if (!is_scene_object)
            {
                abusolute_path = ExtRemoveEnd(Application.dataPath, "Assets") + item.path;
                is_folder = Directory.Exists(abusolute_path);
            }


            if (double_click && Event.current.control && !is_scene_object)
            {
                Selection.activeObject = item.obj;
                EditorGUIUtility.PingObject(item.obj);
                EditorApplication.ExecuteMenuItem("Assets/Show in Explorer");
                return;
            }

            var ping_object = item.obj;
            if (double_click && is_folder) // is folder
            {
                var info = new DirectoryInfo(abusolute_path);
                var files = info.GetFiles();
                var find_file = false;
                for (var i = 0; i < files.Length; i++)
                {
                    var name = files[i].Name;
                    if (name.EndsWith(".meta")) continue;
                    var path = PathParser.CombinePaths(paser.FullPath, name);
                    ping_object = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                    find_file = true;
                }

                if (!find_file)
                {
                    var dirs = info.GetDirectories();
                    for (var i = 0; i < dirs.Length; i++)
                    {
                        var name = dirs[i].Name;
                        var path = PathParser.CombinePaths(paser.FullPath, name);
                        ping_object = AssetDatabase.LoadAssetAtPath(path, typeof(DefaultAsset));
                        find_file = true;
                    }
                }


                Selection.activeObject = item.obj;
                EditorGUIUtility.PingObject(ping_object);
            }
            else
            {
                Selection.activeObject = ping_object;
                EditorGUIUtility.PingObject(ping_object);
                if (double_click && !is_scene_object) AssetDatabase.OpenAsset(ping_object);
            }
        }

        private class FavoritesTabAssetPostprocessor : AssetPostprocessor
        {
            private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
                string[] movedAssets, string[] movedFromAssetPaths)
            {
                if (deletedAssets.Length > 0 || movedAssets.Length > 0) LoadAll();
            }
        }
    }

    internal class WSHelpWindow : EditorWindow
    {
        public WSHelpWindow()
        {
#if UNITY_5_3
            this.titleContent = new GUIContent("Help");
#else
            title = "Working Set";
#endif
        }

        private void OnGUI()
        {
            var style = new GUIStyle();
            style.richText = true;

            GUILayout.Label("Working Set Operations:");

            GUILayout.Label("<color=yellow>drag object in to window </color>: add item", style);
            GUILayout.Label("<color=yellow>drag item up or down </color>: move item up or down", style);
            GUILayout.Label("<color=yellow>drag item left </color>: move item to top", style);
            GUILayout.Label("<color=yellow>drag item right </color>: move item to bottom", style);
            GUILayout.Label("<color=yellow>middle click</color>: remove item", style);

            GUILayout.Space(20f);

            GUILayout.Label("<color=yellow>click</color>: select item", style);
            GUILayout.Label("<color=yellow>shift + click</color>: force hilight in project view or scene view", style);

            GUILayout.Space(20f);

            GUILayout.Label("<color=yellow>double click</color>: open item", style);
            GUILayout.Label("<color=yellow>ctrl + double click item</color>: explore item in folder", style);
        }
    }
}