using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace seleniumtest_medvedev
{ 
    public class SeleniumTestForPractice
    {
        public IWebDriver driver;
        
        
        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArguments("--no-sandbox", "--disable-extensions", "--window-size=1280,1024",  "--headless");
            
            // добавляем js
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            
            // открыть браузер
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            
            // Авторизация
            Authorize();

        }
        
        public void Authorize()
        {
            // ввести url
            driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
            
            // ввести логин и пароль
            var login = driver.FindElement(By.Id("Username"));
            login.SendKeys("megbegb88@gmail.com");
            var password = driver.FindElement(By.Name("Password"));
            password.SendKeys("QQQQqqqq2222@");

            // нажать кнопку "войти"
            var enter = driver.FindElement(By.Name("button"));
            enter.Click();
        }

        public void settingsModalWindow()
        {
            // Открыть менюшку профиля
            driver.FindElement(By.CssSelector("[data-tid='Avatar']")).Click();

            // Войти в настройки
            driver.FindElement(By.CssSelector("[data-tid='Settings']")).Click(); 
        }
       
        [Test]
        
        public void Authorization()
        {
            // проверить что находимся на нужной странице
            var news = driver.FindElement(By.CssSelector("[data-tid='Title']"));
            var currentUrl = driver.Url;
            
            // assert
            currentUrl.Should().Be("https://staff-testing.testkontur.ru/news");

        }

        [Test]
        public void goToCommunitiesFromSideMenu()
        {
            // открыть сайдменю
            var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
            sideMenu.Click();
            
            // открыть сообщества
            var sideMenuCommunity = driver.FindElements(By.CssSelector("[data-tid='Community']"))[1];
            sideMenuCommunity.Click();
            
            // Проверка на открытие страницы Сообщества
            var pageCommunity = driver.Url;
            pageCommunity.Should().Be("https://staff-testing.testkontur.ru/communities");
        }

        [Test]
        public void hotkeyCtrlEnter()
        {
            settingsModalWindow();

            // Открываем выпадающее меню отправки сообщений
            driver.FindElement(By.CssSelector("[data-tid='Hotkey']")).Click();

            // Выбираем Ctrl+Enter
            driver.FindElement(By.CssSelector("[data-tid='HotkeyCtrlEnter']")).Click();

            // Нажать сохранить
            driver.FindElement(By.CssSelector("span[style='display: inline-block; vertical-align: baseline;']")).Click();
            
            // Проверить сохранение изменения в localStorage
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var checkHotKey = js.ExecuteScript("return localStorage.getItem('chatSendHotkey')");
            
            checkHotKey.Should().Be("Ctrl+Enter");
        }

        [Test]
        public void savePhoneNumberProfile()
        {
            // Открыть менюшку профиля
            driver.FindElement(By.CssSelector("[data-tid='Avatar']")).Click();
            
            // Зайти в редактирование профиля
            driver.FindElement(By.CssSelector("[data-tid='ProfileEdit']")).Click();
            
            // Выбиарем поле ввода номера мобилы и вводим тестовый номер
            driver.FindElement(By.CssSelector("[data-tid='Input']")).Click();
            string testNumber = "1234567890";
            driver.FindElement(By.CssSelector("[placeholder='+7 ___ ___-__-__']")).SendKeys(testNumber);

            // scrollUp
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scroll(0, 0)");
            
            // Сохраняем изменения 
            driver.FindElement(By.XPath("//*[contains(text(),'Сохранить')]")).Click();
            
            // Проверяем сохранение на страница профиля
            var checkPhoneNumber = driver.FindElement(By.CssSelector("[data-tid='ContactCard'] [href*='tel:'] [div]"));
            checkPhoneNumber.Should().Be("+7 123 456-78-90");
            
           // тут у меня все посыпалось, но я разберусь! Учту замечания по коду в своих начинаниях и к дедлайну причешу как нужно! 

        }

        [TearDown]
        public void Teardown()
        {
            // закрыть браузер и убить процессы драйвер
            driver.Close();
            driver.Quit();
        }
    }
    
}