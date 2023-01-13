using System.IO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ImportFont : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    private void Start()
    {
        // ReadTTFFile();
    }
    // private Font font;


    [Button]
    public void ReadTTFFile(string filePath)
    {
        var data = readFileBytes(filePath);

// As we cannot create a font from a byte stream,
// write the raw data to a file
        string filename;
#if UNITY_EDITOR
        filename = "Assets/" + "my.ttf";
#else
         filename = Application.persistentDataPath + "/" +"my.ttf";
#endif
        File.WriteAllBytes(filename, data);
        var font = new Font(filename);

        var font_asset = TMP_FontAsset.CreateFontAsset(font);

        text.font = font_asset;
        text.text = filePath;
    }

    public byte[] readFileBytes(string path)
    {
        using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            var bin = new BinaryReader(fileStream);
            var values = bin.ReadBytes((int)bin.BaseStream.Length);
            bin.Close();
            return values;
        }
    }
}