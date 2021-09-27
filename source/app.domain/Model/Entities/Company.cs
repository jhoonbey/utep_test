namespace app.domain.Model.Entities
{
    public class Company : ModelBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Voen { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }

        public int CountryId { get; set; }
        public int CityId { get; set; }
    }
}