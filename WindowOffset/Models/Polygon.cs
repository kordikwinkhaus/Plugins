using System.Collections.Generic;
using System.Drawing;

namespace WindowOffset.Models
{
    internal static class Polygon
    {
        internal static PointF ComputeCentroid(IList<PointF> vertices)
        {
            PointF centroid = new PointF { X = 0, Y = 0 };
            float signedArea = 0;
            float x0 = 0; // Current vertex X
            float y0 = 0; // Current vertex Y
            float x1 = 0; // Next vertex X
            float y1 = 0; // Next vertex Y
            float a = 0;  // Partial signed area

            // For all vertices except last
            int i=0;
            for (i = 0; i < vertices.Count - 1; ++i)
            {
                x0 = vertices[i].X;
                y0 = vertices[i].Y;
                x1 = vertices[i + 1].X;
                y1 = vertices[i + 1].Y;
                a = x0 * y1 - x1 * y0;
                signedArea += a;
                centroid.X += (x0 + x1) * a;
                centroid.Y += (y0 + y1) * a;
            }

            // Do last vertex
            x0 = vertices[i].X;
            y0 = vertices[i].Y;
            x1 = vertices[0].X;
            y1 = vertices[0].Y;
            a = x0 * y1 - x1 * y0;
            signedArea += a;
            centroid.X += (x0 + x1) * a;
            centroid.Y += (y0 + y1) * a;

            signedArea *= 0.5f;
            centroid.X /= (6 * signedArea);
            centroid.Y /= (6 * signedArea);

            return centroid;
        }
    }
}
