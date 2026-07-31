#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JohanJimenez.FolderColor.Editor
{
    [InitializeOnLoad]
    public static class FolderColorProjectWindow
    {
        private const string PrefsKey = "JohanJimenez.FolderColor.Entries";
        private const float ListIconSize = 16f;

        private static readonly Dictionary<string, Color> ColorsByPath = new Dictionary<string, Color>();
        private static readonly Dictionary<string, Texture2D> FolderTextures = new Dictionary<string, Texture2D>();

        static FolderColorProjectWindow()
        {
            LoadColors();
            EditorApplication.projectWindowItemOnGUI -= DrawFolderColor;
            EditorApplication.projectWindowItemOnGUI += DrawFolderColor;
        }

        [MenuItem("Assets/Folder Color/Red", false, 2000)]
        private static void SetRed() => SetSelectedFoldersColor(new Color(0.95f, 0.22f, 0.20f, 1f));

        [MenuItem("Assets/Folder Color/Orange", false, 2001)]
        private static void SetOrange() => SetSelectedFoldersColor(new Color(1.00f, 0.55f, 0.13f, 1f));

        [MenuItem("Assets/Folder Color/Yellow", false, 2002)]
        private static void SetYellow() => SetSelectedFoldersColor(new Color(1.00f, 0.85f, 0.20f, 1f));

        [MenuItem("Assets/Folder Color/Green", false, 2003)]
        private static void SetGreen() => SetSelectedFoldersColor(new Color(0.30f, 0.75f, 0.32f, 1f));

        [MenuItem("Assets/Folder Color/Blue", false, 2004)]
        private static void SetBlue() => SetSelectedFoldersColor(new Color(0.25f, 0.50f, 0.95f, 1f));

        [MenuItem("Assets/Folder Color/Purple", false, 2005)]
        private static void SetPurple() => SetSelectedFoldersColor(new Color(0.58f, 0.36f, 0.95f, 1f));

        [MenuItem("Assets/Folder Color/Gray", false, 2006)]
        private static void SetGray() => SetSelectedFoldersColor(new Color(0.48f, 0.52f, 0.58f, 1f));

        [MenuItem("Assets/Folder Color/Pink", false, 2007)]
        private static void SetPink() => SetSelectedFoldersColor(new Color(0.91f, 0.24f, 0.55f, 1f));

        [MenuItem("Assets/Folder Color/Cyan", false, 2008)]
        private static void SetCyan() => SetSelectedFoldersColor(new Color(0.08f, 0.78f, 0.91f, 1f));

        [MenuItem("Assets/Folder Color/Teal", false, 2009)]
        private static void SetTeal() => SetSelectedFoldersColor(new Color(0.08f, 0.72f, 0.65f, 1f));

        [MenuItem("Assets/Folder Color/Brown", false, 2010)]
        private static void SetBrown() => SetSelectedFoldersColor(new Color(0.60f, 0.35f, 0.16f, 1f));

        [MenuItem("Assets/Folder Color/Black", false, 2011)]
        private static void SetBlack() => SetSelectedFoldersColor(new Color(0.12f, 0.14f, 0.18f, 1f));

        [MenuItem("Assets/Folder Color/Clear", false, 2020)]
        private static void Clear() => ClearSelectedFoldersColor();

        [MenuItem("Assets/Folder Color/Red", true)]
        [MenuItem("Assets/Folder Color/Orange", true)]
        [MenuItem("Assets/Folder Color/Yellow", true)]
        [MenuItem("Assets/Folder Color/Green", true)]
        [MenuItem("Assets/Folder Color/Blue", true)]
        [MenuItem("Assets/Folder Color/Purple", true)]
        [MenuItem("Assets/Folder Color/Gray", true)]
        [MenuItem("Assets/Folder Color/Pink", true)]
        [MenuItem("Assets/Folder Color/Cyan", true)]
        [MenuItem("Assets/Folder Color/Teal", true)]
        [MenuItem("Assets/Folder Color/Brown", true)]
        [MenuItem("Assets/Folder Color/Black", true)]
        [MenuItem("Assets/Folder Color/Clear", true)]
        private static bool ValidateFolderColorMenu() => GetSelectedFolderPaths().Count > 0;

        private static void SetSelectedFoldersColor(Color color)
        {
            List<string> folderPaths = GetSelectedFolderPaths();

            foreach (string path in folderPaths)
            {
                ColorsByPath[path] = color;
            }

            SaveColors();
            EditorApplication.RepaintProjectWindow();
        }

        private static void ClearSelectedFoldersColor()
        {
            List<string> folderPaths = GetSelectedFolderPaths();

            foreach (string path in folderPaths)
            {
                ColorsByPath.Remove(path);
            }

            SaveColors();
            EditorApplication.RepaintProjectWindow();
        }

        private static List<string> GetSelectedFolderPaths()
        {
            var folderPaths = new List<string>();

            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (!string.IsNullOrEmpty(path) &&
                    AssetDatabase.IsValidFolder(path) &&
                    !folderPaths.Contains(path))
                {
                    folderPaths.Add(path);
                }
            }

            foreach (Object selectedObject in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(selectedObject);

                if (!string.IsNullOrEmpty(path) &&
                    AssetDatabase.IsValidFolder(path) &&
                    !folderPaths.Contains(path))
                {
                    folderPaths.Add(path);
                }
            }

            return folderPaths;
        }

        private static void DrawFolderColor(string guid, Rect selectionRect)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!AssetDatabase.IsValidFolder(path) || !ColorsByPath.TryGetValue(path, out Color color))
            {
                return;
            }

            Rect iconRect = GetFolderIconRect(selectionRect);

            Texture2D folderTexture = GetFolderTexture(color, Mathf.RoundToInt(iconRect.width));
            GUI.DrawTexture(iconRect, folderTexture, ScaleMode.ScaleToFit, true);
        }

        private static Rect GetFolderIconRect(Rect selectionRect)
        {
            if (selectionRect.height <= 20f)
            {
                return new Rect(
                    selectionRect.x,
                    selectionRect.y + Mathf.Max(0f, (selectionRect.height - ListIconSize) * 0.5f),
                    ListIconSize,
                    ListIconSize);
            }

            float size = Mathf.Min(selectionRect.width, selectionRect.height - 18f);
            size = Mathf.Clamp(size, 24f, 64f);

            return new Rect(
                selectionRect.x + (selectionRect.width - size) * 0.5f,
                selectionRect.y + 2f,
                size,
                size);
        }

        private static Texture2D GetFolderTexture(Color color, int size)
        {
            size = Mathf.Clamp(size, 16, 64);

            string key = $"{ColorUtility.ToHtmlStringRGBA(color)}-{size}";

            if (FolderTextures.TryGetValue(key, out Texture2D cachedTexture) && cachedTexture != null)
            {
                return cachedTexture;
            }

            Texture2D texture = CreateFolderTexture(color, size);
            FolderTextures[key] = texture;
            return texture;
        }

        private static Texture2D CreateFolderTexture(Color color, int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                hideFlags = HideFlags.HideAndDontSave,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            Color transparent = new Color(0f, 0f, 0f, 0f);
            Color bodyColor = color;
            Color tabColor = Color.Lerp(color, Color.white, 0.18f);
            Color highlightColor = Color.Lerp(color, Color.white, 0.35f);
            Color shadowColor = Color.Lerp(color, Color.black, EditorGUIUtility.isProSkin ? 0.40f : 0.25f);

            bodyColor.a = 0.95f;
            tabColor.a = 0.95f;
            highlightColor.a = 0.80f;
            shadowColor.a = 0.85f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float px = x / (float)size;
                    float py = 1f - (y / (float)size);

                    bool inTab = IsInsideRoundedRect(px, py, 0.10f, 0.16f, 0.48f, 0.36f, 0.06f);
                    bool inBody = IsInsideRoundedRect(px, py, 0.05f, 0.27f, 0.95f, 0.90f, 0.08f);

                    if (!inTab && !inBody)
                    {
                        texture.SetPixel(x, y, transparent);
                        continue;
                    }

                    bool isEdge = IsFolderEdge(px, py);
                    bool isTopBand = inBody && py >= 0.27f && py <= 0.39f;

                    Color pixelColor = inTab ? tabColor : bodyColor;

                    if (isTopBand)
                    {
                        pixelColor = Color.Lerp(pixelColor, highlightColor, 0.40f);
                    }

                    if (isEdge)
                    {
                        pixelColor = shadowColor;
                    }

                    texture.SetPixel(x, y, pixelColor);
                }
            }

            texture.Apply();
            return texture;
        }

        private static bool IsFolderEdge(float x, float y)
        {
            bool bodyEdge = IsNearRoundedRectEdge(x, y, 0.05f, 0.27f, 0.95f, 0.90f, 0.08f, 0.018f);
            bool tabEdge = IsNearRoundedRectEdge(x, y, 0.10f, 0.16f, 0.48f, 0.36f, 0.06f, 0.018f);
            return bodyEdge || tabEdge;
        }

        private static bool IsNearRoundedRectEdge(float x, float y, float minX, float minY, float maxX, float maxY, float radius, float width)
        {
            bool insideOuter = IsInsideRoundedRect(x, y, minX, minY, maxX, maxY, radius);
            bool insideInner = IsInsideRoundedRect(x, y, minX + width, minY + width, maxX - width, maxY - width, Mathf.Max(0f, radius - width));
            return insideOuter && !insideInner;
        }

        private static bool IsInsideRoundedRect(float x, float y, float minX, float minY, float maxX, float maxY, float radius)
        {
            float closestX = Mathf.Clamp(x, minX + radius, maxX - radius);
            float closestY = Mathf.Clamp(y, minY + radius, maxY - radius);
            float dx = x - closestX;
            float dy = y - closestY;

            return x >= minX && x <= maxX && y >= minY && y <= maxY && dx * dx + dy * dy <= radius * radius;
        }

        private static void LoadColors()
        {
            ColorsByPath.Clear();

            string json = EditorPrefs.GetString(PrefsKey, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            FolderColorData data = JsonUtility.FromJson<FolderColorData>(json);

            if (data?.entries == null)
            {
                return;
            }

            foreach (FolderColorEntry entry in data.entries)
            {
                if (!string.IsNullOrEmpty(entry.path))
                {
                    ColorsByPath[entry.path] = entry.color;
                }
            }
        }

        private static void SaveColors()
        {
            var data = new FolderColorData();

            foreach (KeyValuePair<string, Color> colorEntry in ColorsByPath)
            {
                data.entries.Add(new FolderColorEntry
                {
                    path = colorEntry.Key,
                    color = colorEntry.Value
                });
            }

            EditorPrefs.SetString(PrefsKey, JsonUtility.ToJson(data));
        }

        [Serializable]
        private sealed class FolderColorData
        {
            public List<FolderColorEntry> entries = new List<FolderColorEntry>();
        }

        [Serializable]
        private sealed class FolderColorEntry
        {
            public string path;
            public Color color;
        }
    }
}
#endif
