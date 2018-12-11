using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Common.Protractor;
using NLog;
using Common;
using Common.Helpers;
using Common.Extensions;
using System.Collections.Generic;
using Common.Driver;
using PronetAutomation.Values;
using PronetAutomation.Mongo;
using System;

namespace PronetAutomation.Pages
{
    public class CreateAccountPage
    {
        public CreateAccountPage(BasePage basePage, bool shouldNavigate = false)
        {
            Base = basePage;
            //PageFactoryNg.InitNgWebElements(Base.NgDriver, this);
            NewUserPageElements = new List<NgWebElement>
            {
                FirstName,
                LastName,
                NewPassword
            };
            Base.NgDriver.IgnoreSynchronization = true;
            if (shouldNavigate)
            {
                Base.NavigateTo(BaseConfiguration.GetUrlValue("register"));
            }
            Logger.Info("Waiting for page to open");
            Base.IsPageTitle(AssertValues.RegisterTitle);
            Logger.Info("Page successfully opened");
        }
        private BasePage Base { get; set; }
        private IList<NgWebElement> NewUserPageElements;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private NgWebElement FirstName {
            get
            {
                return Base.GetElement(By.Name("FirstName"));
            }
        }

        private NgWebElement LastName {
            get
            {
                return Base.GetElement(By.Name("LastName"));
            }
        }

        private NgWebElement NewEmail {
            get
            {
                return Base.GetElement(By.Name("Email"));
            }
        }

        private NgWebElement NewPassword {
            get
            {
                return Base.GetElement(By.Name("Password"));
            }
        }

        private NgWebElement EmailPrompt {
            get
            {
                return Base.GetElement(By.Id("om-qztif6aa2kjrdj5heg5d"));
            }
        }

        private NgWebElement NewAccountHeader
        {
            get
            {
                return Base.GetElement(By.Id("account-email"));
            }
        }


        public string NewEmailAddress { get; set; }

        public void EnterSignupInformation()
        {
            RemoveEmailPrompt();
            Logger.Info("Entering new user information");
            for (int i = 0; i < NewUserPageElements.Count; i++)
            {
                NewUserPageElements[i].SendKeys(CommonArrays.NewUser[i]);
            }
            Base.Value.EmailAddress = NameHelper.RandomName(8) + "AutomatedTest@test.co";
            Base.Value.EnvironmentCreated = BaseConfiguration.Base;
            NewEmail.SendKeys(Base.Value.EmailAddress);
            NewEmail.Submit();

            Base.NgDriver.IgnoreSynchronization = false;
            //Adding new user email to mongo DB
            Base.Mongo.Insert.NewDocument(Field.NewDocument(Base.Value));
            Base.MongoValuesSet = true;
            Base.IsNewUser = true;
            Logger.Info("Successfully entered new user information");
        }
        public bool VerifyAccountCreated()
        {
            return NewAccountHeader.Text == Base.Value.EmailAddress; 
        }
        private void RemoveEmailPrompt()
        {
            int retryCount = 0;
            while (!Base.CheckIfElementExists(By.Id("om-qztif6aa2kjrdj5heg5d")))
            {
                if (Base.RetryTimeout(retryCount, 4))
                {
                    retryCount++;
                }
                else
                {
                    break;
                }
            }
            try
            {
                Base.ExecuteCustomJavaScript("return document.getElementById('om-qztif6aa2kjrdj5heg5d').remove();");
            }
            catch
            {
                Logger.Info("Annoying Email prompt was not removed, either because it did not exist or the javascript failed to execute.");
            }
        }

    }
}