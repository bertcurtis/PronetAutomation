using PronetAutomation.Pages;
using NUnit.Framework;
using Common;
using PronetAutomation.Values;

namespace PronetAutomation.Tests
{
    [TestFixture]
    public class CreateNewUserTests : ProjectTestBase
    {
        [Test, Category("New User Created")]
        public void CreateNewUser()
        {
            //Instantiate Page Object here
            BasePage basePage = new BasePage();
            basePage.Info = LoginInfo.AnyUser;

            CreateAccountPage createAccountPage = new CreateAccountPage(basePage, true);
            createAccountPage.EnterSignupInformation();
            Assert.IsTrue(createAccountPage.VerifyAccountCreated());
        }

    }
}