using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

static class IniFileUtil
{
    // Windows API 関数のインポート
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern uint GetPrivateProfileString(
        string lpAppName,        // セクション名
        string lpKeyName,        // キー名
        string lpDefault,        // デフォルト値
        StringBuilder lpReturnedString, // 値を格納するバッファ
        uint nSize,              // バッファサイズ
        string lpFileName        // INIファイルのパス
    );
    
    private static extern long WritePrivateProfileString(
        string section, string key, string value, string filePath);

    /// <summary>
    /// INIファイルから値を取得する
    /// </summary>
    public static string ReadIni(string section, string key, string defaultValue, string filePath)
    {
        if (string.IsNullOrWhiteSpace(section) || string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("セクション名とキー名は必須です。");

        if (!File.Exists(filePath))
            throw new FileNotFoundException("INIファイルが見つかりません。", filePath);

        StringBuilder buffer = new StringBuilder(1024);
        GetPrivateProfileString(section, key, defaultValue, buffer, (uint)buffer.Capacity, filePath);
        return buffer.ToString();
    }
    // 書き込み
    public static void WriteIni(string section, string key, string value, string filePath)
    {
        if (string.IsNullOrWhiteSpace(section) || string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Section と Key は必須です。");

        WritePrivateProfileString(section, key, value ?? "", filePath);
    }

    
    public static void CreateIniFile(string path)
    {
        using (StreamWriter writer = new StreamWriter(path, append: true))
        {
            writer.WriteLine(";SRCの設定ファイルです。");
            writer.WriteLine(";項目の内容に関してはヘルプの");
            writer.WriteLine("; 操作方法 => マップコマンド => 設定変更");
            writer.WriteLine(";の項を参照して下さい。");
            writer.WriteLine("");
            writer.WriteLine("[Option]");
            writer.WriteLine(";メッセージのウェイト。標準は700");
            writer.WriteLine("MessageWait=700");
            writer.WriteLine("");
            writer.WriteLine(";ターン数の表示 [On|Off]");
            writer.WriteLine("Turn=Off");
            writer.WriteLine("");
            writer.WriteLine(";マス目の表示 [On|Off]");
            writer.WriteLine("Square=Off");
            writer.WriteLine("");
            writer.WriteLine(";敵フェイズにはＢＧＭを変更しない [On|Off]");
            writer.WriteLine("KeepEnemyBGM=Off");
            writer.WriteLine("");
            writer.WriteLine(";自動防御モード [On|Off]");
            writer.WriteLine("AutoDefense=Off");
            writer.WriteLine("");
            writer.WriteLine(";自動カーソル移動 [On|Off]");
            writer.WriteLine("AutoMoveCursor=On");
            writer.WriteLine("");
            writer.WriteLine(";スペシャルパワーアニメ [On|Off]");
            writer.WriteLine("SpecialPowerAnimation=On");
            writer.WriteLine("");
            writer.WriteLine(";戦闘アニメ [On|Off]");
            writer.WriteLine("BattleAnimation=On");
            writer.WriteLine("");
            writer.WriteLine(";戦闘アニメの拡張機能 [On|Off]");
            writer.WriteLine("ExtendedAnimation=On");
            writer.WriteLine("");
            writer.WriteLine(";武器準備アニメの自動選択表示 [On|Off]");
            writer.WriteLine("WeaponAnimation=On");
            writer.WriteLine("");
            writer.WriteLine(";移動アニメ [On|Off]");
            writer.WriteLine("MoveAnimation=On");
            writer.WriteLine("");
            writer.WriteLine(";MIDI音源リセットの種類 [None|GM|GS|XG]");
            writer.WriteLine("MidiReset=None");
            writer.WriteLine("");
            writer.WriteLine(";MIDI演奏にDirectMusicを使う [On|Off]");
            if (CommonLib.GetWinVersion() >= 500)
            {
                //NT系のOSではデフォルトでDirectMusicを使う
                //DirectMusicの初期化を試みる
                SoundManager.InitDirectMusic();
                //DirectMusicが使用可能かどうかで設定を切り替え
                if (SoundManager.useDirectMusic)
                {
                    writer.WriteLine("UseDirectMusic=On");
                }
                else
                {
                    writer.WriteLine("UseDirectMusic=Off");
                }
            }
            else
            {
                //NT系OSでなければMCIを使う
                SoundManager.useMCI = true;
                writer.WriteLine("UseDirectMusic=Off");
            }

            writer.WriteLine("");
            writer.WriteLine(";DirectMusicで使うMIDI音源のポート番号 [自動検索=0]");
            writer.WriteLine("MIDIPortID=0");
            writer.WriteLine("");
            writer.WriteLine(";MP3再生時の音量 (0～100)");
            writer.WriteLine("MP3Volume=50");
            writer.WriteLine("");
            writer.WriteLine(";MP3の出力フレーム数");
            writer.WriteLine("MP3OutputBlock=20");
            writer.WriteLine("");
            writer.WriteLine(";MP3の入力直後のスリープ時間(ミリ秒)");
            writer.WriteLine("MP3IutputSleep=5");
            writer.WriteLine("");
            writer.WriteLine(";WAV再生にDirectSoundを使う [On|Off]");
            writer.WriteLine("UseDirectSound=On");
            writer.WriteLine("");
            writer.WriteLine(";画像バッファの枚数");
            writer.WriteLine("ImageBufferNum=64");
            writer.WriteLine("");
            writer.WriteLine(";画像バッファの最大サイズ (MB)");
            writer.WriteLine("MaxImageBufferSize=8");
            writer.WriteLine("");
            writer.WriteLine(";拡大画像を画像バッファに保存する [On|Off]");
            writer.WriteLine("KeepStretchedImage=");
            writer.WriteLine("");
            if (CommonLib.GetWinVersion() >= 500)
            {
                writer.WriteLine(";透過描画にAPI関数TransparentBltを使う [On|Off]");
                writer.WriteLine("UseTransparentBlt=On");
                writer.WriteLine("");
            }

            writer.WriteLine(";拡張データのフォルダ (フルパスで指定)");
            writer.WriteLine("ExtDataPath=");
            writer.WriteLine("ExtDataPath2=");
            writer.WriteLine("");
            writer.WriteLine(";デバッグモード [On|Off]");
            writer.WriteLine("DebugMode=Off");
            writer.WriteLine("");
            writer.WriteLine(";新ＧＵＩ(テスト中) [On|Off]");
            writer.WriteLine("NewGUI=Off");
            writer.WriteLine("");
            writer.WriteLine("[Log]");
            writer.WriteLine(";前回使用したフォルダ");
            writer.WriteLine("LastFolder=");
            writer.WriteLine("");
            writer.WriteLine("[BGM]");
            writer.WriteLine(";SRC起動時");
            writer.WriteLine("Opening=Opening.mid");
            writer.WriteLine(";味方フェイズ開始時");
            writer.WriteLine("Map1=Map1.mid");
            writer.WriteLine(";敵フェイズ開始時");
            writer.WriteLine("Map2=Map2.mid");
            writer.WriteLine(";屋内マップの味方フェイズ開始時");
            writer.WriteLine("Map3=Map3.mid");
            writer.WriteLine(";屋内マップの敵フェイズ開始時");
            writer.WriteLine("Map4=Map4.mid");
            writer.WriteLine(";宇宙マップの味方フェイズ開始時");
            writer.WriteLine("Map5=Map5.mid");
            writer.WriteLine(";宇宙マップの敵フェイズ開始時");
            writer.WriteLine("Map6=Map6.mid");
            writer.WriteLine(";プロローグ・エピローグ開始時");
            writer.WriteLine("Briefing=Briefing.mid");
            writer.WriteLine(";インターミッション開始時");
            writer.WriteLine("Intermission=Intermission.mid");
            writer.WriteLine(";テロップ表示時");
            writer.WriteLine("Subtitle=Subtitle.mid");
            writer.WriteLine(";ゲームオーバー時");
            writer.WriteLine("End=End.mid");
            writer.WriteLine(";戦闘時のデフォルトMIDI");
            writer.WriteLine("default=default.mid");
            writer.WriteLine("");
            writer.Close();
        }
    }
    
    
    //使用例
    static void Main()
    {
        try
        {
            string iniPath = "config.ini"; // 読み込むINIファイルのパス

            // 例: [Database] セクションの Host キーを取得
            string host = ReadIni("Database", "Host", "localhost", iniPath);
            Console.WriteLine($"Database Host: {host}");

            // 例: [Database] セクションの Port キーを取得
            string port = ReadIni("Database", "Port", "3306", iniPath);
            Console.WriteLine($"Database Port: {port}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"エラー: {ex.Message}");
        }
    }
}
