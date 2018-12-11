using Common;
using Common.Mongo;
using System.Collections.Generic;

namespace PronetAutomation.Mongo
{
    public static class MongoDriverExtensions
    {
        public static string GetUserEmail(this MongoDriver mongo, Dictionary<string, object> filterFieldAndValue, bool isFirstInArray = true)
        {
            return mongo.GetValueFromDocument(filterFieldAndValue, Field.EmailAddress, isFirstInArray).ToString();
        }
        public static string GetSocialCredentials(this MongoDriver mongo, string credential, bool isFirstInArray = true)
        {
            return mongo.GetValueFromDocument(new Dictionary<string, object> { { "SocialCredentials", "PronetAutomation" } }, credential, isFirstInArray).ToString();
        }
        public static string GetRandomUserEmail(this MongoDriver mongo)
        {
            return mongo.GetValueFromDocument(new Dictionary<string, object> { { Field.EnvironmentCreatedOn, BaseConfiguration.Base } }, Field.EmailAddress, false).ToString();
        }
        public static void DeleteFromMongoDb(this MongoDriver mongo, string emailAddress)
        {
            mongo.DeleteDocument(new Dictionary<string, object> { { Field.EmailAddress, emailAddress } });
        }
    }
}