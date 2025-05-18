using UnityEngine;

public static class Texture2DExtensions
{
    /// <summary>
    /// Creates a new Sprite from a Texture2D.
    /// </summary>
    /// <param name="tex">Source texture.</param>
    /// <param name="pixelsPerUnit">How many pixels in the texture correspond to one Unity unit.</param>
    /// <param name="pivot">
    /// Normalized pivot point (0–1). Defaults to center (0.5,0.5).
    /// </param>
    /// <param name="meshType">
    /// FullRect = simple quad; Tight = alpha-trimmed mesh.
    /// </param>
    /// <param name="border">
    /// 9-slice border. Defaults to zero (no 9-slice).
    /// </param>
    public static Sprite ToSprite(this Texture2D tex,
                                  float pixelsPerUnit = 100f,
                                  Vector2? pivot = null,
                                  SpriteMeshType meshType = SpriteMeshType.FullRect,
                                  Vector4? border = null)
    {
        if (tex == null) return null;
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Vector2 realPivot = pivot ?? new Vector2(0.5f, 0.5f);
        Vector4 realBorder = border ?? Vector4.zero;

        return Sprite.Create(
            tex,
            rect,
            realPivot,
            pixelsPerUnit,
            extrude: 0,
            meshType: meshType,
            border: realBorder
        );
    }

    /// <summary>
    /// Creates a new Sprite from a Texture2D, copying PPU/rect/pivot/border from an existing Sprite.
    /// </summary>
    /// <param name="tex">Source texture.</param>
    /// <param name="template">Sprite whose settings to copy.</param>
    public static Sprite ToSprite(this Texture2D tex, Sprite template)
    {
        if (tex == null || template == null) return null;
        // Use the template’s rect, pivot, border and pixelsPerUnit
        return Sprite.Create(
            tex,
            template.rect,
            template.pivot,
            template.pixelsPerUnit,
            extrude: 0,
            meshType: SpriteMeshType.FullRect,
            border: template.border
        );
    }
}
