using Aquality.Selenium.Core.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RESTAPITest.Framework.Utils;
using RESTAPITest.TestSolution.Interfaces;
using RESTAPITest.TestSolution.Models;
using RESTAPITest.TestSolution.Pages;
using RESTAPITest.TestSolution.Tools;
using RestSharp;
using System.Net;

namespace RESTAPITest.TestSolution.Tests
{
    public class JSonPlaceholderAPITest : BaseTest
    {
        private static readonly string postsUrl = $"{BrowserConfigManager.PostsUrl}";
        private readonly string isJsonValue = "application/json";
        private readonly int isJsonIndex = -1;
        private static readonly string postId = "99";
        private readonly string postById = $"{postsUrl}/{postId}";
        private static readonly string notExistPostId = "150";
        private readonly string postByNotExistId = $"{postsUrl}/{notExistPostId}";
        private readonly int userIdFromPosts = 10;
        private readonly int Id = 99;
        private readonly static string randomTitle = RandomUtil.GetRandomLatinString();
        private readonly static string randomBody = RandomUtil.GetRandomLatinString();
        private readonly static string postUserId = "1";
        private readonly JObject recordForPost = new()
        {
                { "userId", postUserId },
                { "title", randomTitle },
                { "body", randomBody }
            };
        private readonly string postedRecordId = "101";
        private readonly static string usersUrl = $"{BrowserConfigManager.UsersUrl}";
        private readonly static UserModel userIdFive = new()
        {
            Id = 5,
            Name = "Chelsey Dietrich",
            UserName = "Kamren",
            Email = "Lucio_Hettinger@annie.ca",
            Adress = new Adress()
            {
                Street = "Skiles Walks",
                Suite = "Suite 351",
                City = "Roscoeview",
                Zipcode = "33263",
                Geo = new Geo()
                {
                    Lat = "-31.8129",
                    Lng = "62.5342"
                }
            },
            Phone = "(254)954-1289",
            Website = "demarco.info",
            Company = new Company()
            {
                Name = "Keebler LLC",
                CatchPhrase = "User-centric fault-tolerant solution",
                Bs = "revolutionize end-to-end systems"
            }
        };
        private readonly string userById = $"{usersUrl}/{userIdFive.Id}";
        private readonly string postModelType = "PostModel";
        private readonly string userModelType = "UserModel";

        [Test]
        public void Run_GetAllPostsTest()
        {
            Logger.Instance.Info("Open JsonPlaceholder main page, step 1 - get all posts");
            var userinyerfaceMainPage = new JsonPlaceholderPage();
            Assert.That(userinyerfaceMainPage.State.IsDisplayed, Is.True, "JsonPlaceholder main page is not opened");

            var client = new RestClient(postsUrl);
            var response = APIUtil.GetRestResponse(client);

            var postsResponseContentModelsList = ModelParser.ParseToModelList(response.Content, postModelType);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not OK");
                Assert.That(response.ContentType.IndexOf(isJsonValue), Is.Not.EqualTo(isJsonIndex), "Response content is not json");
                Assert.That(postsResponseContentModelsList, Is.Ordered.By("Id"), "Posts is not sorted");
            });

            Logger.Instance.Info("Step 2 - get post by id");
            client = new RestClient(postById);
            response = APIUtil.GetRestResponse(client);

