using System;
using static System.Console;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;

namespace Naver_map
{
    class Store
    {
        // 변수 선언
        string store_name;

        // 메뉴와 가격들을 담을 수 있는 배열
        public List<int> costs;
        public List<string> menus;

        public Store(string store_name)
        {
            this.store_name = store_name;
            this.menus = new List<string>();
            this.costs = new List<int>();
        }

        public void Insert(string crawled_string)
        {
            // 입력받은 문자열을 처리하는 코드 작성
            string[] splited = crawled_string.Split('\n');

            string menu = splited[0];
            int cost = 0;

            foreach (string s in splited)
            {
                if (s.Contains("0") && s.Contains("원"))
                {
                    StringBuilder result = new StringBuilder();

                    foreach (char c in s)
                    {
                        if (char.IsDigit(c))
                        {
                            result.Append(c);
                        }
                    }

                    cost = int.Parse(result.ToString());
                }
            }

            // 문자열을 처리해서 얻은 메뉴와 가격 정보를 배열에 저장하는 코드 작성
            menus.Add(menu);
            costs.Add(cost);
        }
        public void Print_stored_data()
        {
            WriteLine($"가게 이름 : {this.store_name}");
            WriteLine("==============================");
            WriteLine("==============메뉴=============");
            WriteLine("==============================");
            for (int i = 0; i < this.menus.Count; i++)
            {
                WriteLine($"메뉴 : {this.menus[i]}");
                WriteLine($"가격 : {this.costs[i]}");
                WriteLine("");
            }

        }
    }


    class Naver_map
    {
        public static Store[] Load_data(string user_input)
        {
            WriteLine("네이버 지도 크롤링 중...");
            // 반환에 필요한 변수 선언
            Store[] store = new Store[3];


            // Chrome 드라이버를 로드합니다.
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-position=-32000,-32000"); // 브라우저 창을 데스크톱 밖으로 숨김
            IWebDriver driver = new ChromeDriver(options);
            //IWebDriver driver = new ChromeDriver();


            // 네이버 지도 페이지를 엽니다.
            driver.Navigate().GoToUrl("https://map.naver.com/v5/?c=15,0,0,0,dh");

            // 2초(2000밀리초) 동안 실행을 멈춥니다.
            Thread.Sleep(2000);

            // 검색어를 입력합니다.
            IWebElement searchBox = driver.FindElement(By.XPath("/html/body/app/layout/div[3]/div[2]/shrinkable-layout/div/app-base/search-input-box/div/div/div/input"));
            searchBox.SendKeys(user_input);
            searchBox.SendKeys(Keys.Enter);

            // 2초(2000밀리초) 동안 실행을 멈춥니다.
            Thread.Sleep(2000);

            // 프레임을 찾습니다.(돈가스 검색 후)
            IWebElement frameElement = driver.FindElement(By.CssSelector("#searchIframe"));

            // 프레임으로 전환합니다.
            driver.SwitchTo().Frame(frameElement);

            // 프레임 내부에서 요소를 찾아 작업합니다.            

            // 검색 결과에서 매장 이름과 주소를 가져옵니다.
            int idx = 1;
            try
            {
                // 매장은 최대 3개까지만 저장하는 것으로 설정
                while (idx != 4)
                {
                    IWebElement nameElement = driver.FindElement(By.XPath($"/html/body/div[3]/div/div[3]/div[1]/ul/li[{idx}]/div[1]/a[1]/div/div/span[1]"));
                    // store 객체 생성하며 가게 이름 설정
                    store[idx - 1] = new Store(nameElement.Text);
                    nameElement.Click();

                    // 2초(2000밀리초) 동안 실행을 멈춥니다.
                    Thread.Sleep(2000);

                    // 프레임에서 빠져나옵니다.
                    driver.SwitchTo().DefaultContent();

                    // 프레임을 찾습니다.(돈가스 매장 이름 클릭 후)
                    IWebElement frameElement2 = driver.FindElement(By.CssSelector("#entryIframe"));

                    // 프레임으로 전환합니다.
                    driver.SwitchTo().Frame(frameElement2);


                    // 프레임 내부에서 요소를 찾아 작업합니다. 

                    // 메뉴 클릭
                    IWebElement menu = driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div[5]/div/div/div/div/a[2]"));
                    if (menu.Text == "메뉴")
                    {
                        menu.Click();
                    }
                    else
                    {
                        menu = driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div[5]/div/div/div/div/a[3]"));
                        menu.Click();
                    }


                    // 2초(2000밀리초) 동안 실행을 멈춥니다.
                    Thread.Sleep(2000);


                    // 검색 결과에서 메뉴 리스트를 가져옵니다.

                    int cnt = 1;
                    try
                    {
                        while (true)
                        {

                            // 메뉴 이름 요소를 찾습니다.
                            IWebElement nameElement_menu_name = driver.FindElement(By.XPath($"/html/body/div[3]/div/div/div/div[7]//li[{cnt++}]"));

                            // 이름과 가격을 가져옵니다.
                            string name = nameElement_menu_name.Text;
                            store[idx - 1].Insert(name);

                            // 결과를 출력합니다.

                        }
                    }
                    catch (NoSuchElementException)
                    {
                        WriteLine("메뉴검색 완료");
                    }

                    // 2초(2000밀리초) 동안 실행을 멈춥니다.
                    Thread.Sleep(2000);

                    // 프레임에서 빠져나옵니다.
                    driver.SwitchTo().DefaultContent();

                    // 프레임을 찾습니다.(돈가스 검색 후)
                    frameElement = driver.FindElement(By.CssSelector("#searchIframe"));

                    // 프레임으로 전환합니다.
                    driver.SwitchTo().Frame(frameElement);

                    idx++;
                }
            }

            catch (NoSuchElementException)
            {
                WriteLine("가게 이름 검색 완료");
            }

            // 드라이버를 종료합니다.
            driver.Quit();

            return store;
        }
    }

