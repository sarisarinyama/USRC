using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DLavender.Windows
{
    public class FileDrop : MonoBehaviour
    {
        public TextMeshProUGUI board;

        public ImportFont imp;

        private void OnEnable()
        {
            FileBridge.Enable();
            FileBridge.OnDragFiles += OnDragCallback;
        }

        private void OnDisable()
        {
            FileBridge.Disable();
        }

        private void OnDragCallback(List<string> paths)
        {
            // board.text = "";
            foreach (var path in paths)
            {
                // board.text += path;
                // board.text += "\n";
                imp.ReadTTFFile(path);
            }
        }
    }
}