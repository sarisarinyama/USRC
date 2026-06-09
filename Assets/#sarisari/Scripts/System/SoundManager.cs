using UnityEngine;
using UnityMidi;

public static class SoundManager
{

    public static AudioImporter mp3Importer;
    public static AudioSource bgmAudioSource;
    public static MidiPlayer bgmMidiPlayer;
    public static bool useDirectMusic;

    public static bool useMCI;
    public static int MP3Volume;

    
    public static string BGMName(string bgmName)
    {
        //inifileから読み込む
        return IniFileUtil.ReadIni("BGM", "bgmName", "", GlobalData.Instance.IniPath);
    }
    
    public static void StopBGM()
    {
        bgmMidiPlayer.Stop();
    }
    
    public static void StartBGM(string audioPath, bool loop = true)
    {
        bgmMidiPlayer.midiSource.streamingAssetPath = bgmMidiPlayer.MakeRelativePath(audioPath);

        bgmMidiPlayer.ResetMidi();
        bgmMidiPlayer.audioSource.loop = loop;
        bgmMidiPlayer.Play();
    }
    
    public static bool InitDirectMusic()
    {
        bool result = false;
        try
        {
            //DirectMusicの初期化処理をここに記述
            //例: DirectMusicのオブジェクトを作成し、必要な設定を行う
            //result = true; // 初期化が成功した場合はtrueを返す
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to initialize DirectMusic: {ex.Message}");
            result = false; // 初期化が失敗した場合はfalseを返す
        }
        return result;
    }

}
