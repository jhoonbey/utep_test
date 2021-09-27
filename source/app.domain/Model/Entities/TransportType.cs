namespace app.domain.Model.Entities
{
    public class TransportType : ModelBase
    {
        public string Name { get; set; }
        public string WeightCapacity { get; set; }
        public string VolumeCapacity { get; set; }
        public string CapacityInPalletes { get; set; }
        public string Description { get; set; }
    }
}