# GitHub Copilot Instruction File for "ACI - Ability Cooldown Indicator (Continued)"

Welcome to the development guide for the "ACI - Ability Cooldown Indicator (Continued)" mod. This document provides an overview, coding patterns, XML integration instructions, and suggestions for using GitHub Copilot to enhance your modding workflow.

## Mod Overview and Purpose

**Mod Name:** ACI - Ability Cooldown Indicator (Continued)

**Description:** This RimWorld mod provides an intuitive cooldown indicator for colonist Ideology abilities. It is a continuation and update of Georodin's original mod. ACI enhances gameplay by providing a visual and auditory cue when Ideology abilities are ready to be used again.

**Purpose:** The primary goal of this mod is to prevent players from missing the right moment to reactivate colonist abilities. The indicator utilizes a color ramp from red (not ready) to green (almost ready) and finally cyan (ready). A notification sound and a short message are also displayed when abilities are ready.

**Important Note:** This mod is currently limited to Ideology abilities and does not support Royalty abilities. It should be placed at the end of the mod list to ensure compatibility with supported mods.

## Key Features and Systems

- **Topbar Integration:** The mod modifies the colonist topbar to show ability cooldown indicators.
- **Color-Coded Notifications:** A color ramp provides a clear visual status of ability readiness.
- **Auditory and Text Notifications:** Users are alerted with a sound and text message when abilities are ready.
- **Mod Compatibility:** While it supports “Colony Groups” and “CM Color Coded Mood Bar,” it may conflict with other mods altering the topbar.

## Coding Patterns and Conventions

- **File Naming:** Use PascalCase for class and method names.
- **Access Modifiers:** Keep classes and methods internal unless they need to be exposed externally.
- **Consistent Indentation:** Use spaces instead of tabs for consistent formatting.

### Relevant Files

- **HarmonyPatches.cs**
  - Defines Harmony patches to modify game behavior selectively.

- **IOUtil.cs**
  - Contains utility methods for Input/Output operations.

- **MainFunctionality.cs**
  - Core class where the main logic of the cooldown indicator is implemented.

## XML Integration

- Define XML extensions to extend existing game definitions without altering the core files.
- Ensure that XML definitions are placed in the appropriate folder structure, following RimWorld's modding best practices.
- Utilize XML inheritance to extend base game functionality where possible.

## Harmony Patching

- Use Harmony to patch methods without modifying the original game files.
- Create prefix, postfix, or transpiler methods as needed to inject logic.
- Ensure patches are targeted only at necessary methods to minimize performance impact and compatibility issues.

## Suggestions for Copilot

To leverage GitHub Copilot effectively:

- **Utilize Copilot for Boilerplate Code:** Auto-generate repetitive parts of the code, such as property definitions or basic method structures.
- **Leverage Code Suggestions:** Use Copilot's suggestions to explore potential improvements in logic structures or optimizations in the code.
- **Implement Error Handling:** Depending on the suggestions, implement comprehensive error checking and handling mechanisms.
- **Collaborate and Review:** Encourage the team to use Copilot but review all generated code carefully to ensure it aligns with the mod's requirements and standards.

We hope this guide proves helpful as you work on improving and maintaining the ACI mod. Feel free to contribute by fixing bugs or suggesting enhancements!
