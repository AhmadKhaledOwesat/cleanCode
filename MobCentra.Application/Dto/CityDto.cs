using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace MobCentra.Application.Dto
{
    public class CityDto : BaseDto<Guid>
    {
        public string Name { get; set; }
        public string NameOt { get; set; }
        public string Location { get; set; }
        public Guid? CompanyId { get; set; }
        public string CreatedByName { get; set; }
        [JsonIgnore]
        public Polygon Area { get; set; }
        public string RestrictedArea { get; set; }
        public List<double[]> RestrictedJsonArea
        {
            get
            {
                if (Area == null) return [];

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
