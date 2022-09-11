using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public static class EditorHelper
{
    public static readonly float WideButtonWidth = EditorGUIUtility.singleLineHeight * 2f;
    public static readonly float NarrowButtonWidth = EditorGUIUtility.singleLineHeight * 1.2f;
    public static readonly float LineHeight = EditorGUIUtility.singleLineHeight;
    public static readonly float ElementSpace = 5f;
    public static readonly Color32 DetailFoldoutBackerCol = new Color32(41, 45, 62, 255); //new Color32(37, 34, 102, 255);
    public static readonly Color32 GroupBoxCol = new Color32(64, 64, 64, 255);

    public static Rect Header(string Text, float Offset = 0f)
    {
        Rect Rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, EditorStyles.foldout);
        Rect.x += Offset;
        Rect.width -= Offset;
        EditorGUI.DrawRect(EditorGUI.IndentedRect(Rect), new Color(1f, 1f, 1f, 0.1f));
        Rect.x += (EditorGUIUtility.singleLineHeight / 3) - Offset;
        EditorGUI.LabelField(Rect, Text, EditorStyles.boldLabel);
        return Rect;
    }

    public static bool Toggle(string VariableName, bool Toggled)
    {
        using (new GUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField(VariableName, GUILayout.Width(180));
            GUILayout.FlexibleSpace();
            Toggled = EditorGUILayout.Toggle(Toggled, GUILayout.Width(15 * (EditorGUI.indentLevel + 1)));
        }
        return Toggled;
    }

    public static bool Foldout(bool FoldState, string Content, float Offset = 0f)
    {
        Rect Rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, EditorStyles.foldout);
        Rect.x += Offset;
        Rect.width -= Offset;

        Rect.x += 10;
        EditorGUI.DrawRect(EditorGUI.IndentedRect(Rect), new Color(1f, 1f, 1f, 0.1f));
        Rect.x -= 10;

        FoldState = EditorGUI.Foldout(Rect, FoldState, "", true);
        Rect.x += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(Rect, Content, EditorStyles.boldLabel);
        return FoldState;
    }

    public static bool Foldout(bool FoldState, string Content, float LabelOffset, float Offset = 0f)
    {
        Rect Rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, EditorStyles.foldout);
        Rect.x += Offset;
        Rect.width -= Offset;

        Rect.xMax += LabelOffset;
        Rect.x += 10;
        EditorGUI.DrawRect(EditorGUI.IndentedRect(Rect), new Color(1f, 1f, 1f, 0.1f));
        Rect.x -= 10;
        Rect.xMax -= LabelOffset;

        FoldState = EditorGUI.Foldout(Rect, FoldState, "", true);
        Rect.x += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(Rect, Content, EditorStyles.boldLabel);
        return FoldState;
    }

    public static bool Foldout(bool FoldState, GUIContent Content, float Offset = 0f)
    {
        Rect Rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, EditorStyles.foldout);
        Rect.x += Offset;
        Rect.width -= Offset;

        Rect.width -= 10;
        Rect.x += 10;
        EditorGUI.DrawRect(EditorGUI.IndentedRect(Rect), new Color(1f, 1f, 1f, 0.1f));
        Rect.x -= 10;
        Rect.width += 10;

        FoldState = EditorGUI.Foldout(Rect, FoldState, "", true);
        Rect.x += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(Rect, Content, EditorStyles.boldLabel);
        return FoldState;
    }

    public static bool Foldout(bool FoldState, GUIContent Content, GUIStyle GUIStyle, float Offset = 0f)
    {
        Rect Rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, EditorStyles.foldout);
        Rect.x += Offset;
        Rect.width -= Offset;

        Rect.width -= 10;
        Rect.x += 10;
        EditorGUI.DrawRect(EditorGUI.IndentedRect(Rect), new Color(1f, 1f, 1f, 0.1f));
        Rect.x -= 10;
        Rect.width += 10;

        FoldState = EditorGUI.Foldout(Rect, FoldState, "", true);
        Rect.x += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(Rect, Content, GUIStyle);
        return FoldState;
    }

    public static bool Foldout(bool FoldState, string Content, GUIStyle GUIStyle, float Offset = 0f)
    {
        Rect Rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, EditorStyles.foldout);
        Rect.x += Offset;
        Rect.width -= Offset;

        Rect.width -= 10;
        Rect.x += 10;
        EditorGUI.DrawRect(EditorGUI.IndentedRect(Rect), new Color(1f, 1f, 1f, 0.1f));
        Rect.x -= 10;
        Rect.width += 10;

        FoldState = EditorGUI.Foldout(Rect, FoldState, "", true);
        Rect.x += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(Rect, Content, GUIStyle);
        return FoldState;
    }

    public static Vector2 MinMaxSlider(GUIContent Label, Vector2 MinMax)
    {
        float MinWander = MinMax.x;
        float MaxWander = MinMax.y;

        Rect[] SplitRect = EditorHelper.SplitRect(EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), Label), 3);

        int Padding = (int)SplitRect[0].width - 40;
        int Spacing = 5;

        SplitRect[0].width -= Padding + Spacing;

        SplitRect[1].x -= Padding;
        SplitRect[1].width += Padding * 2;

        SplitRect[2].width -= Padding + Spacing;
        SplitRect[2].x += Padding + Spacing;

        MinWander = EditorGUI.FloatField(SplitRect[0], float.Parse(MinWander.ToString("F2")));
        MaxWander = EditorGUI.FloatField(SplitRect[2], float.Parse(MaxWander.ToString("F2")));
        EditorGUI.MinMaxSlider(SplitRect[1], ref MinWander, ref MaxWander, 0f, 10f);

        MinWander = Mathf.Max(MinWander, 0f);
        MaxWander = Mathf.Min(MaxWander, 10f);
        MaxWander = Mathf.Max(MaxWander, 0f);
        MinWander = Mathf.Min(MinWander, MaxWander);
        MaxWander = Mathf.Max(MaxWander, MinWander);

        MinMax = new Vector2(MinWander, MaxWander);
        return MinMax;
    }

    public static GUIStyle GetColouredStyle(Color Colour)
    {
        GUIStyle Style = new GUIStyle();
        Texture2D Texture = new Texture2D(1, 1);
        Texture.SetPixel(0, 0, Colour);
        Texture.Apply();
        Style.normal.background = Texture;
        return Style;
    }

    public static GUIStyle GetColouredStyle(GUIStyle StyleTag, Color Colour)
    {
        GUIStyle Style = new GUIStyle(StyleTag);
        Texture2D Texture = new Texture2D(1, 1);
        Texture.SetPixel(0, 0, Colour);
        Texture.Apply();
        Style.normal.background = Texture;
        return Style;
    }

    public static GUIStyle SetColouredStyle(GUIStyle Style, Color Colour)
    {
        Texture2D Texture = new Texture2D(1, 1);
        Texture.SetPixel(0, 0, Colour);
        Texture.Apply();
        Style.normal.background = Texture;
        return Style;
    }

    public static void SetColouredStyle(ref GUIStyle Style, Color Colour)
    {
        Texture2D Texture = new Texture2D(1, 1);
        Texture.SetPixel(0, 0, Colour);
        Texture.Apply();
        Style.normal.background = Texture;
    }

    public static bool GUIButtonTextured(Rect Rect, Texture2D Texture)
    {
        bool Out = GUI.Button(Rect, "");
        GUI.DrawTexture(Rect, Texture);
        return Out;
    }

    public static bool GUIButtonTextured(Rect Rect, Texture2D Texture, float TexScale)
    {
        bool Out = GUI.Button(Rect, "");
        Rect.size = new Vector2(TexScale, TexScale);
        GUI.DrawTexture(Rect, Texture);
        return Out;
    }

    public static bool GUIButtonTextured(Rect Rect, Texture2D Texture, float TexWidth, float TexHeight)
    {
        bool Out = GUI.Button(Rect, "");
        Rect.size = new Vector2(TexWidth, TexHeight);
        GUI.DrawTexture(Rect, Texture);
        return Out;
    }

    public static bool GUILayoutButtonIndented(string Text, float Indent)
    {
        using (var scope = new GUILayout.HorizontalScope())
        {
            GUILayout.Space(Indent);
            if (GUILayout.Button(Text)){
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Evently splits a rect transform into a specified number of evenly spaced segments.
    /// </summary>
    public static Rect[] SplitRect(Rect Rect, int Segments)
    {
        Rect[] Rects = new Rect[Segments];
        for (int i = 0; i < Segments; i++){
            Rects[i] = new Rect(Rect.position.x + (i * Rect.width / Segments), Rect.position.y, Rect.width / Segments, Rect.height);
        }
        return Rects;
    }
}
#endif