using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace RESTAPITest.TestSolution.Pages
{
    public class JsonPlaceholderPage : Form
    {
        public JsonPlaceholderPage() : base(By.Id("run-button"),  "Run button") { }
    }
}