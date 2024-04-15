using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Seleniumtest_CL_medvedev;

public class SeleniumTestForPractice

{
    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        // открыть браузер
        var driver = new ChromeDriver(options);
        
        // ввести url
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        Thread.Sleep(5000);
        
        
        // ввести логин и пароль
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("megbegb88@gmail.com");

        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("QQQQqqqq2222@");
        
        Thread.Sleep(1000);
        
        // нажать кнопку "войти"
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        Thread.Sleep(3000);
        
        // проверить что находимся на нужной странице
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news");
        
        // закрыть браузер и убить процессы драйвер
        driver.Quit();
    }
}