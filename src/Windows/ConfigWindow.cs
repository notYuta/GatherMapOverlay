using System;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace GatherMapOverlay.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    private int miningPrimaryIconId;
    private int miningSecondaryIconId;
    private int botanyPrimaryIconId;
    private int botanySecondaryIconId;
    private int legendaryMiningIconId;
    private int legendaryBotanyIconId;

    public ConfigWindow(Configuration configuration)
        : base("GatherMapOverlay 設定###GatherMapOverlayConfig")
    {
        this.configuration = configuration;

        Flags = ImGuiWindowFlags.AlwaysAutoResize;

        // 設定値を読み込み
        LoadConfigValues();
    }

    private void LoadConfigValues()
    {
        miningPrimaryIconId = (int)configuration.MiningPrimaryIconId;
        miningSecondaryIconId = (int)configuration.MiningSecondaryIconId;
        botanyPrimaryIconId = (int)configuration.BotanyPrimaryIconId;
        botanySecondaryIconId = (int)configuration.BotanySecondaryIconId;
        legendaryMiningIconId = (int)configuration.LegendaryMiningIconId;
        legendaryBotanyIconId = (int)configuration.LegendaryBotanyIconId;
    }

    public override void Draw()
    {
        // アイコン設定セクション
        ImGui.Text("アイコン設定");
        ImGui.Separator();
        ImGui.TextColored(new System.Numerics.Vector4(0.7f, 0.7f, 0.7f, 1.0f),
            "主道具: 採掘/伐採、副道具: 砕岩/草刈");

        ImGui.Spacing();

        // 採掘アイコン
        ImGui.Text("採掘 - 主道具:");
        if (ImGui.InputInt("##miningPrimary", ref miningPrimaryIconId))
        {
            if (miningPrimaryIconId > 0)
            {
                configuration.MiningPrimaryIconId = (uint)miningPrimaryIconId;
            }
        }

        ImGui.Text("採掘 - 副道具:");
        if (ImGui.InputInt("##miningSecondary", ref miningSecondaryIconId))
        {
            if (miningSecondaryIconId > 0)
            {
                configuration.MiningSecondaryIconId = (uint)miningSecondaryIconId;
            }
        }

        ImGui.Spacing();

        // 園芸アイコン
        ImGui.Text("園芸 - 主道具:");
        if (ImGui.InputInt("##botanyPrimary", ref botanyPrimaryIconId))
        {
            if (botanyPrimaryIconId > 0)
            {
                configuration.BotanyPrimaryIconId = (uint)botanyPrimaryIconId;
            }
        }

        ImGui.Text("園芸 - 副道具:");
        if (ImGui.InputInt("##botanySecondary", ref botanySecondaryIconId))
        {
            if (botanySecondaryIconId > 0)
            {
                configuration.BotanySecondaryIconId = (uint)botanySecondaryIconId;
            }
        }

        ImGui.Spacing();

        // 伝説アイコン
        ImGui.Text("伝説/刻限 - 採掘:");
        if (ImGui.InputInt("##legendaryMining", ref legendaryMiningIconId))
        {
            if (legendaryMiningIconId > 0)
            {
                configuration.LegendaryMiningIconId = (uint)legendaryMiningIconId;
            }
        }

        ImGui.Text("伝説/刻限 - 園芸:");
        if (ImGui.InputInt("##legendaryBotany", ref legendaryBotanyIconId))
        {
            if (legendaryBotanyIconId > 0)
            {
                configuration.LegendaryBotanyIconId = (uint)legendaryBotanyIconId;
            }
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        // ボタン
        if (ImGui.Button("保存"))
        {
            configuration.Save();
        }

        ImGui.SameLine();

        if (ImGui.Button("リセット"))
        {
            // デフォルト値に戻す
            configuration.MiningPrimaryIconId = 60438;
            configuration.MiningSecondaryIconId = 60437;
            configuration.BotanyPrimaryIconId = 60433;
            configuration.BotanySecondaryIconId = 60432;
            configuration.LegendaryMiningIconId = 60464;
            configuration.LegendaryBotanyIconId = 60462;
            configuration.Save();
            LoadConfigValues();
        }

        ImGui.SameLine();

        if (ImGui.Button("閉じる"))
        {
            IsOpen = false;
        }
    }

    public void Dispose()
    {
    }
}
