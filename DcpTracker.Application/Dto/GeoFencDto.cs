using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace DcpTracker.Application.Dto
{
    public class GeoFencDto : BaseDto<Guid>
    {
        public Guid DeviceId { get; set; }
        public Guid CompanyId { get; set; }
        [JsonIgnore]
        public Polygon Area { get; set; }
        public string RestrictedArea { get; set; }

        public List<double[]> RestrictedJsonArea
        {
            get
            {
                if (Area == null) return new List<double[]>();

                var coords = new List<double[]>();
                foreach (var c in Area.Coordinates)
                {
                    coords.Add([c.Y, c.X]);
                }

                if (coords.Count > 1 && coords[0][0] == coords[^1][0] && coords[0][1] == coords[^1][1])
                    coords.RemoveAt(coords.Count - 1);

                return coords;
            }
        }
    }
}
