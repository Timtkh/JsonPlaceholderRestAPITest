using RESTAPITest.TestSolution.Interfaces;

namespace RESTAPITest.TestSolution.Models
{
    public class PostModel : IModel
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}