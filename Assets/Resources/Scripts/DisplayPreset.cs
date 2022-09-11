using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DisplayPreset", menuName = "Triangulation/DisplayPreset")]
public class DisplayPreset : ScriptableObject
{
    [SerializeField]
    private Vector2 MinMaxPointScale = new Vector2(0.05f, 0.15f);
    [SerializeField]
    private Vector2 MinMaxWanderSpeed = new Vector2(0.1f, 0.4f);

    [Space]

    [SerializeField]
    private bool DrawEdges = true;
    [SerializeField][Range(0f, 800f)]
    private float EdgeThickness = 1f;

    [Space]

    [SerializeField]
    private float GradientOffset;
    [SerializeField]
    private float GradientStretch;

    [Space]

    [SerializeField]
    private Color EdgeColour = Color.black;
    [SerializeField]
    private Color PointColour = Color.black;
    [SerializeField]
    private Color BgColourA = Color.white;
    [SerializeField]
    private Color BgColourB = Color.white;

    public Vector2 minMaxPointScale { get { return MinMaxPointScale; } }
    public Vector2 minMaxWanderSpeed { get { return MinMaxWanderSpeed; } }
    public bool drawEdges { get { return DrawEdges; } }
    public float edgeThickness { get { return EdgeThickness; } }
    public float gradientOffset { get { return GradientOffset; } }
    public float gradientStretch { get { return GradientStretch; } }
    public Color edgeColour { get { return EdgeColour; } }
    public Color pointColour { get { return PointColour; } }
    public Color bgColourA { get { return BgColourA; } }
    public Color bgColourB { get { return BgColourB; } }

    public event System.Action OnValuesUpdated;

    #if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        EdgeThickness = Mathf.Max(0f, EdgeThickness);
        UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
    }

    public virtual void NotifyOfUpdatedValues()
    {
        UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
        OnValuesUpdated?.Invoke();
    }
    #endif
}