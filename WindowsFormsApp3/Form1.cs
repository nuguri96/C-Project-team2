using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public class Recommand
        {
            public string question;
            public string answer;
            public string answer_p;

            public Recommand(string question, string answer, string answer_p)
            {
                this.question = question;
                this.answer = answer;
                this.answer_p = answer_p;
            }
        }

        private Recommand[] recommand_list;
        private Random random;
        private int q_num;
        public Form1()
        {
            InitializeComponent();

            /*Button myButton = this.button1;  // 버튼 객체를 받아옴
            myButton.Name = "클릭";  // 버튼 객체의 Name 속성을 변경

            this.button3.Name = "YES";
            this.button4.Name = "NO";
            this.button4.Name = "확정";*/
            // 버튼 객체를 받아옴
            // 버튼 객체의 Name 속성을 변경
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.ControlBox = false;
            this.Text = "MENU RECOMMEND";
            this.Location = new Point(0, 100);
            //this.BackColor = Color.Yellow;

            label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            button1.FlatStyle = FlatStyle.Flat;
            button1.BackColor = Color.Green;

            label2.Text = "귀찮으니 랜덤으로 메뉴 \n정하고 싶으면  여기 이용";
            label6.Text = "질문으로 메뉴 정하고 싶으면 여기 이용";
            label9.Text = "확정하고 싶으면 여기";
            // 추천 음식 정보 초기화
            recommand_list = new Recommand[17];
            random = new Random();

            // 추천 음식 정보 할당
            recommand_list[0] = new Recommand("면 요리를 먹고싶으신가요?", "짜장면, 중식, 국수, 쌀국수, 우동, 모밀, 냉면", "");
            recommand_list[1] = new Recommand("다이어트 중 이신가요?", "샐러드, 두부요리, 샌드위치, 요거트, 스프, 포케", "");
            recommand_list[2] = new Recommand("국물요리를 먹고 싶으신가요?", "라면, 쌀국수, 순대국, 국밥, 마라탕, 갈비탕, 찌개, 만두국, 칼국수", "");
            recommand_list[3] = new Recommand("술과함께 먹고 싶으신가요?", "치킨, 족발, 보쌈, 김치찌개, 술집, 고기, 감자탕, 곱창", "");
            recommand_list[4] = new Recommand("기름진 음식을 먹고 싶으신가요?", "양식, 치킨, 피자, 햄버거, 텐동, 돈가스, 중식, 분식, 고기, 닭갈비", "");
            recommand_list[5] = new Recommand("시간이 부족한가요?", "햄버거, 분식, 컵밥, 도시락, 국수, 김밥, 냉면, 편의점, 국밥, 토스트", "");
            recommand_list[6] = new Recommand("매콤한 음식을 먹고 싶으신가요?", "떡볶이, 마라탕, 짬뽕, 라면, 육개장, 닭갈비, 카레, 컵밥, 감자탕", "");
            recommand_list[7] = new Recommand("포장이 필요한 음식을 찾으시나요?", "밥버거, 샐러드, 컵밥, 도시락, 햄버거, 부리또, 떢볶이, 김밥, 초밥, 토스트", "");
            recommand_list[8] = new Recommand("간식을 먹고 싶으신가요?", "분식, 핫도그, 와플, 빵, 편의점, 부리또, 샐러드", "");
            recommand_list[9] = new Recommand("가격이 저렴한 음식을 찾으시나요?", "김밥, 컵밥, 편의점, 기념관 식당, 국수, 핫도그, 샌드위치, 착한초밥, 학식", "");

            recommand_list[10] = new Recommand("한식, 양식, 아시아음식, 일식, 어떤 음식을 먹고 싶으신가요?", "한식, 양식, 아시아음식, 일식", "");
            recommand_list[11] = new Recommand("고기를 먹고 싶으신가요?", "고기구이, 수육, 삼겹살, 소고기, 돼지고기, 불고기, 갈비, 스테이크", "");
            recommand_list[12] = new Recommand("해산물을 먹고 싶으신가요?", "회, 참치, 연어, 굴, 조개구이, 냉면, 아구찜, 해물파전", "");
            recommand_list[13] = new Recommand("야채를 많이 먹고 싶으신가요?", "샐러드, 두부요리, 비빔밥, 불고기, 채소전, 채소스키야끼, 채소튀김", "");
            recommand_list[14] = new Recommand("특별한 날이거나 기념일이 있나요?", "고급 뷔페, 스테이크, 프렌치, 이탈리안, 일식, 중식, 한식", "");
            recommand_list[15] = new Recommand("한식 중에 어떤 종류가 좋으신가요?", "국밥, 찜, 구이, 전, 밥, 면, 탕, 떡볶이, 닭갈비", "");
            recommand_list[16] = new Recommand("음식에 대한 특별한 요구사항이 있나요?", "베지테리언, 글루텐프리, 무조건 매운맛, 적당히 매운맛, 고단백, 저칼로리, 무조건 달달한맛, 무조건 짭짤한맛", "");

            // 문제 번호 초기화
            q_num = 0;
        }

        private string[] meatCategory = { "돼지고기", "소고기", "양고기", "닭고기" };
        private string[] zzigaeCategory = { "된장찌개", "김치찌개" };
        private string[] noodleCategory = { "우동", "모밀", "국수", "냉면" };
        private string[] gukbabCategory = { "돼지국밥", "소머리국밥", "순대국밥", "콩나물국밥" };
        private string[] bunsikCategory = { "떡뽁이", "순대", "오뎅", "튀김" };
        private string[] fastCategory = { "피자", "햄버거", "치킨" };
        private string[] dietCategory = { "샐러드", "닭가슴살", "포케", "두부요리", "요거트" };
        private string GetRandomFood(string[] category)
        {
            Random random = new Random();
            int randomCategoryIndex = random.Next(category.Length);

            string[] foods = null;
            switch (category[randomCategoryIndex])
            {
                case "고기류":
                    foods = meatCategory;
                    break;
                case "찌개류":
                    foods = zzigaeCategory;
                    break;
                case "면류":
                    foods = noodleCategory;
                    break;
                case "국밥류":
                    foods = gukbabCategory;
                    break;
                case "분식류":
                    foods = bunsikCategory;
                    break;
                case "패스트푸드류":
                    foods = fastCategory;
                    break;
                case "다이어트류":
                    foods = dietCategory;
                    break;
            }

            if (foods != null)
            {
                int randomFoodIndex = random.Next(foods.Length);
                return foods[randomFoodIndex];
            }
            else
            {
                return "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] categories = { "고기류", "찌개류", "면류", "국밥류", "분식류", "패스트푸드류", "다이어트류" };
            string randomFood = GetRandomFood(categories);
            label1.Text = randomFood;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button5.PerformClick();
            // 랜덤으로 질문 선택
            int index = random.Next(recommand_list.Length);
            Recommand recommand = recommand_list[index];

            // 질문과 음식 표시
            label3.Text = recommand.question;
            label4.Text = recommand.answer;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // label4에 표시된 음식 중 랜덤으로 선택
            int index = random.Next(recommand_list.Length);
            string[] foods = label4.Text.Split(',');
            string selected_food = foods[random.Next(foods.Length)];

            // 선택된 음식 표시
            label5.Text = selected_food;
            label5.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 랜덤으로 질문 선택
            int index = random.Next(recommand_list.Length);
            Recommand recommand = recommand_list[index];

            // 질문과 음식 표시
            label3.Text = recommand.question;
            label4.Text = recommand.answer;
            label5.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label7.Text = label1.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label8.Text = label5.Text;
        }

        

        private void button8_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            form2.Show();
            

            Form1 form1 = new Form1();
            form1.Hide();

            //label8.Text = form2.label4.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            label1.Text = "";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            label7.Text = "";
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            label8.Text = "";
        }
    }
}
