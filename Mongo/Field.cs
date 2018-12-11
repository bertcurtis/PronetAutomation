using MongoDB.Bson;
using System.Collections.Generic;

namespace PronetAutomation.Mongo
{
    public static class Field
    {

        public const string EmailAddress = "emailAddress";
        public const string EnvironmentCreatedOn = "environmentCreatedOn";
        public const string HasAdminPermissions = "hasAdminPermissions";
        public const string ProjectQuantity = "projectQuantity";
        public const string PaymentMethods = "paymentMethods";


        public static string[] DocumentFields =
        {
             EmailAddress,
             EnvironmentCreatedOn,
             HasAdminPermissions,
             ProjectQuantity,
             PaymentMethods
        };

        public static BsonDocument NewDocument(BsonDocumentValues values)
        {
            Dictionary<string, object> bsonDictionary = new Dictionary<string, object>();

            bsonDictionary.Add(EmailAddress, values.EmailAddress);
            bsonDictionary.Add(EnvironmentCreatedOn, values.EnvironmentCreated);
            bsonDictionary.Add(HasAdminPermissions, values.HasAdminPermissions);
            bsonDictionary.Add(ProjectQuantity, values.ProjectQuantity);
            bsonDictionary.Add(PaymentMethods, values.PaymentMethods);

            return new BsonDocument(bsonDictionary);
        }
    }
}