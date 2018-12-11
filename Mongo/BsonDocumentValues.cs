using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MongoDB.Bson;
using Common.Mongo;
using Common;

namespace PronetAutomation.Mongo
{
    public class BsonDocumentValues
    {
        public BsonDocumentValues(MongoDriver mongo)
        {
            Mongo = mongo;
            int count = typeof(BsonDocumentValues).GetProperties().Count();
            if (Field.DocumentFields.Length != count)
            {
                throw new Exception("BsonDocumentFields and BsonDocumentValues do not match. Add or take away from either so they match.");
            }

            SetValuesToBlankDocument();
        }
        private MongoDriver Mongo { get; set;  }
        public string EmailAddress { get; set; }
        public string EnvironmentCreated { get; set; }
        public bool HasAdminPermissions { get; set; }
        public int ProjectQuantity { get; set; }
        public string[] PaymentMethods { get; set; }

        /// <summary>
        /// The login infos.
        /// </summary>
        public List<LoginInfo> LoginInfos = new List<LoginInfo>
        {
            new LoginInfo(new Dictionary<string, object> { { Field.EnvironmentCreatedOn, BaseConfiguration.Base } }, LoginType.AnyUser),
            new LoginInfo(new Dictionary<string, object> { { Field.HasAdminPermissions, true }, { Field.EnvironmentCreatedOn, BaseConfiguration.Base } }, LoginType.Admin),
            new LoginInfo(new Dictionary<string, object> { { Field.ProjectQuantity, 0 }, { Field.EnvironmentCreatedOn, BaseConfiguration.Base } }, LoginType.NothingOrdered),
            new LoginInfo(new Dictionary<string, object> { { Field.ProfileCreated, true }, { Field.EnvironmentCreatedOn, BaseConfiguration.Base } }, LoginType.ProfileCompleted)
        };

        public void SetValuesToBlankDocument()
        {
            Type type = typeof(BsonDocumentValues);
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "EnvironmentCreated")
                {
                    property.SetValue(this, BaseConfiguration.Base);
                }
                else if (property.PropertyType == typeof(string))
                {
                    property.SetValue(this, "");
                }
                else if (property.PropertyType == typeof(int))
                {
                    property.SetValue(this, 0);
                }
                else if (property.PropertyType == typeof(string[]))
                {
                    property.SetValue(this, new string[] { });
                }
                else
                {
                    property.SetValue(this, false);
                }
            }
        }
        public void SetValuesFromEmailAddress() 
        {
            Type type = typeof(BsonDocumentValues);
            PropertyInfo[] properties = type.GetProperties();
            foreach(var property in properties) 
            {
                foreach(var field in Field.DocumentFields) 
                {
                    if (nameof(field) == property.Name && property.Name != "EmailAddress") {
                        property.SetValue(this, ConvertToMemberType(Mongo.GetValueFromDocument(new Dictionary<string, object> { { Field.EmailAddress, EmailAddress } }, field), property));
                    }
                }
            }
        }

        /// <summary>
        /// Ensures that the object returned from the Mongo object is assigned with the correct type
        /// </summary>
        /// <returns>The to member type.</returns>
        /// <param name="value">Value.</param>
        /// <param name="property">Property.</param>
        private dynamic ConvertToMemberType(object value, PropertyInfo property) 
        {
            if (property.PropertyType == typeof(string))
            {
                return value.ToString();
            }
            else if (property.PropertyType == typeof(int))
            {
                return Convert.ToInt32(value);
            }
            else if (property.PropertyType == typeof(string[]))
            {
                return GetArrayValues(value).ToArray();
            }
            else
            {
                return Convert.ToBoolean(value);
            }
        }
        /// <summary>
        /// Helper method if the value of the passed in field is an array to return a list of strings of that array based on whatever the
        /// current email address is.
        /// </summary>
        /// <returns>The array values.</returns>
        /// <param name="object">bsonObject.</param>
        private List<string> GetArrayValues(object bsonObject)
        {
            BsonArray arrayValues = (BsonArray)bsonObject;
            List<string> values = new List<string>();
            foreach (var value in arrayValues.ToArray())
            {
                values.Add(value.ToString());
            }
            return values;
        }
    }
}
