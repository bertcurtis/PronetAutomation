using PronetAutomation.Mongo;

namespace PronetAutomation.Values
{
    public class SocialCredentials
    {
        public SocialCredentials(BasePage basePage)
        {
            TestInstagramUsername = basePage.Mongo.GetSocialCredentials("TestInstagramUsername");
            TestInstagramPassword = basePage.Mongo.GetSocialCredentials("TestInstagramPassword");

            TestGoogleUsername = basePage.Mongo.GetSocialCredentials("TestGoogleUsername");
            TestGooglePassword = basePage.Mongo.GetSocialCredentials("TestGooglePassword");

            TestGoogleUsername2 = basePage.Mongo.GetSocialCredentials("TestGoogleUsername2");
            TestGooglePassword2 = basePage.Mongo.GetSocialCredentials("TestGooglePassword2");

            TestFacebookUsername = basePage.Mongo.GetSocialCredentials("TestFacebookUsername");
            TestFacebookPassword = basePage.Mongo.GetSocialCredentials("TestFacebookPassword");

            TestFacebookUsername2 = basePage.Mongo.GetSocialCredentials("TestFacebookUsername2");
            TestFacebookPassword2 = basePage.Mongo.GetSocialCredentials("TestFacebookPassword2");
        }

        public string TestInstagramUsername { get; set; }
        public string TestInstagramPassword { get; set; }

        public string TestGoogleUsername { get; set; }
        public string TestGooglePassword { get; set; }

        public string TestGoogleUsername2 { get; set; }
        public string TestGooglePassword2 { get; set; }

        public string TestFacebookUsername { get; set; }
        public string TestFacebookPassword { get; set; }

        public string TestFacebookUsername2 { get; set; }
        public string TestFacebookPassword2 { get; set; }

    }
}