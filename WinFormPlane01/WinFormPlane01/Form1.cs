using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormPlane01
{
    public partial class Form1 : Form
    {
        double time = 0;
        public Form1()
        {
            InitializeComponent();
        }
        // Функция нажатия на кнопку вычислить
        private async void button1_Click(object sender, EventArgs e)
        {
            var s = richTextBox1.Text.Split('\n'); // Текст вводног поля разбиваем на массив строк по переходам на новую строку
            try
            {
                double p = double.Parse(s[0]);
                if (p < 0.01 || p > 0.5)
                {
                    MessageBox.Show(
                                "Были введены некорректные данные. p должно удовлетворять условию 0,01<=p<=0.5",
                                "Ошибка ввода!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }
                int N = int.Parse(s[1]);
                if (N < 3 || N > 25000)
                {
                    MessageBox.Show(
                                "Были введены некорректные данные. N должно быть целочисленным и удовлетворять условию 3<=N<=25000",
                                "Ошибка ввода!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }
                double[] x = new double[N];
                double[] y = new double[N];
                double[] z = new double[N];
                for (int i = 0; i < N; i++)
                {
                    var str = s[i + 2].Split('\t');
                    x[i] = double.Parse(str[0]);
                    y[i] = double.Parse(str[1]);
                    z[i] = double.Parse(str[2]);
                    if (x[i] < -100 || x[i] > 100 || y[i] < -100 || y[i] > 100 || z[i] < -100 || z[i] > 100)
                    {
                        MessageBox.Show(
                                "Были введены некорректные данные. Координаты должны удовлетворять условию -100<=x,y,z<=100",
                                "Ошибка ввода!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                        return;
                    }
                }
                time = 0.0;
                timer1.Interval = 500;
                timer1.Tick+=timer1_Tick;
                timer1.Start();
                var plane = await Task.Run(()=> mathPlanes.CalculateMainPlane(p, x, y, z));
                richTextBox2.Text = String.Format("{0:F6} {1:F6} {2:F6} {3:F6}\n", plane[0], plane[1], plane[2], plane[3]);
                timer1.Stop();
            }
            catch (Exception bug)
            {
                MessageBox.Show(
                                bug.Message,
                                "Ошибка ввода!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
        }
        // Функция нажатия на кнопку открыть файл
        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = ""; // Устанвливаем изначально пустое имя файла в открытом менеджере файлов
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) // Открываем менеджер. Если отмена, то выходим
                return;
            string filename = openFileDialog1.FileName; // Строка, сохраняющая имя файла
            richTextBox1.Text = ""; // Стираем данные из текстбокса1
            richTextBox1.Text = System.IO.File.ReadAllText(filename);// считываем все данные из файла
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files(*.txt)|*.txt"; // Фильтр для файлового менеджера 
            openFileDialog1.Title = "Выберите тестовые данные, которые хотите загрузить";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt";
            radioButton1.Checked = true;
        }
        // Функция нажатия на кнопку сохранить ответ
        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Title = "Сохраните полученный ответ"; // Добавляем заголовок
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) // Открываем файловый менеджер. Если отмена, то выходим
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, richTextBox2.Text);
        }
        // Функция нажатия на кнопку сгенерировать данные
        private async void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) // Если выбрано сгенерировать в поле ввода
            {
                richTextBox1.Text = await Task.Run(()=>mathPlanes.generateCoordinates());
            }
            else // Если выбрана генерация в файл
            {
                saveFileDialog1.FileName = "";
                saveFileDialog1.Title = "Сохраните сгенерированные данные"; // Добавляем заголовок
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) // Открываем файловый менеджер. Если отмена, то выходим
                    return;
                // получаем выбранный файл
                string filename = saveFileDialog1.FileName;
                // сохраняем текст в файл
                System.IO.File.WriteAllText(filename, await Task.Run(() => mathPlanes.generateCoordinates()));
            }
        }
        // Функция нажатия на кнопку Сохранить данные ввода
        private void button5_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Title = "Сохраните данные ввода"; // Добавляем заголовок
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) // Открываем файловый менеджер. Если отмена, то выходим
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, richTextBox1.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time += 0.5; ;
            label4.Text = String.Format("{0} с", time/2.0); // Почему то, времмя считает в 2 раза быстрее. Поэтому делим его на 2
        }
    }
}
