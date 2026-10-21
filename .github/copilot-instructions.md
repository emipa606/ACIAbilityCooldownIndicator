# Copilot Instructions for ACI - Ability Cooldown Indicator (Continued)

## Mod Overview and Purpose

The ACI - Ability Cooldown Indicator (Continued) is a mod for RimWorld that enhances gameplay by providing visual indicators for colonist Ideology abilities. This mod is especially useful for players who want to optimize their strategy by effectively managing ability cooldowns. The mod visually alerts players when abilities are ready to use again, ensuring that no crucial moment is missed.

## Key Features and Systems

- **Cooldown Indicator**: Displays an icon on the topbar that changes color based on the cooldown status of Ideology abilities.
- **Color Ramp**: The indicator transitions from red (not ready) to green (almost ready) and finally to cyan (ready).
- **Notifications**: The mod displays a brief message and plays a notification sound when abilities are ready.
- **Compatibility**: Currently, the mod only supports Ideology abilities. There are no features for Royalty abilities currently as the author hasn't used the Royalty DLC extensively.
- **Integration with Other Mods**: The mod supports integration with Colony Groups and CM Color Coded Mood Bar.

## Coding Patterns and Conventions

- **Code Structure**: The functionality is divided into several C# files, separating concerns such as Harmony patches and utility functions.
- **Naming Conventions**: Follows standard C# conventions with PascalCase for class names and camelCase for method names.
- **Error Handling**: Encourage users to use the Log Uploader for logs and to report issues through Discord channels for efficient error resolution.

## XML Integration

The mod consists of an `About.xml` file that specifies dependencies and metadata. This XML configuration is critical for ensuring compatibility with other mods and identifying the mod within RimWorld.

- **XML Definition**: Ensure correct XML formatting and dependencies listed properly. 
- **Load Order**: Instructions emphasize placing this mod at the end of the modlist to ensure it correctly overrides supported mods.

## Harmony Patching

Harmony is a critical dependency for this mod, facilitating non-destructive modification of existing game code.

- **Patch Class**: `HarmonyPatches` is utilized to intercept and extend the game's behavior without modifying base engine code.
- **Transpiler Method**: The `Transpiler` is used to modify the IL code of the target method, adding the cooldown indicator logic.

## Suggestions for Copilot

- **Autogenerate Patch Templates**: Provide starter code for new Harmony patches.
- **XML Update Prompts**: Assist in maintaining XML files, ensuring dependencies are correctly referenced.
- **Naming Suggestions**: Recommend consistent naming conventions aligned with existing code for new functions and classes.
- **Debugging Assistance**: Propose potential debugging steps or comments, especially within Harmony transpilation.

## Additional Developer Notes

- **Community Contributions**: Encourage developers to contribute by reporting issues or providing solutions via GitHub.
- **Troubleshooting Guidelines**: Start with minimal mods to isolate errors and use specific channels for reporting issues.
- **Special Acknowledgments**: Thanks to contributors and the RimWorld modding community at large for their support.

This mod enhances player experience by offering real-time feedback on ability cooldowns, making strategic management in RimWorld more efficient and enjoyable.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

