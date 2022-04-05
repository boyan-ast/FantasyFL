namespace FantasyFL.Web.Tests
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    using Xunit;

    using static FantasyFL.Common.GlobalConstants;

    public class SeleniumTests : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        private readonly SeleniumServerFactory<Startup> server;
        private readonly IWebDriver browser;

        public SeleniumTests(SeleniumServerFactory<Startup> server)
        {
            this.server = server;
            server.CreateClient();
            var opts = new ChromeOptions();
            opts.AddArguments("--headless");
            opts.AcceptInsecureCertificates = true;
            this.browser = new ChromeDriver(opts);
        }

        [Fact]
        public void FooterOfThePageContainsRulesLink()
        {
            this.browser.Navigate().GoToUrl(this.server.RootUri);
            Assert.EndsWith(
                "/Home/Rules",
                this.browser.FindElements(By.CssSelector("footer div a")).First().GetAttribute("href"));
        }

        [Fact]
        public void LoginShouldLogAdmin()
        {
            this.browser.Navigate()
                .GoToUrl(this.server.RootUri + @"/Identity/Account/Login");

            this.browser.FindElement(By.Id("Input_Username"))
                .SendKeys(AdministratorUserName);

            this.browser.FindElement(By.Id("Input_Password"))
                .SendKeys(AdministratorPassword);

            this.browser.FindElement(By.CssSelector("button[type='submit']"))
                .Click();

            var userNavLinkText = this.browser
                .FindElement(By.CssSelector("li.nav-item > a.nav-link[title='Manage']")).Text;

            Assert.Equal("Hello Admin!", userNavLinkText);
        }

        [Fact(Skip = "Require blob connection string")]
        public void ResultsPageShouldReturnCompleteList()
        {
            this.browser.Navigate().GoToUrl(this.server.RootUri + @"/FirstLeague/Results");

            var tableRows = this.browser
                .FindElements(By.CssSelector("table.table > tbody > tr"))
                .Count;

            Assert.Equal(7, tableRows);
        }

        [Fact(Skip = "Require blob connection string")]
        public void FixturesPageShouldReturnCompleteList()
        {
            this.browser.Navigate().GoToUrl(this.server.RootUri);

            this.browser
                .FindElement(By
                    .CssSelector("div.dropdown-menu > a.dropdown-item[href='/FirstLeague/Fixtures']"))
                .Click();

            var tableRows = this.browser
                .FindElements(By.CssSelector("table.table > tbody > tr"))
                .Count;

            Assert.Equal(7, tableRows);
        }

        [Fact]
        public void TeamsPageShouldReturnCompleteList()
        {
            this.browser.Navigate().GoToUrl(this.server.RootUri);

            this.browser
                .FindElement(By
                    .CssSelector("div.dropdown-menu > a.dropdown-item[href='/Teams/All']"))
                .Click();

            var tableRows = this.browser
                .FindElements(By.CssSelector("div.row > div.col-md-3"))
                .Count;

            Assert.Equal(14, tableRows);
        }

        [Fact]
        public void PlayersPageShouldReturnCorrectTeamPlayers()
        {
            this.browser.Navigate().GoToUrl(this.server.RootUri + @"/Teams/Players/3");

            var teamName = this.browser
                .FindElement(By.CssSelector("div.col-md-4 > div.text-center > h5"))
                .Text;

            Assert.Equal("Levski Sofia", teamName);
        }

        [Fact]
        public void AdminShoudSeeImportDataTab()
        {
            this.browser.Navigate()
               .GoToUrl(this.server.RootUri + @"/Identity/Account/Login");

            this.browser.FindElement(By.Id("Input_Username"))
                .SendKeys(AdministratorUserName);

            this.browser.FindElement(By.Id("Input_Password"))
                .SendKeys(AdministratorPassword);

            this.browser.FindElement(By.CssSelector("button[type='submit']"))
                .Click();

            var result = this.browser
                .FindElement(By
                    .CssSelector("ul.navbar-nav > li:nth-child(2) > a"))
                .Text;

            Assert.Equal("Import Data", result);
        }

        [Fact]
        public void LogoutShouldReturnToHomaPage()
        {
            this.browser.Navigate()
               .GoToUrl(this.server.RootUri + @"/Identity/Account/Login");

            this.browser.FindElement(By.Id("Input_Username"))
                .SendKeys(AdministratorUserName);

            this.browser.FindElement(By.Id("Input_Password"))
                .SendKeys(AdministratorPassword);

            this.browser.FindElement(By.CssSelector("button[type='submit']"))
                .Click();

            this.browser.FindElement(By.CssSelector("li.nav-item > form.form-inline > button"))
                .Click();

            var registerNavLinkTitle = this.browser
                .FindElement(By.CssSelector("ul.navbar-nav > li:nth-child(1) > a"))
                .Text;

            Assert.Equal("Register", registerNavLinkTitle);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.server?.Dispose();
                this.browser?.Dispose();
            }
        }
    }
}
