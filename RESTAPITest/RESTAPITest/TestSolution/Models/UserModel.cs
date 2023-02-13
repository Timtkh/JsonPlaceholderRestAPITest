using RESTAPITest.TestSolution.Interfaces;

namespace RESTAPITest.TestSolution.Models
{
    public class UserModel : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Adress Adress { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Company Company { get; set; }

    }
}