using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;
using System.IO;

namespace First
{//"c:\Users\Flayger\Documents\Visual Studio 2015\Projects\First\First\bin\Debug\First.exe" -login=arthura635@yahoo.com -pass=21SDF4535146
    class FirstTestCase
    {
        //создание драйвера для запуска браузера, элемента страницы, логин-пароль, изображение при ошибке, ожидание
        IWebDriver driver;
        IWebElement ele;
        string login;
        string pass;
        Screenshot image;
        WebDriverWait wait;
        bool error;

        public void BeforeTest(string[] args)
        {
            //через параметры задается логин и пароль, без параметров запускается стандартный
            if((args != null && args.Length != 0))
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
            Console.WriteLine("логин пароль определен");
            //вход в профиль
            this.openReg(login, pass);
        }

        public void openReg(string login, string pass)
        {
            //открыть yahoo
            driver.Url = "https://mail.yahoo.com/";
            Console.WriteLine("переход на ссылку");
            
            

            //нажать sign in
            ele = driver.FindElement(By.PartialLinkText("Sign in"));
            ele.Click();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            Console.WriteLine("войти нажато");

            //ввод логина
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='login-username']")));
            ele = driver.FindElement(By.XPath("//*[@id='login-username']"));
            ele.SendKeys(login);
            Console.WriteLine("логин введен");

            //далее
            
            ele = driver.FindElement(By.XPath("//*[@id='login-signin']"));
            ele.Click();
            Console.WriteLine("логин, далее");

            //ввод пароля
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='login-passwd']")));
            ele = driver.FindElement(By.XPath("//*[@id='login-passwd']"));
            ele.SendKeys(pass);
            Console.WriteLine("пароль введен");

            string a = driver.Url;
            //далее
            
            ele = driver.FindElement(By.XPath("//*[@id='login-signin']"));
            ele.Click();
            
            
            error = wait.Until((d) =>
            {
                if (d.Url != a)
                { Console.WriteLine("Проверка: вход выполнен"); return true; }
                return false;
            });
            if (error == false)
            {
                throw new Exception("ОШИБКА вход не выполнен");
            }


        }

        public void Testing()
        {
                SendLetter();
        }


        public void SendLetter()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            //нажимаем написать, чтобы написать письмо
            wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText("Написать")));
            ele = driver.FindElement(By.PartialLinkText("Написать"));
            ele.Click();
            Console.WriteLine("письмо начато");

            
            string a = "https://mail.yahoo.com/d/compose/";
            //ожидание, пока URL страницы поменяется, потому что Yahoo сбрасывает все поля, если дает ID письму
            error = wait.Until((d) =>
            {
                if (d.Url != a)
                { Console.WriteLine("Проверка: id письма присвоен"); return true; }
                return false;
            });
            if(error==false)
            {
                throw new Exception("ID не происвоен");
            }

            //адрес письма
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='message-to-field']")));
            ele = driver.FindElement(By.XPath("//*[@id='message-to-field']"));
            ele.SendKeys("flayger@yandex.ru");
            Console.WriteLine("адрес письма добавлен");

            //название письма
            ele = driver.FindElement(By.XPath("//input[@data-test-id='compose-subject']"));
                //"//*[@id='mail-app-component']/div/div/div[1]/div[3]/div/div/input"));
            ele.SendKeys("AVTOTEST");
            Console.WriteLine("название письма добавлено");

            //вставка текста и ссылки на репозиторий
            ele = driver.FindElement(By.XPath("//*[@role='textbox']"));
            string text;

            text = "Тест(от англ.test «испытание, проверка») или испытание" +
                "— способ изучения глубинных процессов деятельности системы, посредством" +
                "помещения системы в разные ситуации и отслеживание доступных наблюдению изменений в ней.";
            ele.SendKeys(text);
            ele.SendKeys(OpenQA.Selenium.Keys.Enter);
            ele.SendKeys("https://github.com/Flayger/First");

            ele.SendKeys(OpenQA.Selenium.Keys.Enter);
            Console.WriteLine("текст добавлен");

            //гиперссылка
            ele = driver.FindElement(By.XPath("//*[@title='Ссылка']"));
            ele.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@placeholder='Отобразить текст']")));
            ele = driver.FindElement(By.XPath("//*[@placeholder='Отобразить текст']"));
            ele.SendKeys("hyper");
            ele = driver.FindElement(By.XPath("//*[@value='http://']"));
            ele.SendKeys("mail.yahoo.com/");
            ele.SendKeys(OpenQA.Selenium.Keys.Enter);
            Console.WriteLine("гиперссылка добавлена");

            //прикрепить файл
            ele = driver.FindElement(By.XPath("//input[@title='Прикрепить файлы']"));

            ele.SendKeys(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + @"\Resources\text.docx");
            //Environment.CurrentDirectory + @"\Resources\text.docx");
            Console.WriteLine("файл добавлен");
            //вставить картинку через ctrl+v
            Clipboard.SetImage(System.Drawing.Image.FromFile(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + @"\Resources\logo.png"));

            ele = driver.FindElement(By.XPath("//*[@role='textbox']"));
                //aria-label='Тело сообщения']"));
                //"//*[@id='editor-container']/div[1]"));
            ele.SendKeys(OpenQA.Selenium.Keys.Enter);
            ele.SendKeys(OpenQA.Selenium.Keys.Control + "v");
            Console.WriteLine("картинка добавлена");

            //подпись
            ele.SendKeys(OpenQA.Selenium.Keys.Enter);
            ele.SendKeys("от меня, Фёдорова Артёма");
            Console.WriteLine("подпись добавлена");

            a = driver.Url;

            //подождать загрузки файла
            if(wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@title='13215text.docx']")))!=null)
            {
                Console.WriteLine("Проверка: файл загружен");
            }
            else
            {
                throw new Exception("ОШИБКА файл не загружен");
            }
            
            //отправить
            ele = driver.FindElement(By.XPath("//*[@title='Отправить это сообщение']"));
            //By.CssSelector("#mail-app-component > div.p_a.R_0.T_0.L_0.B_0.D_F > div > div.em_N.D_F.ek_BB.p_R.o_h > div:nth-child(2) > div > button"));
            ele.Click();
            
            //ожидание, пока URL страницы поменяется, потому что Yahoo сбрасывает все поля, если дает ID письму
            error = wait.Until((d) =>
            {
                if (d.Url != a)
                { Console.WriteLine("Проверка: отправлено"); return true; }
                //Console.WriteLine("Проверка: ошибка отправления");
                return false;
            });
            if (error == false)
            {
                throw new Exception("ОШИБКА письмо не отправлено");
            }
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

                Console.ReadLine();
            }
            finally
            {
                test.driver.Close();
                test.driver.Quit();
                Console.ReadLine();
            }


        }
    }
}