using Aquality.Selenium.Core.Logging;
using Newtonsoft.Json.Linq;
using RESTAPITest.TestSolution.Interfaces;
using RESTAPITest.TestSolution.Models;

namespace RESTAPITest.TestSolution.Tools
{
    public static class ModelParser
    {
        public static IModel ParseToModel(string restResponsContent, string model)
        {
            Logger.Instance.Info("Parse to model");
            var responseJObject = JObject.Parse(restResponsContent);

            switch (model)
            {
                case "PostModel":
                    return new PostModel
                    {
                        UserId = responseJObject.Value<int>("userId"),
                        Id = responseJObject.Value<int>("id"),
                        Title = responseJObject.Value<string>("title"),
                        Body = responseJObject.Value<string>("body")
                    };

                case "UserModel":
                    var parsedJObject = JObject.Parse(responseJObject.ToString());
                    var adress = JObject.Parse(parsedJObject.Value<JToken>("address").ToString());
                    var geo = JObject.Parse(adress.Value<JToken>("geo").ToString());
                    var company = JObject.Parse(parsedJObject.Value<JToken>("company").ToString());

                    return new UserModel
                    {
                        Id = parsedJObject.Value<int>("id"),
                        Name = parsedJObject.Value<string>("name"),
                        UserName = parsedJObject.Value<string>("username"),
                        Email = parsedJObject.Value<string>("email"),
                        Adress = new Adress()
                        {
                            Street = adress.Value<string>("street"),
                            Suite = adress.Value<string>("suite"),
                            City = adress.Value<string>("city"),
                            Zipcode = adress.Value<string>("zipcode"),
                            Geo = new Geo()
                            {
                                Lat = geo.Value<string>("lat"),
                                Lng = geo.Value<string>("lng")
                            }
                        },
                        Phone = parsedJObject.Value<string>("phone"),
                        Website = parsedJObject.Value<string>("website"),
                        Company = new Company()
                        {
                            Name = company.Value<string>("name"),
                            CatchPhrase = company.Value<string>("catchPhrase"),
                            Bs = company.Value<string>("bs"),
                        }
                    };

                default:
                    return null;
            }
        }

        public static List<IModel> ParseToModelList(string restResponsContent, string model)
        {
            Logger.Instance.Info("Parse to model list");
            var responseJArray = JArray.Parse(restResponsContent);

            switch (model)
            {
                case "PostModel":
                    var postModelList = new List<IModel>();

                    foreach (var item in responseJArray)
                    {
                        var parsedJObject = JObject.Parse(item.ToString());

                        postModelList.Add(new PostModel
                        {
                            UserId = parsedJObject.Value<int>("userId"),
                            Id = parsedJObject.Value<int>("id"),
                            Title = parsedJObject.Value<string>("title"),
                            Body = parsedJObject.Value<string>("body")
                        });
                    }

                    return postModelList;

                case "UserModel":
                    var UserModelList = new List<IModel>();

                    foreach (var item in responseJArray)
                    {
                        var parsedJObject = JObject.Parse(item.ToString());
                        var adress = JObject.Parse(parsedJObject.Value<JToken>("address").ToString());
                        var geo = JObject.Parse(adress.Value<JToken>("geo").ToString());
                        var company = JObject.Parse(parsedJObject.Value<JToken>("company").ToString());

                        UserModelList.Add(new UserModel
                        {
                            Id = parsedJObject.Value<int>("id"),
                            Name = parsedJObject.Value<string>("name"),
                            UserName = parsedJObject.Value<string>("username"),
                            Email = parsedJObject.Value<string>("email"),
                            Adress = new Adress()
                            {
                                Street = adress.Value<string>("street"),
                                Suite = adress.Value<string>("suite"),
                                City = adress.Value<string>("city"),
                                Zipcode = adress.Value<string>("zipcode"),
                                Geo = new Geo()
                                {
                                    Lat = geo.Value<string>("lat"),
                                    Lng = geo.Value<string>("lng")
                                }
                            },
                            Phone = parsedJObject.Value<string>("phone"),
                            Website = parsedJObject.Value<string>("website"),
                            Company = new Company()
                            {
                                Name = company.Value<string>("name"),
                                CatchPhrase = company.Value<string>("catchPhrase"),
                                Bs = company.Value<string>("bs"),
                            }
                        });
                    }

                    return UserModelList;

                default:
                    return null;
            }
        }
    }
}