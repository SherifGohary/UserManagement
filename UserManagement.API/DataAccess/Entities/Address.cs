namespace UserManagement.API.DataAccess.Entities
{
    public class Address: BaseEntity<int>
    {
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string GeoLat { get; set; }
        public string GeoLng { get; set; }
    }
}
