using System;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Lumina.Excel.Sheets;

namespace GatherMapOverlay.Services;

public unsafe class MapMarkerService : IDisposable
{
    private readonly IDataManager dataManager;
    private readonly IPluginLog log;
    private readonly Configuration configuration;
    private readonly IObjectTable objectTable;

    // 採掘師のClassJobId
    private const uint MinerJobId = 16;
    // 園芸師のClassJobId
    private const uint BotanistJobId = 17;

    private readonly Lumina.Excel.ExcelSheet<GatheringPoint>? gatheringPointSheet;
    private readonly Lumina.Excel.ExcelSheet<GatheringPointBase>? gatheringPointBaseSheet;
    private readonly Lumina.Excel.ExcelSheet<GatheringPointTransient>? gatheringPointTransientSheet;

    public MapMarkerService(
        IClientState clientState,
        IDataManager dataManager,
        IPluginLog log,
        Configuration configuration,
        IObjectTable objectTable)
    {
        this.dataManager = dataManager;
        this.log = log;
        this.configuration = configuration;
        this.objectTable = objectTable;

        this.gatheringPointSheet = dataManager.GetExcelSheet<GatheringPoint>();
        this.gatheringPointBaseSheet = dataManager.GetExcelSheet<GatheringPointBase>();
        this.gatheringPointTransientSheet = dataManager.GetExcelSheet<GatheringPointTransient>();
    }

    /// <summary>
    /// ObjectTableから近くの採取可能ポイントをマップに表示
    /// 主道具/副道具/伝説でアイコンを分ける
    /// </summary>
    public void RefreshMarkers()
    {
        var agentMap = AgentMap.Instance();
        if (agentMap == null) return;

        // マーカーリセット
        agentMap->ResetMapMarkers();

        if (gatheringPointSheet == null || gatheringPointBaseSheet == null) return;

        var localPlayer = objectTable.LocalPlayer;
        if (localPlayer == null) return;

        var currentJobId = localPlayer.ClassJob.RowId;
        bool isMiner = currentJobId == MinerJobId;
        bool isBotanist = currentJobId == BotanistJobId;

        // ギャザラー以外の場合は表示しない
        if (!isMiner && !isBotanist)
        {
            return;
        }

        foreach (var obj in objectTable)
        {
            if (obj.ObjectKind != ObjectKind.GatheringPoint) continue;

            // 採取可能なポイントのみ表示
            if (!obj.IsTargetable) continue;

            // GatheringPointシートからタイプを取得
            if (!gatheringPointSheet.TryGetRow(obj.BaseId, out var gpRow)) continue;
            var baseId = gpRow.GatheringPointBase.RowId;
            if (!gatheringPointBaseSheet.TryGetRow(baseId, out var baseRow)) continue;

            // GatheringType:
            // 0: 採掘（主道具）, 1: 砕岩（副道具）
            // 2: 伐採（主道具）, 3: 草刈（副道具）
            var typeId = baseRow.GatheringType.RowId;
            
            bool isMiningNode = (typeId == 0 || typeId == 1);
            bool isBotanyNode = (typeId == 2 || typeId == 3);
            bool isPrimaryTool = (typeId == 0 || typeId == 2); // 主道具

            if (!isMiningNode && !isBotanyNode) continue;

            // 現在のクラスに対応するポイントのみ表示
            if (isMiner && !isMiningNode) continue;
            if (isBotanist && !isBotanyNode) continue;

            // 伝説/刻限ノードの判定
            // GatheringPointTransient シートで時限ノードかどうかを確認
            bool isLegendary = false;
            if (gatheringPointTransientSheet != null && 
                gatheringPointTransientSheet.TryGetRow(gpRow.RowId, out var transient))
            {
                // EphemeralStartTime/EphemeralEndTime が 65535 でない場合は刻限
                // GatheringRarePopTimeTable が設定されている場合は伝説/未知
                var startTime = transient.EphemeralStartTime;
                var endTime = transient.EphemeralEndTime;
                var rarePopTable = transient.GatheringRarePopTimeTable.RowId;
                
                isLegendary = (startTime != 65535 || endTime != 65535 || rarePopTable != 0);
            }

            // アイコンIDを決定
            uint iconId;
            if (isLegendary)
            {
                // 伝説/刻限ノード
                iconId = isMiningNode ? configuration.LegendaryMiningIconId : configuration.LegendaryBotanyIconId;
            }
            else if (isMiningNode)
            {
                iconId = isPrimaryTool ? configuration.MiningPrimaryIconId : configuration.MiningSecondaryIconId;
            }
            else // Botany
            {
                iconId = isPrimaryTool ? configuration.BotanyPrimaryIconId : configuration.BotanySecondaryIconId;
            }

            // マーカー追加
            agentMap->AddMapMarker(obj.Position, iconId, scale: 0);
        }
    }

    public void Dispose()
    {
    }
}
