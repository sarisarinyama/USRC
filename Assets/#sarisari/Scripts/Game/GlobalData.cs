using UnityEngine;
using Sirenix.OdinInspector;

public class GlobalData : SingletonMonoBehaviour<GlobalData>
{
    [Title("イベントファイル名")] public string ScenarioFileName;

    [Title("イベントファイル名のあるフォルダ")] public string ScenarioPath;

    [Title("セーブデータのファイルディスクリプタ")] public int SaveDataFileNumber;

    [Title("セーブデータのバージョン")] public long SaveDataVersion;

    [Title("そのステージが終了したかを示すフラグ")] public bool IsScenarioFinished;

    [Title("インターミッションコマンドによるステージかどうかを示すフラグ")] public bool IsSubStage;

    [Title("コマンドがキャンセルされたかどうかを示すフラグ")] public bool IsCanceled;

    [Title("フェイズ名")] public string Stage;

    [Title("ターン数")] public int Turn;

    [Title("総ターン数")] public long TotalTurn;

    [Title("総資金")] public long Money;

    [Title("読み込まれているデータ数")] public string Titles;

    [Title("ローカルデータが読み込まれているか？")] public bool IsLocalDataLoaded;

    [Title("最新のセーブデータのファイル名")] public string LastSaveDataFileName;

    [Title("リスタート用セーブデータが利用可能かどうか")] public bool IsRestartSaveDataAvailable;

    [Title("クイックロード用セーブデータが利用可能かどうか")] public bool IsQuickSaveDataAvailable;

    [Title("マス目の表示をするか")] public bool ShowSquareLine;

    [Title("敵フェイズにはＢＧＭを変更しないか")] public bool KeepEnemyBGM;

    [Title("拡張データフォルダへのパス")] public string ExtDataPath;
    [Title("拡張データフォルダへのパス2")] public string ExtDataPath2;

    [Title("MIDI音源リセットの種類")] public string MidiResetType;

    [Title("自動防御モードを使うか")] public bool AutoMoveCursor;

    [Title("スペシャルパワーアニメを表示するか")] public bool SpecialPowerAnimation;

    [Title("戦闘アニメを表示するか")] public bool BattleAnimation;

    [Title("武器準備アニメを表示するか")] public bool WeaponAnimation;

    [Title("拡大戦闘アニメを表示するか")] public bool ExtendedAnimation;

    [Title("移動アニメを表示するか")] public bool MoveAnimation;

    [Title("画像バッファの枚数")] public int ImageBufferSize;

    [Title("画像バッファの最大バイト数")] public long MaxImageBufferByteSize;

    [Title("拡大画像を画像バッファに保存するか")] public bool KeepStretchedImage;

    [Title("透過描画にTransparentBltを使うか")] public bool UseTransparentBlt;

    [Title("SRC.exeのある場所")] public string AppPath;

    [Title("データ中にレベル指定を省略した場合のデフォルトのレベル値")] public const int DEFAULT_LEVEL = -1000;

}