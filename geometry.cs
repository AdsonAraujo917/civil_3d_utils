public static class Geometry
{
    public static double GetTriangleArea(Point3d p1, Point3d p2, Point3d p3) 
    {
        Vector3d ab = p2 - p1;
        Vector3d ac = p3 - p1;
        Vector3d cross = ab.CrossProduct(ac);
        return 0.5 * cross.Length;
    }
}
