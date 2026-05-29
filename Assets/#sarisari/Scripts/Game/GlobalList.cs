using UnityEngine;
using Sirenix.OdinInspector;

public class GlobalList : SingletonMonoBehaviour<GlobalList>
{
    [Title("パイロットデータのリスト")] public PilotDataList PDList;
    [Title("ノンパイロットデータのリスト")] public NonPilotDataList NPDList;
    [Title("ユニットデータのリスト")] public UnitDataList UDList;
    [Title("アイテムデータのリスト")] public ItemDataList IDList;
    [Title("メッセージデータのリスト")] public MessageDataList MDList;
    [Title("特殊効果データのリスト")] public MessageDataList EDList;
    [Title("戦闘アニメデータのリスト")] public MessageDataList ADList;
    [Title("拡張戦闘アニメデータのリスト")] public MessageDataList EADList;
    [Title("ダイアログデータのリスト")] public DialogDataList DDList;
    [Title("スペシャルパワーデータのリスト")] public SpecialPowerDataList SPDList;
    [Title("エリアスデータのリスト")] public AliasDataList ALDList;
    [Title("地形データのリスト")] public TerrainDataList TDList;
    [Title("バトルコンフィグデータのリスト")] public BattleConfigDataList BCList;
    [Title("パイロットのリスト")] public PilotList PList;
    [Title("ユニットのリスト")] public UnitList UList;
    [Title("アイテムのリスト")] public ItemList IList;
}