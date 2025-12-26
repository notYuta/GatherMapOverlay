using Dalamud.IoC;
using Dalamud.Plugin;
using System;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using GatherMapOverlay.Services;
using GatherMapOverlay.Windows;

namespace GatherMapOverlay;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static IAddonLifecycle AddonLifecycle { get; private set; } = null!;
    [PluginService] internal static IObjectTable ObjectTable { get; private set; } = null!;

    public static Configuration Configuration { get; private set; } = null!;
    private readonly WindowSystem windowSystem = new("GatherMapOverlay");
    private readonly ConfigWindow configWindow;
    private readonly MapMarkerService mapMarkerService;

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        configWindow = new ConfigWindow(Configuration);
        windowSystem.AddWindow(configWindow);

        mapMarkerService = new MapMarkerService(ClientState, DataManager, Log, Configuration, ObjectTable);

        // マップが開かれるたびにマーカーを再追加
        AddonLifecycle.RegisterListener(AddonEvent.PostSetup, "AreaMap", OnMapRefresh);
        AddonLifecycle.RegisterListener(AddonEvent.PostRefresh, "AreaMap", OnMapRefresh);
        AddonLifecycle.RegisterListener(AddonEvent.PostUpdate, "AreaMap", OnMapRefresh);

        // UI描画
        PluginInterface.UiBuilder.Draw += windowSystem.Draw;
        PluginInterface.UiBuilder.OpenConfigUi += () => configWindow.IsOpen = true;
        PluginInterface.UiBuilder.OpenMainUi += () => configWindow.IsOpen = true;

        Log.Information("GatherMapOverlay loaded successfully");
    }

    private void OnMapRefresh(AddonEvent type, AddonArgs args)
    {
        mapMarkerService.RefreshMarkers();
    }

    public void Dispose()
    {
        PluginInterface.UiBuilder.Draw -= windowSystem.Draw;

        AddonLifecycle.UnregisterListener(AddonEvent.PostSetup, "AreaMap", OnMapRefresh);
        AddonLifecycle.UnregisterListener(AddonEvent.PostRefresh, "AreaMap", OnMapRefresh);
        AddonLifecycle.UnregisterListener(AddonEvent.PostUpdate, "AreaMap", OnMapRefresh);

        windowSystem.RemoveAllWindows();
        configWindow.Dispose();
        mapMarkerService.Dispose();
    }
}