    class Kiosk
    {


        public static int CalculateMenuPrice(string menuName, int menuPrice, int userCount)
        {
            int totalPrice = menuPrice * userCount;
            WriteLine($"{menuName} {userCount}개를 주문하셨습니다.");
            WriteLine($"총 가격은 {totalPrice}원 입니다.");
            return totalPrice;
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            Write("먹고 싶은 음식을 입력해 주세요 : ");
            string user_input = ReadLine();
            Store[] store = Naver_map.Load_data(user_input);

            for (int i = 0; i < store.Length; i++)
            {
                WriteLine($"{i + 1}번째 가게 정보");
                store[i].Print_stored_data();
            }

            Write("주문할 가게 번호를 입력하세요 : ");
            int storeNum = int.Parse(ReadLine()) - 1;

            Dictionary<string, int> order = new Dictionary<string, int>();
            while (true)
            {

                Write("주문할 메뉴 번호를 입력하세요 (주문을 마치려면 0을 입력하세요): ");
                int menuNum = int.Parse(ReadLine()) - 1;
                if (menuNum < 0) break;
                Write("주문할 개수를 입력하세요: ");
                int count = int.Parse(ReadLine());
                string menuName = store[storeNum].menus[menuNum];
                if (order.ContainsKey(menuName))
                {
                    order[menuName] += count;
                }
                else
                {
                    order[menuName] = count;
                }
            }

            int totalCost = 0;
            foreach (KeyValuePair<string, int> item in order)
            {
                string menuName = item.Key;
                int count = item.Value;
                int menuNum = store[storeNum].menus.IndexOf(menuName);
                int menuPrice = store[storeNum].costs[menuNum];
                int cost = Kiosk.CalculateMenuPrice(menuName, menuPrice, count);
                totalCost += cost;
            }

            WriteLine("총 가격: {0}원", totalCost);
        }
    }

}