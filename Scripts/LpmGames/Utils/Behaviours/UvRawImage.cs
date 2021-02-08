using LpmGames.Utils.Debug;
using UnityEngine;
using UnityEngine.UI;

namespace LpmGames.Utils.Behaviours
{
    public class UvRawImage : RawImage
    {
        public Vector2[] Uvs = {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };
        
        private static Vector2 RotateUv(Vector2 uv) =>  new Vector2(uv.y, -uv.x);
        
        public void UpdateUvs(bool rotated, float width, float height, float xPosition, float yPosition)
        {
            // Steps:
            // 1 - Offset to -0.5,0.5
            // 2 - Rotate if needed. We do this inline.
            // 3 - Scale to appropriate size
            // 4 - Transform to final
            
            var frameSize = new Vector2(
                width,
                height
            );
            var framePosition = new Vector2(
                xPosition + frameSize.x/2, 
                1 - (yPosition + frameSize.y/2)
            );
            var baseOffset = new Vector2(-0.5f, -0.5f);
            
            var uvs = new[]{
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0)
            };
            
            for (var i = 0; i < uvs.Length; i++)
            {
                var uv = uvs[i];
                
                uv += baseOffset;
                if (rotated)
                    uv = RotateUv(uv);
                uv *= frameSize;
                uv += framePosition;
                
                uvs[i] = uv;
            }
            Uvs = uvs;
            SetVerticesDirty();
        }
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (mainTexture == null) return;
            
            var r = GetPixelAdjustedRect();
            var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);
            
            var color32 = color;
            vh.AddVert(new Vector3(v.x, v.y), color32, Uvs[0]);
            vh.AddVert(new Vector3(v.x, v.w), color32, Uvs[1]);
            vh.AddVert(new Vector3(v.z, v.w), color32, Uvs[2]);
            vh.AddVert(new Vector3(v.z, v.y), color32, Uvs[3]);

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }
    }
}
