using NetTopologySuite.Geometries;
using ProjNet;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace Service.ManagerVPS.Extensions
{
    
    public static class GeometryExtensions
    {
        public static int PROJECT_SRID = 3405;
        private static readonly CoordinateSystemServices _coordinateSystemServices
            = new CoordinateSystemServices(
                new Dictionary<int, string>
                {
                    // Coordinate systems:

                    [4326] = GeographicCoordinateSystem.WGS84.WKT,

                    // This coordinate system covers the area of our data.
                    // Different data requires a different coordinate system.
                    [3405] =
                        @"
                        PROJCS[""VN-2000 / UTM zone 48N"",
    GEOGCS[""VN-2000"",
        DATUM[""Vietnam_2000"",
            SPHEROID[""WGS 84"",6378137,298.257223563],
            TOWGS84[-192.873,-39.382,-111.202,0.00205,0.0005,-0.00335,0.0188]],
        PRIMEM[""Greenwich"",0,
            AUTHORITY[""EPSG"",""8901""]],
        UNIT[""degree"",0.0174532925199433,
            AUTHORITY[""EPSG"",""9122""]],
        AUTHORITY[""EPSG"",""4756""]],
    PROJECTION[""Transverse_Mercator""],
    PARAMETER[""latitude_of_origin"",0],
    PARAMETER[""central_meridian"",105],
    PARAMETER[""scale_factor"",0.9996],
    PARAMETER[""false_easting"",500000],
    PARAMETER[""false_northing"",0],
    UNIT[""metre"",1,
        AUTHORITY[""EPSG"",""9001""]],
    AXIS[""Easting"",EAST],
    AXIS[""Northing"",NORTH],
    AUTHORITY[""EPSG"",""3405""]]
                    "
                });

        public static Geometry ProjectTo(this Geometry geometry, int srid)
        {
            var transformation = _coordinateSystemServices.CreateTransformation(geometry.SRID, srid);

            var result = geometry.Copy();
            result.Apply(new MathTransformFilter(transformation.MathTransform));

            return result;
        }

        private class MathTransformFilter : ICoordinateSequenceFilter
        {
            private readonly MathTransform _transform;

            public MathTransformFilter(MathTransform transform)
                => _transform = transform;

            public bool Done => false;
            public bool GeometryChanged => true;

            public void Filter(CoordinateSequence seq, int i)
            {
                var x = seq.GetX(i);
                var y = seq.GetY(i);
                var z = seq.GetZ(i);
                _transform.Transform(ref x, ref y, ref z);
                seq.SetX(i, x);
                seq.SetY(i, y);
                seq.SetZ(i, z);
            }
        }
    }
}
