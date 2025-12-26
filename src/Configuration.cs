using Dalamud.Configuration;

namespace GatherMapOverlay;

public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    /// <summary>
    /// 採掘ポイント主道具アイコンID（黄色系）
    /// </summary>
    public uint MiningPrimaryIconId { get; set; } = 60438;

    /// <summary>
    /// 採掘ポイント副道具アイコンID（青系）
    /// </summary>
    public uint MiningSecondaryIconId { get; set; } = 60437;

    /// <summary>
    /// 園芸ポイント主道具アイコンID（黄色系）
    /// </summary>
    public uint BotanyPrimaryIconId { get; set; } = 60433;

    /// <summary>
    /// 園芸ポイント副道具アイコンID（青系）
    /// </summary>
    public uint BotanySecondaryIconId { get; set; } = 60432;

    /// <summary>
    /// 伝説採掘アイコンID
    /// </summary>
    public uint LegendaryMiningIconId { get; set; } = 60464;

    /// <summary>
    /// 伝説園芸アイコンID
    /// </summary>
    public uint LegendaryBotanyIconId { get; set; } = 60462;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
