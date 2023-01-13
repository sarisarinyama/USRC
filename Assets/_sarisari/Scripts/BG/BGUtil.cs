using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;
using Sirenix.OdinInspector;
using System.IO;
namespace sarisari
{
    public class BGUtil : MonoBehaviour
    {
        public BGGo_ImportedFonts _ImportedFonts;
        public BGGo_ImportedFontsCommit _ImportedFontsCommit;

        [Button]
        public void CleanImportedFonts()
        {
            _ImportedFonts.Meta.DeleteEntities(_ImportedFonts.Meta.FindEntities(entity => true));

            _ImportedFontsCommit.Meta.DeleteEntities(_ImportedFontsCommit.Meta.FindEntities(entity => true));
        }

        private void Start()
        {
            Load();
        }
        
        private void  OnApplicationQuit() {
            Save();
            
      

        }
        
    public bool HasSavedFile
    {
        get { return File.Exists(SaveFilePath); }
    }

    public string SaveFilePath
    {
        get { return Path.Combine(Application.streamingAssetsPath, "bansheegz_database.bytes"); }
    }

    public void Save()
    {
        File.WriteAllBytes(SaveFilePath, BGRepo.I.Addons.Get<BGAddonSaveLoad>().Save());
    }

    public void Load()
    {
        if (!HasSavedFile) return;

        BGRepo.I.Addons.Get<BGAddonSaveLoad>().Load(File.ReadAllBytes(SaveFilePath));
        // SceneManager.LoadScene(E_Player.GetEntity(0).f_scene.Name);
    }

    }

}
