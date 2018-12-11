using PronetAutomation.Mongo;
using PronetAutomation.Values;
using Common;
using Common.Driver;
using Common.Extensions;
using MongoDB.Bson;
using NLog;
using Common.Protractor;
using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Net;
using System.IO;
using System.Drawing;

namespace PronetAutomation
{
    /// <summary>
    /// This class should be instantiated as an object as the first step in every test, and then passed in as a parameter for every Page Object created
    /// created after. It is meant to serve as a place to implement custom logic for each individual app outside of the logic provided in 
    /// the Pronet framework. Because it is meant to be used as a base for every page object, it's the place to add properties and methods that 
    /// you want to persist throughout the creation of new page objects.
    /// </summary>
    public class BasePage : ProjectPageBase
    {
        //Under the assumption that the BasePage is always created first for every test, this constructor is used to instantiate universal objects,
        // and execute universal logic (such as navigating to an app url), or even logging in.  
        public BasePage(bool shouldNavigateToBaseUrl = true, LoginType loginType = LoginType.AnyUser)
        {
            LoginType = loginType;
            Value = new BsonDocumentValues(Mongo);
            SetEmailAddress();
            Logger = LogManager.GetCurrentClassLogger();
            Logger.Info("Tests being run on: " + BaseConfiguration.Base);
            SocialCredentials = new SocialCredentials(this);

            if (shouldNavigateToBaseUrl)
            {
                try 
                {
                    NgDriver.Navigate().GoToUrl(BaseConfiguration.GetUrlValue());
                    Thread.Sleep(1000);
                }
                catch
                {
                    JSNavigate();
                    Thread.Sleep(1000);
                }
            }
        }
        public BsonDocumentValues Value { get; set; }
        public LoginType LoginType { get; set; }
        public AccountType AccountType { get; set; }
        public Logger Logger { get; set; }
        public SocialCredentials SocialCredentials { get; set; }
        public bool MongoValuesSet { get; set; }
        public bool IsNewUser { get; internal set; }


        public NgWebElement Spinner
        {
            get 
            {
                return GetElement(By.ClassName("spinner"));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the computer executing the code is linux.
        /// </summary>
        /// <value><c>true</c> if is linux; otherwise, <c>false</c>.</value>
        public bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        /// <summary>
        /// Execute javascript in the browser to navigate to the passed in URL if one is provided otherwise navigate to the URL in the app.config
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="urlOverride">If set to <c>true</c> URL override.</param>
        public void JSNavigate(string url = "", bool urlOverride = false)
        {
            Logger.Info("Using custom javascript to navigate");
            string expectedPageTitle = "";
            if (url == "" && !urlOverride)
            {
                url = BaseConfiguration.GetUrlValue("app");
                if (BaseConfiguration.Base == WebEnvironment.Dev)
                {
                    expectedPageTitle = ExpectedPageTitles.DevApp;
                }
            }
            else if (url == "" && urlOverride) 
            {
                throw new Exception("You must secify a URL as a parameter if you're going to override the base URL");
            }
            else if (url != "" && !urlOverride)
            {
                string page = url;
                url = BaseConfiguration.GetUrlValue(page);
            }
            ExecuteCustomJavaScript(string.Format("window.location.href = '{0}';", url));
            if (expectedPageTitle != "")
            {
                WaitForPageTitleToBe(expectedPageTitle);
            }
            Logger.Info("Successfully navigated to: " + url);
        }

        public void WaitForPageTitleToBe(string title) 
        {
            int retryCount = 0;
            while (!IsPageTitle(title))
            {
                if (RetryTimeout(retryCount, 20))
                {
                    retryCount++;
                }
                else
                {
                    break;
                }
            }
        }

        public void WaitForSpinnerToGoAway()
        {
            try
            {
                Spinner.WaitUntilElementIsInvisible();
            }
            catch
            {
                Logger.Info("No need to wait for spinner, spinner is not visible");
            }
        }

        /// <summary>
        /// Sets the properties on the BsonDocumentValues object based on the email address.
        /// </summary>
        public void SetMongoValuesBasedOnEmailAddress()
        {
            SetEmailAddress();
            Value.SetValuesFromEmailAddress();
        }

        /// <summary>
        /// Verifies the images are same.
        /// </summary>
        /// <param name="url1">Url1.</param>
        /// <param name="url2">Url2.</param>
        public void VerifyImagesAreSame(string url1, string url2)
        {
            WebRequest originalImage = WebRequest.Create(url1);
            WebRequest currentImage = WebRequest.Create(url2);

            WebResponse original = originalImage.GetResponse();
            WebResponse current = currentImage.GetResponse();

            Stream originalStream = original.GetResponseStream();
            Stream currentStream = current.GetResponseStream();

            Bitmap originalBM = new Bitmap(originalStream);
            Bitmap currentBM = new Bitmap(currentStream);

            Assert.IsTrue(originalBM.Size.Equals(currentBM.Size));

            for (int x = 0; x < originalBM.Width; x++)
            {
                for (int y = 0; y < originalBM.Height; y++)
                {
                    if (originalBM.GetPixel(x, y) != currentBM.GetPixel(x, y))
                    {
                        Assert.IsTrue(originalBM.GetPixel(x, y) == currentBM.GetPixel(x, y));
                    }
                }
            }
        }
        /// <summary>
        /// Verifies the images are same.
        /// </summary>
        /// <param name="bmp1">Bmp1.</param>
        /// <param name="bmp2">Bmp2.</param>
        public void VerifyImagesAreSame(Bitmap bmp1, Bitmap bmp2)
        {
            Assert.IsTrue(bmp1.Size.Equals(bmp2.Size));

            for (int x = 0; x < bmp1.Width; x++)
            {
                for (int y = 0; y < bmp1.Height; y++)
                {
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                    {
                        Assert.IsTrue(bmp1.GetPixel(x, y) == bmp2.GetPixel(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Iterates through the LoginInfos list and finds the matching LoginType for the test and sets the Value.EmailAddress based off of the 
        /// dictionary fields that are associated with that LoginType. If none are found, then it defaults to the default email found in the app.config
        /// </summary>
        private void SetEmailAddress()
        {
            while (String.IsNullOrEmpty(Value.EmailAddress))
            {
                foreach (var info in Value.LoginInfos)
                {
                    if (info.LoginType == LoginType)
                    {
                        Value.EmailAddress = Mongo.GetUserEmail(info.DictionaryFields, false);
                    }
                }
                Logger.Info("Could not get EmailAddress from DB, defaulting to config email");
                Value.EmailAddress = BaseConfiguration.DefaultEmailAddress;
            }
            Logger.Info("Email address being used is: " + Value.EmailAddress);
        }
    }
}