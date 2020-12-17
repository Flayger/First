using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;
using System.IO;

namespace First
{
    class FirstTestCase
    {
        //создание драйвера для запуска браузера, элемента страницы, логин-пароль, изображение при ошибке, ожидание
        IWebDriver driver;
        IWebElement ele;
        string login;
        string pass;
        Screenshot image;
        WebDriverWait wait;

        public void BeforeTest(string[] args)
        {
            //через параметры задается логин и пароль, без параметров запускается стандартный
            if(args!=null)
            {
                login = null;
                pass = null;
            }
            login = "arthura635@yahoo.com";
            pass = "21SDF4535146";

            for (int i = 0; i < args.Length; i++)
            {
                string sKeyResult;
                string[] keys = new string[] { "-login=", "-pass=" };
                sKeyResult = null;
                sKeyResult = keys.FirstOrDefault<string>(s => args[i].Contains(s));

                switch (sKeyResult)
                {
                    case "-login=":
                        login = args[i].Substring(args[i].IndexOf("-login=") + 7);

                        break;
                    case "-pass=":
                        pass = args[i].Substring(args[i].IndexOf("-pass=") + 6);
                        break;
                }
            }
            //Console.WriteLine(login);
           // Console.WriteLine(pass);
            //вход в профиль
            //Console.ReadLine();
            this.openReg(login, pass);
        }

        public void openReg(string login, string pass)
        {
            //открыть yahoo
            driver.Url = "https://mail.yahoo.com/";

            //нажать sign in
            ele = driver.FindElement(By.PartialLinkText("Sign in"));
            ele.Click();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //ввод логина
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='login-username']")));
            ele = driver.FindElement(By.XPath("//*[@id='login-username']"));
            ele.SendKeys(login);

            //далее
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='login-signin']")));
            ele = driver.FindElement(By.XPath("//*[@id='login-signin']"));
            ele.Click();

            //ввод пароля
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='login-passwd']")));
            ele = driver.FindElement(By.XPath("//*[@id='login-passwd']"));
            ele.SendKeys(pass);

            //далее
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='login-signin']")));
            ele = driver.FindElement(By.XPath("//*[@id='login-signin']"));
            ele.Click();

        }

        public void Testing()
        {
                SendLetter();
        }


        public void SendLetter()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
            //нажимаем написать, чтобы написать письмо
            wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText("Написать")));
            ele = driver.FindElement(By.PartialLinkText("Написать"));
            ele.Click();


            string a = "https://mail.yahoo.com/d/compose/";
            //ожидание, пока URL страницы поменяется, потому что Yahoo сбрасывает все поля, если дает ID письму
            wait.Until((d) =>
            {
                if (d.Url != a)
                { return true; }
                return false;
            });

            //адрес письма
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='message-to-field']")));
            ele = driver.FindElement(By.XPath("//*[@id='message-to-field']"));
            ele.SendKeys("flayger@yandex.ru");

            //название письма
            ele = driver.FindElement(By.XPath("//*[@id='mail-app-component']/div/div/div[1]/div[3]/div/div/input"));
            ele.SendKeys("AVTOTEST");

            //вставка текста и ссылки на репозиторий
            ele = driver.FindElement(By.XPath("//*[@id='editor-container']/div[1]"));
            string text = "Привет, Вова! Надеюсь, у тебя будет свободное время и желание проверить мою работу, Спасибо!" +
            "запуск через командную строку - переходим в каталог с экзешником, "
            +"@'c:/Users/Flayger/Documents/Visual Studio 2015/Projects/First/First/bin/Debug/First.exe' -login=arthura635@yahoo.com -pass=21SDF4535146";
            ele.SendKeys(text);

            text = "Тест(от англ.test «испытание, проверка») или испытание" +
                "— способ изучения глубинных процессов деятельности системы, посредством" +
                "помещения системы в разные ситуации и отслеживание доступных наблюдению изменений в ней.";
            ele.SendKeys(text);
            ele.SendKeys(OpenQA.Selenium.Keys.Enter);
            ele.SendKeys("https://github.com/Flayger/First");

            ele.SendKeys(OpenQA.Selenium.Keys.Enter);

            //гиперссылка
            ele = driver.FindElement(By.XPath("//*[@title='Ссылка']"));
            ele.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@placeholder='Отобразить текст']")));
            ele = driver.FindElement(By.XPath("//*[@placeholder='Отобразить текст']"));
            ele.SendKeys("hyper");
            ele = driver.FindElement(By.XPath("//*[@value='http://']"));
            ele.SendKeys("mail.yahoo.com/");
            ele.SendKeys(OpenQA.Selenium.Keys.Enter);
            
            //прикрепить файл
            ele = driver.FindElement(By.XPath("//input[@title='Прикрепить файлы']"));

            ele.SendKeys(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + @"\Resources\text.docx");
                //Environment.CurrentDirectory + @"\Resources\text.docx");

            //вставить картинку через ctrl+v
            Clipboard.SetImage(System.Drawing.Image.FromFile(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + @"\Resources\logo.png"));
            
            ele = driver.FindElement(By.XPath("//*[@id='editor-container']/div[1]"));
            ele.SendKeys(OpenQA.Selenium.Keys.Enter);
            ele.SendKeys(OpenQA.Selenium.Keys.Control + "v");

            //подпись
            ele.SendKeys(OpenQA.Selenium.Keys.Enter);
            ele.SendKeys("от меня, Фёдорова Артёма");

            //System.Threading.Thread.Sleep(5);
            //подождать загрузки файла
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@title='text.docx']")));
            //отправить
            ele = driver.FindElement(By.CssSelector("#mail-app-component > div.p_a.R_0.T_0.L_0.B_0.D_F > div > div.em_N.D_F.ek_BB.p_R.o_h > div:nth-child(2) > div > button"));
            ele.Click();

        }





        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            foreach (string s in args)
                Console.WriteLine(s);
            FirstTestCase test = new FirstTestCase();
            try
            {   //@"../../resources/"
                string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
                _filePath += @"\resources\";
                test.driver = new ChromeDriver(_filePath);
                test.driver.Manage().Window.Maximize();
                test.BeforeTest(args);
                test.Testing();
                
            }
            catch (Exception e)
            {
                if (e.Source != null)
                    Console.WriteLine(e);
                
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                test.image = ((ITakesScreenshot)test.driver).GetScreenshot();
                string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
                _filePath = Directory.GetParent(_filePath).FullName;
                _filePath = Directory.GetParent(_filePath).FullName;
                test.image.SaveAsFile(_filePath + @"\temp\screenshot_" + timestamp + ".png");

                String PageSource = test.driver.PageSource;
                System.IO.File.WriteAllText(_filePath + @"\temp\html\" + timestamp + "filename.html", PageSource);

            }
            finally
            {
                test.driver.Close();
                test.driver.Quit();
            }


        }
    }
}