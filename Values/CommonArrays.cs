using System;
using System.Collections.Generic;
using PronetAutomation.Mongo;

namespace PronetAutomation.Values
{
    public static class CommonArrays
    {
        public static string[] NewUser
        {
            get
            {
                return new string[]
                {
                AccountPageValues.TestAccountFirstName,
                AccountPageValues.TestAccountLastName,
                AccountPageValues.TestAccountPassword
                };
            }
        }
        public static string[] NewAddress
        {
            get
            {
                return new string[]
                {
                AccountPageValues.TestAccountFirstName,
                AccountPageValues.AddressLine1,
                AccountPageValues.AddressLine2,
                AccountPageValues.City,
                AccountPageValues.State,
                AccountPageValues.Zip
                };
            }
        }
        public static string[] NewCard
        {
            get
            {
                return new string[]
                {
                AccountPageValues.TestAccountFirstName,
                AccountPageValues.CardNumber,
                AccountPageValues.Expiry,
                AccountPageValues.Cvc,
                };
            }
        }
    }
}