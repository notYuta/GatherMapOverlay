# GatherMapOverlay

A Dalamud plugin that displays gathering points on the main map for Miners and Botanists.

## Features

- Shows harvestable gathering points near the player on the map
- Only displays nodes matching your current class (Miner → Mining nodes, Botanist → Botany nodes)
- Different icons for primary/secondary tools
  - Mining (Primary) / Quarrying (Secondary)
  - Logging (Primary) / Harvesting (Secondary)
- Legendary/Unspoiled nodes shown with distinct icons

## Installation

1. Open Dalamud Settings (`/xlsettings`)
2. Go to "Experimental" tab and add this to "Custom Plugin Repositories":
   ```
   https://raw.githubusercontent.com/notYuta/DalamudPluginRepo/main/repo.json
   ```
3. Save and restart the game
4. Install "GatherMapOverlay" from the Plugin Installer

## Configuration

Customize icon IDs via `/xlsettings` → Plugins → GatherMapOverlay.

## Default Icon IDs

| Node Type | Icon ID |
|-----------|---------|
| Mining (Primary) | 60438 |
| Quarrying (Secondary) | 60437 |
| Logging (Primary) | 60433 |
| Harvesting (Secondary) | 60432 |
| Legendary Mining | 60464 |
| Legendary Botany | 60462 |

## License

AGPL-3.0-or-later
