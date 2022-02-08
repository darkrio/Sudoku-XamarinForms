using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Riots.Sudoku.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SudokuPage : ContentPage
    {
        public string[,] sudoku = new string[9, 9];
        public string[,] history = new string[9, 9];
        public int x = 0;
        public int y = 0;
        private const int blank = 40;
        private Button currentButton;

        public SudokuPage()
        {
            InitializeComponent();
            InitNumberButton();
        }

        public void InitNumberButton() 
        {
            var num = 1;
            //生成数字键盘
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    var btn = new Button();
                    if (num == 10)
                    {
                        btn.Text = "Clear";
                        btn.Clicked += btnClear_Clicked;
                        btn.FontSize = 15;
                    }
                    else
                    {
                        btn.Text = num.ToString();
                        btn.Clicked += btnNumber_Clicked;
                        btn.FontSize = 16;
                    }
                    btn.Padding = 0;
                    NumberGrid.Children.Add(btn, j, i);
                    num++;
                }
            }
            //修饰数独按钮
            foreach (View v in this.SudokuGrid.Children)
            {
                if (v is Button btn)
                {
                    btn.Text = " ";
                    btn.Padding = 0;
                    btn.Margin = 0;
                    btn.FontSize = 20;
                    btn.BorderWidth = 2;
                    btn.Clicked += btnSudokuItem_Clicked;
                    btn.IsEnabled = false;
                }
            }
        }

        private void btnNumber_Clicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (currentButton != null)
            {
                currentButton.Text = b.Text;
                string btnName = currentButton.StyleId;
                currentButton.TextColor = !IsValid(int.Parse(btnName.Split('_')[1]), int.Parse(btnName.Split('_')[2]), int.Parse(b.Text.Trim()))
                    ? Color.Red
                    : Color.White;
            }
        }

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            if (currentButton != null)
                currentButton.Text = " ";
        }
        
        private void InitSudoku() 
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sudoku[i, j] = " ";
                    history[i, j] = " ";
                }
            }
        }

        private string[] GetRandomArray()
        {
            Random r = new Random();
            string[] arr = new string[9] { " ", " ", " ", " ", " ", " ", " ", " ", " ", };
            int count = 1;
            while (count <= 9)
            {
                int num = r.Next(0, 9);
                if (arr[num] == " ")
                {
                    arr[num] = count.ToString();
                    count++;
                }

            }
            return arr;
        }

        private void GetNewSudoku() 
        {
            do
            {
                InitSudoku();
                for (int i = 0; i < 3; i++)
                {
                    string[] temp = GetRandomArray();
                    int p = 0;
                    for (int j = 0 + (i * 3); j < 3 + (i * 3); j++)
                    {
                        for (int k = 0 + (i * 3); k < 3 + (i * 3); k++)
                        {
                            sudoku[j, k] = temp[p];
                            p++;
                        }
                    }
                }
                SolveSudoku();
            } while (sudoku[8, 5] == " ");
            DigHole(blank);
        }

        private void SolveSudoku()
        {
            Find(0, 0);
            void Find(int x, int y)
            {
                for (int i = x; i < 9; i++)
                {
                    int z = i == x ? y : 0;
                    for (int j = z; j < 9; j++)
                    {
                        if (sudoku[i, j].Trim() == "")
                        {
                            string[] row = GetRow(i);
                            string[] col = GetColumn(j);
                            string[] box = GetBox(i, j);
                            int[] result = GetPossibleNumber(row, col, box);
                            switch (result.Length)
                            {
                                case 0:
                                    {
                                        return;
                                    }
                                case 1:
                                    {
                                        sudoku[i, j] = result[0].ToString();
                                        Find(i, j);


                                        if (sudoku[8, 8].Trim() == "")
                                            sudoku[i, j] = " ";
                                        else
                                            return;

                                        return;
                                    }
                                default:
                                    {
                                        for (int k = 0; k < result.Length; k++)
                                        {
                                            sudoku[i, j] = result[k].ToString();
                                            Find(i, j);


                                            if (sudoku[8, 8].Trim() == "")
                                                sudoku[i, j] = " ";
                                            else
                                                return;

                                        }
                                        return;
                                    }
                            }
                        }
                    }
                }
                return;
            }
        }

        private string[] GetRow(int x)
        {
            string[] row = new string[9];
            for (int i = 0; i < 9; i++)
                row[i] = sudoku[x, i];

            return row;
        }

        private string[] GetColumn(int y)
        {
            string[] column = new string[9];
            for (int i = 0; i < 9; i++)
                column[i] = sudoku[i, y];
            return column;
        }

        private string[] GetBox(int x, int y)
        {
            int dx = (int)(x / 3);
            int dy = (int)(y / 3);
            string[] arr = new string[9];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    arr[(i * 3) + j] = sudoku[i + dx * 3, j + dy * 3];

            }
            return arr;
        }

        private static int[] GetPossibleNumber(string[] row, string[] col, string[] box)
        {
            int[] arr = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 9; i++)
            {
                if (row[i].Trim() != "")
                    arr[Int32.Parse(row[i]) - 1]++;

                if (col[i].Trim() != "")
                    arr[Int32.Parse(col[i]) - 1]++;

                if (box[i].Trim() != "")
                    arr[Int32.Parse(box[i]) - 1]++;
            }
            ArrayList res = new ArrayList();
            for (int i = 0; i < 9; i++)
            {
                if (arr[i] == 0)
                    res.Add(i + 1);
            }
            return (int[])res.ToArray(typeof(int));
        }

        private void DigHole(int blank)
        {
            int num = blank;
            do
            {
                Random rx = new Random();
                Random ry = new Random();
                int x = rx.Next(0, 9);
                int y = ry.Next(0, 9);
                if (sudoku[x, y] != " ")
                {
                    string[,] copy = new string[9, 9];
                    Array.Copy(sudoku, copy, sudoku.Length);
                    sudoku[x, y] = " ";
                    SolveSudoku();
                    if (IsFull())
                    {
                        copy[x, y] = " ";
                        num--;
                    }
                    Array.Copy(copy, sudoku, sudoku.Length);
                }
            } while (num > 0);
        }

        private bool IsFull()
        {
            int count = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j].Trim() == "")
                        count++;
                }
            }
            return count == 0;
        }

        private bool IsValid(int x, int y, int number)
        {
            string[] row = GetRow(x);
            string[] col = GetColumn(y);
            string[] box = GetBox(x, y);
            foreach (string item in row)
            {
                if (item.Trim() == "") continue;
                if (number == int.Parse(item))
                    return false;
            }
            foreach (string item in col)
            {
                if (item.Trim() == "") continue;
                if (number == int.Parse(item))
                    return false;
            }
            foreach (string item in box)
            {
                if (item.Trim() == "") continue;
                if (number == int.Parse(item))
                    return false;
            }
            return true;
        }

        private void btnGen_Clicked(object sender, EventArgs e)
        {
           //生成数独数组
            GetNewSudoku();
            for (var i = 0; i < 9; i++)//第I列
            {
                for (var j = 0; j < 9; j++)//第J行
                {
                    string name = "btn_" + j + "_" + i;
                    Button btn = (Button)this.SudokuGrid.FindByName(name);
                    btn.Text = sudoku[j, i].ToString();
                    if (btn.Text.Trim() != "")
                        btn.IsEnabled = false;
                    else
                        btn.IsEnabled = true;
                }
            }
        }

        private void btnSudokuItem_Clicked(object sender, EventArgs e)
        {
            foreach (View v in this.SudokuGrid.Children)
            {
                if (v is Button btn)
                {
                    btn.BorderColor = Color.Black;
                }
            }
            Button b = (Button)sender;
            b.BorderColor = Color.White;
            currentButton = b;
        }
    }
}