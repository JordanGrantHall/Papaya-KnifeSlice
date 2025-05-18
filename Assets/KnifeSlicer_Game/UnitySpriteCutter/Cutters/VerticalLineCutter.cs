using System.Linq;
using UnityEngine;

namespace KnifeSlicer.UnitySpriteCutter.Cutters
{
    /// <summary>
    /// Helper to cut a convex 2D shape with a vertical line at a given xPosition.
    /// </summary>
    public static class VerticalLineCutter
    {
        /// <summary>
        /// Cuts the given convex polygon 'shape' into two at x = xPosition.
        /// </summary>
        /// <param name="shape">Array of vertices (in local or world space) forming a convex polygon.</param>
        /// <param name="xPosition">X coordinate of the vertical cut line.</param>
        /// <returns>
        /// A ShapeCutter.CutResult with two point-lists: firstSidePoints & secondSidePoints.
        /// </returns>
        public static ShapeCutter.CutResult CutShapeAtX(Vector2[] shape, float xPosition)
        {
            if (shape == null || shape.Length < 3)
                throw new System.ArgumentException("Need at least 3 vertices to cut a shape.", nameof(shape));

            // 1) find the min/max Y of the polygon
            float minY = shape.Min(p => p.y);
            float maxY = shape.Max(p => p.y);

            // 2) pad a bit so the line really goes “all the way through”
            const float pad = 0.5f;
            Vector2 lineStart = new Vector2(xPosition, minY - pad);
            Vector2 lineEnd = new Vector2(xPosition, maxY + pad);

            // 3) call the generic infinite‐line cutter
            return ShapeCutter.CutShapeIntoTwo(lineStart, lineEnd, shape);
        }
    }

    public static class VerticalLineCutterForSprite
    {
        /// <summary>
        /// Cuts the given sprite‐GameObject with a vertical line at world X = clickX.
        /// </summary>
        public static SpriteCutterOutput CutAtX(
            GameObject go,
            float clickX,
            bool dontCutColliders = false,
            SpriteCutterInput.GameObjectCreationMode mode = SpriteCutterInput.GameObjectCreationMode.CUT_OFF_NEW)
        {
            // 1) determine the world‐space Y‐bounds of the sprite
            var renderers = go.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return null;
            Bounds b = renderers[0].bounds;
            foreach (var r in renderers) b.Encapsulate(r.bounds);

            // 2) build the cut line
            Vector2 start = new Vector2(clickX, b.min.y - 0.5f);
            Vector2 end = new Vector2(clickX, b.max.y + 0.5f);

            // 3) assemble input and fire
            var input = new SpriteCutterInput
            {
                gameObject = go,
                lineStart = start,
                lineEnd = end,
                dontCutColliders = dontCutColliders,
                gameObjectCreationMode = mode
            };
            return SpriteCutter.Cut(input);
        }
    }
}

namespace KnifeSlicer.UnitySpriteCutter.Tools
{
    /// <summary>
    /// Helper class to calculate texture coordinates for cut sprites while preserving the original mapping
    /// </summary>
    public static class BilinearUVMapping
    {
        /// <summary>
        /// Generates UV coordinates for new vertices by precisely mapping them to the original sprite's UV space
        /// </summary>
        public static Vector2[] GeneratePreservedUVs(Vector2[] newVertices, Mesh originalMesh)
        {
            Vector2[] result = new Vector2[newVertices.Length];
            Vector3[] origVerts = originalMesh.vertices;
            Vector2[] origUVs = originalMesh.uv;

            // For meshes created from sprites, we typically have a quad with 4 vertices
            if (origVerts.Length != 4 || origUVs.Length != 4)
            {
                Debug.LogWarning("Original mesh doesn't have exactly 4 vertices. UV mapping might not be accurate.");
                // Fall back to basic mapping if not a quad
                return BasicUVMapping(newVertices, originalMesh);
            }

            // Get the corners of the original quad
            Vector2 origBL = origVerts[3]; // Bottom-left
            Vector2 origBR = origVerts[2]; // Bottom-right  
            Vector2 origTL = origVerts[0]; // Top-left
            Vector2 origTR = origVerts[1]; // Top-right

            // Get the UVs at those corners
            Vector2 uvBL = origUVs[3]; // Bottom-left
            Vector2 uvBR = origUVs[2]; // Bottom-right
            Vector2 uvTL = origUVs[0]; // Top-left
            Vector2 uvTR = origUVs[1]; // Top-right

            // Calculate the position of each new vertex relative to the original quad
            for (int i = 0; i < newVertices.Length; i++)
            {
                Vector2 pos = newVertices[i];

                // Calculate bilinear coordinates (u,v) where:
                // u = 0 at left edge, u = 1 at right edge
                // v = 0 at bottom edge, v = 1 at top edge

                // First get the dimensions of the original quad
                float width = Vector2.Distance(origBL, origBR);
                float height = Vector2.Distance(origBL, origTL);

                // Calculate the local basis vectors of the original quad
                Vector2 rightDir = (origBR - origBL).normalized;
                Vector2 upDir = (origTL - origBL).normalized;

                // Calculate offsets from bottom-left corner
                Vector2 offset = pos - origBL;

                // Project the offset onto the basis vectors to get u,v
                float u = Vector2.Dot(offset, rightDir) / width;
                float v = Vector2.Dot(offset, upDir) / height;

                // Clamp to avoid extrapolation issues
                u = Mathf.Clamp01(u);
                v = Mathf.Clamp01(v);

                // Use bilinear interpolation to find the corresponding UV
                Vector2 bottomUV = Vector2.Lerp(uvBL, uvBR, u);
                Vector2 topUV = Vector2.Lerp(uvTL, uvTR, u);
                result[i] = Vector2.Lerp(bottomUV, topUV, v);
            }

            return result;
        }

        // Fallback method that uses basic mapping
        private static Vector2[] BasicUVMapping(Vector2[] newVertices, Mesh originalMesh)
        {
            Vector2[] result = new Vector2[newVertices.Length];

            // Find the bounds of the original mesh
            Bounds bounds = new Bounds(originalMesh.vertices[0], Vector3.zero);
            for (int i = 1; i < originalMesh.vertices.Length; i++)
            {
                bounds.Encapsulate(originalMesh.vertices[i]);
            }

            // Map vertices based on their position within the bounds
            for (int i = 0; i < newVertices.Length; i++)
            {
                result[i] = new Vector2(
                    Mathf.InverseLerp(bounds.min.x, bounds.max.x, newVertices[i].x),
                    Mathf.InverseLerp(bounds.min.y, bounds.max.y, newVertices[i].y)
                );
            }

            return result;
        }
    }
}