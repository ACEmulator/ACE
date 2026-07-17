# CustomClothingBase v1.11 for OpenDereth

CustomClothingBase is authored by OptimShi. It loads custom ClothingTable definitions from JSON, allowing server-side clothing colors and appearance combinations without requiring players to install client DAT updates.

This package contains OptimShi's **unmodified official v1.11 DLLs** plus OpenDereth packaging metadata and instructions. The official release was SHA-256 verified and passed an isolated load test using the exact ACE.Server bundled with OpenDereth.

## Use

Place custom `*.json` clothing definitions directly in this installed mod's `json` folder. With `WatchContent` enabled in `Settings.json`, saving a JSON file clears the clothing cache automatically. You can also use:

- `@clothingbase-export <ClothingBaseID>` to export an existing entry;
- `@clear-clothing-cache` to force a reload while testing.

OptimShi's source and examples: <https://github.com/OptimShi/CustomClothingBase>

Official v1.11 release: <https://github.com/OptimShi/CustomClothingBase/releases/tag/v1.11>

## Preview and saved-world warning

The mod loads successfully, but it has not been thoroughly tested in OpenDereth with every JSON shape, clothing combination, cache-reload path, or saved world. Back up `%LOCALAPPDATA%\OpenDereth` before using it. Do not disable or remove it while world content or saved items refer to custom ClothingBase IDs.

The upstream repository currently has no `LICENSE` file. This redistribution is included based on permission from OptimShi reported by the OpenDereth project maintainer. OptimShi retains ownership of the upstream mod.
