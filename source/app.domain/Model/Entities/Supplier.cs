namespace app.domain.Model.Entities
{
    public class Supplier : ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WhatsappNumber { get; set; }
        public string ContactNumber { get; set; }
        public string PlateNumber { get; set; }
        public string PlateNumber2 { get; set; }
        public string TransportPassport { get; set; }
        public string TransportPassport2 { get; set; }
        public string InternationalPassport { get; set; }
        public string DrivingLicense { get; set; }
        public int Year { get; set; }
        public int Condition { get; set; }
        public string TruckFrontPhotoName { get; set; }
        public string TruckBackPhotoName { get; set; }
        public string TruckRightPhotoName { get; set; }
        public string TruckLeftPhotoName { get; set; }


        public string TransportPassportPhotoName { get; set; }
        public string TransportPassport2PhotoName { get; set; }
        public string InternationalPassportPhotoName { get; set; }
        public string DrivingLicensePhotoName { get; set; }


        public int TransportTypeId { get; set; }
        public int CompanyId { get; set; }
    }
}






