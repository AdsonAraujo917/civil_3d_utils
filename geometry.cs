public static class Geometry
{
    public static double GetTriangleArea(Point3d p1, Point3d p2, Point3d p3) 
    {
        Vector3d v1 = p2 - p1;
        Vector3d v2 = p3 - p1;
        Vector3d crossProduct = v1.CrossProduct(v2);
        return 0.5 * crossProduct.Length;
    }
}
