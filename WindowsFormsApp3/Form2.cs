using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;


using static System.Console;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Runtime.InteropServices;
using OpenQA.Selenium.DevTools.V112.Debugger;
using Keys = OpenQA.Selenium.Keys;
using System.Reflection.Emit;

namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {
        private Form1 form1;

        private Store[] stores;
        private int menuItemNumber = -1;
        private int menuCost = 0;
        private int realtotal = 0;

        public Form2(Form1 form1)
        {

            InitializeComponent();
            this.form1 = form1;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string searchText = textBox1.Text;
            Store[] stores = Naver_map.Load_data(searchText);



            // 각 가게에 대한 메뉴와 가격 정보를 관리하기 위한 딕셔너리 생성
            Dictionary<string, List<string>> menuDict = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> costDict = new Dictionary<string, List<string>>();
            foreach (Store store in stores)
            {
                List<string> menus = new List<string>();
                List<string> costs = new List<string>();
                for (int i = 0; i < store.menus.Count; i++)
                {
                    menus.Add(store.menus[i]);
                    costs.Add(store.costs[i].ToString());
                }

                menuDict[store.store_name] = menus;
                costDict[store.store_name] = costs;
            }

            // 리스트뷰 컨트롤 초기화
            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("#", 30);
            listView1.Columns.Add("메뉴", 200);
            listView1.Columns.Add("가격", 100);

            listView2.Items.Clear();
            listView2.Columns.Clear();
            listView2.Columns.Add("#", 30);
            listView2.Columns.Add("메뉴", 200);
            listView2.Columns.Add("가격", 100);

            listView3.Items.Clear();
            listView3.Columns.Clear();
            listView3.Columns.Add("#", 30);
            listView3.Columns.Add("메뉴", 200);
            listView3.Columns.Add("가격", 100);

            // 각 가게에 대한 리스트뷰와 라벨에 메뉴와 가격 정보를 추가
            int index1 = 1;
            int index2 = 1;
            int index3 = 1;

            foreach (string name in menuDict.Keys)
            {

                List<string> menus = menuDict[name];
                List<string> costs = costDict[name];

                for (int i = 0; i < menus.Count; i++)
                {
                    string menuName = menus[i];
                    string menuCostText = costs[i];

                    int menuCost;
                    if (int.TryParse(menuCostText, out menuCost))
                    {
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Clear();
                        item.SubItems.Add(menuName);
                        item.SubItems.Add(menuCost.ToString());

                        if (name == stores[0].store_name)
                        {
                            item.Text = "1-" + index1.ToString();
                            listView1.Items.Add(item);
                            index1++;
                            label1.Text = "1 " + stores[0].store_name; // 가게1의 라벨에 가게 이름을 업데이트
                        }
                        else if (name == stores[1].store_name)
                        {
                            item.Text = "2-" + index2.ToString();
                            listView2.Items.Add(item);
                            index2++;
                            label2.Text = "2 " + stores[1].store_name; // 가게2의 라벨에 가게 이름을 업데이트
                        }
                        else if (name == stores[2].store_name)
                        {
                            item.Text = "3-" + index3.ToString();
                            listView3.Items.Add(item);
                            index3++;
                            label3.Text = "3 " + stores[2].store_name; // 가게3의 라벨에 가게 이름을 업데이트
                        }
                    }
                    else
                    {
                        // 가격 정보가 숫자가 아닌 경우, 처리할 내용을 작성합니다.
                    }
                }
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 입력된 메뉴 번호를 확인합니다.
            string menuNumberText = textBox3.Text.Trim();
            if (!menuNumberText.Contains("-"))
            {
                MessageBox.Show("잘못된 입력입니다.");
                return;
            }

            string[] menuNumberParts = menuNumberText.Split('-');
            if (menuNumberParts.Length != 2 || !int.TryParse(menuNumberParts[1], out menuItemNumber))
            {
                MessageBox.Show("잘못된 입력입니다.");
                return;
            }

            string listViewName = null;
            switch (menuNumberParts[0])
            {
                case "1":
                    listViewName = "listView1";
                    break;
                case "2":
                    listViewName = "listView2";
                    break;
                case "3":
                    listViewName = "listView3";
                    break;
                default:
                    MessageBox.Show("잘못된 입력입니다.");
                    return;
            }

            // 선택된 ListView 컨트롤에서 메뉴의 가격을 찾습니다.
            menuCost = 0;
            ListView listView = Controls.Find(listViewName, true).FirstOrDefault() as ListView;
            if (listView != null)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    string[] itemTexts = item.Text.Split('-');
                    if (itemTexts.Length == 2)
                    {
                        int itemNumberInList = int.Parse(itemTexts[1]);
                        if (menuItemNumber == itemNumberInList)
                        {
                            if (int.TryParse(item.SubItems[2].Text, out menuCost))
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (menuItemNumber == -1)
            {
                MessageBox.Show("메뉴 번호가 잘못 입력되었습니다.");
                return;
            }

            // 찾은 메뉴 가격을 출력합니다.
            MessageBox.Show("메뉴 가격: " + menuCost);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int quantity;
            if (!int.TryParse(textBox4.Text, out quantity))
            {
                MessageBox.Show("잘못된 입력입니다.");
                return;
            }

            int totalCost = menuCost * quantity;
            
            MessageBox.Show("이번 주문 금액은 " + totalCost.ToString() + "원입니다!!");
            
            realtotal = realtotal + totalCost;
        
    }

        private void Form2_Load(object sender, EventArgs e)
        {
            label4.Text = form1.label7.Text;
            label5.Text = form1.label8.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string searchText = label4.Text;
            Store[] stores = Naver_map.Load_data(searchText);

            // 각 가게에 대한 메뉴와 가격 정보를 관리하기 위한 딕셔너리 생성
            Dictionary<string, List<string>> menuDict = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> costDict = new Dictionary<string, List<string>>();
            foreach (Store store in stores)
            {
                List<string> menus = new List<string>();
                List<string> costs = new List<string>();
                for (int i = 0; i < store.menus.Count; i++)
                {
                    menus.Add(store.menus[i]);
                    costs.Add(store.costs[i].ToString());
                }

                menuDict[store.store_name] = menus;
                costDict[store.store_name] = costs;
            }

            // 리스트뷰 컨트롤 초기화
            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("#", 30);
            listView1.Columns.Add("메뉴", 200);
            listView1.Columns.Add("가격", 100);

            listView2.Items.Clear();
            listView2.Columns.Clear();
            listView2.Columns.Add("#", 30);
            listView2.Columns.Add("메뉴", 200);
            listView2.Columns.Add("가격", 100);

            listView3.Items.Clear();
            listView3.Columns.Clear();
            listView3.Columns.Add("#", 30);
            listView3.Columns.Add("메뉴", 200);
            listView3.Columns.Add("가격", 100);

            // 각 가게에 대한 리스트뷰와 라벨에 메뉴와 가격 정보를 추가
            int index1 = 1;
            int index2 = 1;
            int index3 = 1;
            foreach (string name in menuDict.Keys)
            {
                List<string> menus = menuDict[name];
                List<string> costs = costDict[name];

                for (int i = 0; i < menus.Count; i++)
                {
                    string menuName = menus[i];
                    string menuCostText = costs[i];

                    int menuCost;
                    if (int.TryParse(menuCostText, out menuCost))
                    {
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Clear();
                        item.SubItems.Add(menuName);
                        item.SubItems.Add(menuCost.ToString());

                        if (name == stores[0].store_name)
                        {
                            item.Text = "1-" + index1.ToString();
                            listView1.Items.Add(item);
                            index1++;
                            label1.Text = "1 " + stores[0].store_name; // 가게1의 라벨에 가게 이름을 업데이트
                        }
                        else if (name == stores[1].store_name)
                        {
                            item.Text = "2-" + index2.ToString();
                            listView2.Items.Add(item);
                            index2++;
                            label2.Text = "2 " + stores[1].store_name; // 가게2의 라벨에 가게 이름을 업데이트
                        }
                        else if (name == stores[2].store_name)
                        {
                            item.Text = "3-" + index3.ToString();
                            listView3.Items.Add(item);
                            index3++;
                            label3.Text = "3 " + stores[2].store_name; // 가게3의 라벨에 가게 이름을 업데이트
                        }
                    }
                    else
                    {
                        // 가격 정보가 숫자가 아닌 경우, 처리할 내용을 작성합니다.
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string searchText = label5.Text;
            Store[] stores = Naver_map.Load_data(searchText);

            // 각 가게에 대한 메뉴와 가격 정보를 관리하기 위한 딕셔너리 생성
            Dictionary<string, List<string>> menuDict = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> costDict = new Dictionary<string, List<string>>();
            foreach (Store store in stores)
            {
                List<string> menus = new List<string>();
                List<string> costs = new List<string>();
                for (int i = 0; i < store.menus.Count; i++)
                {
                    menus.Add(store.menus[i]);
                    costs.Add(store.costs[i].ToString());
                }

                menuDict[store.store_name] = menus;
                costDict[store.store_name] = costs;
            }

            // 리스트뷰 컨트롤 초기화
            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("#", 30);
            listView1.Columns.Add("메뉴", 200);
            listView1.Columns.Add("가격", 100);

            listView2.Items.Clear();
            listView2.Columns.Clear();
            listView2.Columns.Add("#", 30);
            listView2.Columns.Add("메뉴", 200);
            listView2.Columns.Add("가격", 100);

            listView3.Items.Clear();
            listView3.Columns.Clear();
            listView3.Columns.Add("#", 30);
            listView3.Columns.Add("메뉴", 200);
            listView3.Columns.Add("가격", 100);

            // 각 가게에 대한 리스트뷰와 라벨에 메뉴와 가격 정보를 추가
            int index1 = 1;
            int index2 = 1;
            int index3 = 1;
            foreach (string name in menuDict.Keys)
            {
                List<string> menus = menuDict[name];
                List<string> costs = costDict[name];

                for (int i = 0; i < menus.Count; i++)
                {
                    string menuName = menus[i];
                    string menuCostText = costs[i];

                    int menuCost;
                    if (int.TryParse(menuCostText, out menuCost))
                    {
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Clear();
                        item.SubItems.Add(menuName);
                        item.SubItems.Add(menuCost.ToString());

                        if (name == stores[0].store_name)
                        {
                            item.Text = "1-" + index1.ToString();
                            listView1.Items.Add(item);
                            index1++;
                            label1.Text = "1 " + stores[0].store_name; // 가게1의 라벨에 가게 이름을 업데이트
                        }
                        else if (name == stores[1].store_name)
                        {
                            item.Text = "2-" + index2.ToString();
                            listView2.Items.Add(item);
                            index2++;
                            label2.Text = "2 " + stores[1].store_name; // 가게2의 라벨에 가게 이름을 업데이트
                        }
                        else if (name == stores[2].store_name)
                        {
                            item.Text = "3-" + index3.ToString();
                            listView3.Items.Add(item);
                            index3++;
                            label3.Text = "3 " + stores[2].store_name; // 가게3의 라벨에 가게 이름을 업데이트
                        }
                    }
                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 주문이 종료되면 최종 합계값을 출력합니다.
            label7.Text = realtotal.ToString() + "원 입니다!";
            realtotal = 0;
            // Close();

        }
   
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }

    class Store
    {
        // 변수 선언
        public string store_name;

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
            Thread.Sleep(4000);

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

            return totalPrice;
        }
    }
}

