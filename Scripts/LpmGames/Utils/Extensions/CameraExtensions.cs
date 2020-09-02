using UnityEngine;

namespace Extensions
{
    public static class CameraExtensions
    {
        
        public static float BoundsToOrthographicSize(this Camera camera, Bounds bounds)
        {
            var cameraSize = camera.orthographicSize;
            var screenRatio = Screen.width / (float)Screen.height;

            var rect = camera.BoundsToScreenRect(bounds);
            var scale = rect.height / Screen.height;
            var ySize = scale * cameraSize * 2;
            var xSize = scale * cameraSize / (rect.height / rect.width) * 2;
            
            var targetRatio = xSize / ySize;
 
            if (targetRatio > screenRatio)
            {
                var differenceInSize = targetRatio / screenRatio;
                return ySize / 2 * differenceInSize;
            }

            return ySize / 2f;
        }
        
        // I'm sure this could be more optimized but it works. I will revisit in the future to see if it can be made more
        // elegant.
        public static Rect BoundsToScreenRect(this Camera camera, Bounds bounds)
        {
            var cen = bounds.center;
            var ext = bounds.extents;
            var cam = camera;
            
            Vector2 min = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z - ext.z));
            var max = min;


            //0
            var point = min;
            min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
            max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

            //1
            point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z - ext.z));
            min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
            max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);


            //2
            point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z + ext.z));
            min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
            max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

            //3
            point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z + ext.z));
            min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
            max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

            //4
            point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z - ext.z));
            min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
            max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

            //5
            point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z - ext.z));
            min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
            max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

            //6
            point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z + ext.z));
            min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
            max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

            //7
            point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z + ext.z));
            min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
            max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }
    }
}