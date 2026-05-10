# Folder Color

Folder Color is a small editor-only Unity package that lets you color folders directly from the Project window context menu.

## Features

- Right-click folder menu: `Folder Color > Red / Orange / Yellow / Green / Blue / Purple / Gray`
- `Clear` option to remove a folder color
- Editor-only package
- No runtime dependencies
- Local preferences per developer

## Installation

In Unity:

1. Open `Window > Package Manager`.
2. Click `+`.
3. Choose `Add package from git URL...`.
4. Paste:

```text
https://github.com/JohanJimenex/unity-folder-color.git#v1.0.0
```

For the latest development version:

```text
https://github.com/JohanJimenex/unity-folder-color.git
```

## Usage

1. Right-click a folder in the Project window.
2. Open `Folder Color`.
3. Choose a color.

Use `Folder Color > Clear` to remove the color.

## Notes

Colors are saved using Unity `EditorPrefs`, so they are local to each developer machine and are not committed to your project.

## Requirements

- Unity 2022.3 or newer

## Contributing

Contributions are welcome. Please open an issue before large changes so the scope is clear.

## License

MIT