            var postByIdresponseContentModel = (PostModel)ModelParser.ParseToModel(response.Content, postModelType);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not OK");
                Assert.That(postByIdresponseContentModel.UserId, Is.EqualTo(userIdFromPosts), $"Received data userId is not equal to {postUserId}");
                Assert.That(postByIdresponseContentModel.Id, Is.EqualTo(Id), $"Received data id is not equal to {Id}");
                Assert.That(postByIdresponseContentModel.Title, Is.Not.Empty, "Received data title is empty");
                Assert.That(postByIdresponseContentModel.Body, Is.Not.Empty, "Received data body is empty");
            });

            Logger.Instance.Info("Step 3 - get non-existent post");
            client = new RestClient(postByNotExistId);
            response = APIUtil.GetRestResponse(client);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Status code is not NotFound");

            Logger.Instance.Info("Step 4 - send post model");
            client = new RestClient(postsUrl);

            response = APIUtil.PostDataToClient(client, recordForPost);

            var postedResponseContentModel = (PostModel)ModelParser.ParseToModel(response.Content, postModelType);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Status code is not Created");
                Assert.That(postedResponseContentModel.Id, Is.EqualTo(Int32.Parse(postedRecordId)), "Received record id is not equal posted");
                Assert.That(postedResponseContentModel.UserId, Is.EqualTo(Int32.Parse(postUserId)), "Received record userId is not equal posted");
                Assert.That(postedResponseContentModel.Title, Is.EqualTo(randomTitle), "Received record title is not equal posted");
                Assert.That(postedResponseContentModel.Body, Is.EqualTo(randomBody), "Received record body is not equal posted");
            });

            Logger.Instance.Info("Step 5 - get all users");
            client = new RestClient(usersUrl);
            response = APIUtil.GetRestResponse(client);

            var usersResponseContentModelsList = ModelParser.ParseToModelList(response.Content, userModelType);

            var userFromResponseList = (UserModel)GetRecordFromList(usersResponseContentModelsList, userIdFive.Id);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not OK");
                Assert.That(response.ContentType.IndexOf(isJsonValue), Is.Not.EqualTo(isJsonIndex), "Response content is not json");
                Assert.That(userFromResponseList.Name, Is.EqualTo(userIdFive.Name), $"Received user name is not equal {userIdFive.Name}");
                Assert.That(userFromResponseList.UserName, Is.EqualTo(userIdFive.UserName), $"Received user UserName is not equal {userIdFive.UserName}");
                Assert.That(userFromResponseList.Email, Is.EqualTo(userIdFive.Email), $"Received user email is not equal {userIdFive.Email}");
                Assert.That(userFromResponseList.Adress.Street, Is.EqualTo(userIdFive.Adress.Street), $"Received user adress street is not equal {userIdFive.Adress.Street}");
                Assert.That(userFromResponseList.Adress.Suite, Is.EqualTo(userIdFive.Adress.Suite), $"Received user adress suite is not equal {userIdFive.Adress.Suite}");
                Assert.That(userFromResponseList.Adress.City, Is.EqualTo(userIdFive.Adress.City), $"Received user adress city is not equal {userIdFive.Adress.City}");
                Assert.That(userFromResponseList.Adress.Zipcode, Is.EqualTo(userIdFive.Adress.Zipcode), $"Received user adress zipcode is not equal {userIdFive.Adress.Zipcode}");
                Assert.That(userFromResponseList.Adress.Geo.Lat, Is.EqualTo(userIdFive.Adress.Geo.Lat), $"Received user adress geo lat is not equal {userIdFive.Adress.Geo.Lat}");
                Assert.That(userFromResponseList.Adress.Geo.Lng, Is.EqualTo(userIdFive.Adress.Geo.Lng), $"Received user adress geo lng is not equal {userIdFive.Adress.Geo.Lng}");
                Assert.That(userFromResponseList.Phone, Is.EqualTo(userIdFive.Phone), $"Received user phone is not equal {userIdFive.Phone}");
                Assert.That(userFromResponseList.Website, Is.EqualTo(userIdFive.Website), $"Received user website is not equal {userIdFive.Website}");
                Assert.That(userFromResponseList.Company.Name, Is.EqualTo(userIdFive.Company.Name), $"Received user company name is not equal {userIdFive.Company.Name}");
                Assert.That(userFromResponseList.Company.CatchPhrase, Is.EqualTo(userIdFive.Company.CatchPhrase), $"Received user company catch phrase is not equal {userIdFive.Company.CatchPhrase}");
                Assert.That(userFromResponseList.Company.Bs, Is.EqualTo(userIdFive.Company.Bs), $"Received user company bs is not equal {userIdFive.Company.Bs}");
            });

            Logger.Instance.Info("Step 6 - get user by id");
            client = new RestClient(userById);
            response = APIUtil.GetRestResponse(client);

            var userModel = (UserModel)ModelParser.ParseToModel(response.Content, userModelType);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code is not OK");
                Assert.That(userModel.Name, Is.EqualTo(userIdFive.Name), $"Received user name is not equal {userIdFive.Name}");
                Assert.That(userModel.UserName, Is.EqualTo(userIdFive.UserName), $"Received user UserName is not equal {userIdFive.UserName}");
                Assert.That(userModel.Email, Is.EqualTo(userIdFive.Email), $"Received user email is not equal {userIdFive.Email}");
                Assert.That(userModel.Adress.Street, Is.EqualTo(userIdFive.Adress.Street), $"Received user adress street is not equal {userIdFive.Adress.Street}");
                Assert.That(userModel.Adress.Suite, Is.EqualTo(userIdFive.Adress.Suite), $"Received user adress suite is not equal {userIdFive.Adress.Suite}");
                Assert.That(userModel.Adress.City, Is.EqualTo(userIdFive.Adress.City), $"Received user adress city is not equal {userIdFive.Adress.City}");
                Assert.That(userModel.Adress.Zipcode, Is.EqualTo(userIdFive.Adress.Zipcode), $"Received user adress zipcode is not equal {userIdFive.Adress.Zipcode}");
                Assert.That(userModel.Adress.Geo.Lat, Is.EqualTo(userIdFive.Adress.Geo.Lat), $"Received user adress geo lat is not equal {userIdFive.Adress.Geo.Lat}");
                Assert.That(userModel.Adress.Geo.Lng, Is.EqualTo(userIdFive.Adress.Geo.Lng), $"Received user adress geo lng is not equal {userIdFive.Adress.Geo.Lng}");
                Assert.That(userModel.Phone, Is.EqualTo(userIdFive.Phone), $"Received user phone is not equal {userIdFive.Phone}");
                Assert.That(userModel.Website, Is.EqualTo(userIdFive.Website), $"Received user website is not equal {userIdFive.Website}");
                Assert.That(userModel.Company.Name, Is.EqualTo(userIdFive.Company.Name), $"Received user company name is not equal {userIdFive.Company.Name}");
                Assert.That(userModel.Company.CatchPhrase, Is.EqualTo(userIdFive.Company.CatchPhrase), $"Received user company catch phrase is not equal {userIdFive.Company.CatchPhrase}");
                Assert.That(userModel.Company.Bs, Is.EqualTo(userIdFive.Company.Bs), $"Received user company bs is not equal {userIdFive.Company.Bs}");
            });
        }

        private static IModel GetRecordFromList(List<IModel> users, int id)
        {
            Logger.Instance.Info("Get record from list");

            foreach (var user in users)
            {
                if (user.Id == id)
                {
                    return user;
                }
            }

            return null;
        }
    }
}