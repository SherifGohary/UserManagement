namespace UserManagement.API.DataAccess.Entities
{
    public class Company: BaseEntity<int>
    {
        public string Name { get; set; }
        public string CatchPhrase { get; set; }
        public string Bs { get; set; }
    }
}
