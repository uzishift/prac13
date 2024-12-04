using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using libmas;

namespace prac13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool flagCellEditEnding = false;
        bool flagDoneCellEditEnding = false;
        int[,] array;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Практическая работа №3. Вариант 12\nДана матрица размера M × N.\nНайти минимальный среди максимальных элементов ее столбцов.", "О программе");
        }

        private void btnDev_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Демьяхин Роман ИСП-31", "Разработчик");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            flagCellEditEnding = true;
            int indexRow = e.Row.GetIndex();
            int indexColumn = e.Column.DisplayIndex;

            if (Int32.TryParse(((TextBox)e.EditingElement).Text, out int newValue))
            {
                flagDoneCellEditEnding = true;
                array[indexRow, indexColumn] = newValue;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            array = ArrayEditor.Open();
            InputDataGrid.ItemsSource = VisualArray.ToDataTable(array).DefaultView;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ArrayEditor.Save(array);
        }

        private void btnFill_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tbRows.Text, out int numberRow) && int.TryParse(tbCols.Text, out int numberColumn))
            {
                array = new int[numberRow, numberColumn];
                Random rnd = new Random();

                for (int i = 0; i < numberRow; i++)
                {
                    for (int j = 0; j < numberColumn; j++)
                    {
                        array[i, j] = rnd.Next(1, 51);
                    }
                }
                InputDataGrid.ItemsSource = VisualArray.ToDataTable(array).DefaultView;
            }
            else
            {
                MessageBox.Show("Введите корректные значения для строк и столбцов.", "Ошибка");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            InputDataGrid.ItemsSource = null;
            array = null; // сброс массива
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            if (array != null && flagCellEditEnding == flagDoneCellEditEnding)
            {
                // Получаем номер столбца K
                if (int.TryParse(tbK.Text, out int k) && k > 0 && k <= array.GetLength(1))
                {
                    int sum = 0;
                    int product = 1;

                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        sum += array[i, k - 1]; // Индексация начинается с 0
                        product *= array[i, k - 1];
                    }

                    // Выводим результаты
                    tbRez.Text = $"Сумма: {sum}, Произведение: {product}";
                }
                else
                {
                    MessageBox.Show("Введите корректный номер столбца (K).", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Введите значение в таблицу", "Ошибка");
            }
        }
    }
}