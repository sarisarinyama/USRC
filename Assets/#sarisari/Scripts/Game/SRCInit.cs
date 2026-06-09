using System;
using System.IO;
using UnityEngine;

public static class SRCInit
{
    public static void Init()
    {
        //二重起動禁止はProjectSettingsのPlayerSettingsで行う


        //SRC.exeのある場所を調べる
# if UNITY_EDITOR
        GlobalData.Instance.AppPath = Directory.GetCurrentDirectory() + "\\"; //Editor上では普通にプロジェクトのカレントディレクトリを確認
# else
		GlobalData.Instance.AppPath = =
 AppDomain.CurrentDomain.BaseDirectory.TrimEnd(//\\//);//EXEを実行したカレントディレクトリ (ショートカット等でカレントディレクトリが変わるのでこの方式で)
# endif


        //SRCが正しくインストールされているかをチェック

        //Bitmap関係のチェック
        CommonLib.DirectoryExists(GlobalData.Instance.AppPath + "Bitmap");

        //イベントグラフィック
        CommonLib.DirectoryExists(GlobalData.Instance.AppPath + "Bitmap\\Event");

        //マップグラフィック
        CommonLib.DirectoryExists(GlobalData.Instance.AppPath + "Bitmap\\Map");

        //効果音
        CommonLib.DirectoryExists(GlobalData.Instance.AppPath + "Sound");


        //Src.iniが無ければ作る
        GlobalData.Instance.IniPath = GlobalData.Instance.AppPath + "Src.ini";
        if (!CommonLib.FileExists(GlobalData.Instance.IniPath))
        {
            IniFileUtil.CreateIniFile(GlobalData.Instance.IniPath);
        }


        //MP3の再生音量
        string buf = IniFileUtil.ReadIni("Option", "MP3Volume", "", GlobalData.Instance.IniPath);
        if (buf == "")
        {
            IniFileUtil.WriteIni("Option", "MP3Volume", "50", GlobalData.Instance.IniPath);
            SoundManager.MP3Volume = 50;
        }
        else
        {
            SoundManager.MP3Volume = int.Parse(buf);
            if (SoundManager.MP3Volume < 0)
            {
                IniFileUtil.WriteIni("Option", "MP3Volume", "0", GlobalData.Instance.IniPath);
                SoundManager.MP3Volume = 0;
            }
            else if (SoundManager.MP3Volume > 100)
            {
                IniFileUtil.WriteIni("Option", "MP3Volume", "100", GlobalData.Instance.IniPath);
                SoundManager.MP3Volume = 100;
            }
        }


        //MP3の入力直後のスリープ時間
        buf = IniFileUtil.ReadIni("Option", "MP3InputSleep", "", GlobalData.Instance.IniPath);
        if (buf == "")
        {
            IniFileUtil.WriteIni("Option", "MP3InputSleep", "5", GlobalData.Instance.IniPath);
        }

        //ＢＧＭ用MIDIファイル設定
        if (IniFileUtil.ReadIni("BGM", "Opening", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Opening", "Opening.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "Map1", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Map1", "Map1.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "Map2", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Map2", "Map2.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "Map3", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Map3", "Map3.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "Map4", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Map4", "Map4.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "Map5", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Map5", "Map5.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "Map6", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Map6", "Map6.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "Briefing", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Briefing", "Briefing.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "Intermission", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Intermission", "Intermission.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "Subtitle", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "Subtitle", "Subtitle.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "End", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "End", "End.mid", GlobalData.Instance.IniPath);
        }

        if (IniFileUtil.ReadIni("BGM", "default", "", GlobalData.Instance.IniPath) == "")
        {
            IniFileUtil.WriteIni("BGM", "default", "default.mid", GlobalData.Instance.IniPath);
        }
        
            
        //起動時の引数から読み込むファイルを探す
        string scenarioFileName = CommonLib.GetExeArgsLast();
        var ext = Path.GetExtension(scenarioFileName).ToLower();
        if (ext != ".src" && ext != ".eve")
        {
            //ダイアログを表示して読み込むファイルを指定する場合
            
            //ダイアログの初期フォルダをlogまたはapppathで設定
            string scenarioPath =IniFileUtil.ReadIni("Log", "LastFolder", "", GlobalData.Instance.IniPath);
            if (scenarioPath == "")
            {
                scenarioPath = GlobalData.Instance.AppPath;
            }
            
            //拡張データのフォルダを設定
            GlobalData.Instance.ExtDataPath = IniFileUtil.ReadIni("Option", "ExtDataPath", "", GlobalData.Instance.IniPath);
            GlobalData.Instance.ExtDataPath2 = IniFileUtil.ReadIni("Option", "ExtDataPath2", "", GlobalData.Instance.IniPath);
            
            
            //オープニング曲演奏
            SoundManager.StopBGM();
            SoundManager.StartBGM(SoundManager.BGMName("Opening"), true);
        }




    }
}