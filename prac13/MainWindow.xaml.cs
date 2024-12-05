using System.Data;
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
using System.Windows.Threading;
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
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
        }
        /// <summary>
        /// Кнопка вывода информации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Практическая работа №13. Вариант 12\nДана матрица размера M * N и целое число K (1 < K < N).\nНайти сумму и произведение элементов K-го столбца данной матрицы.", "О программе");
        }

        /// <summary>
        /// Кнопка закрытия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Изменение DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Кнопка открытия таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            array = ArrayEditor.Open();
            InputDataGrid.ItemsSource = VisualArray.ToDataTable(array).DefaultView;
        }
        /// <summary>
        /// Кнопка сохранения таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ArrayEditor.Save(array);
        }
        /// <summary>
        /// Кнопка заполнения таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Кнопка очистки DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            InputDataGrid.ItemsSource = null;
            array = null;
        }
        /// <summary>
        /// Кнопка нахождения суммы и произведения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (array != null && flagCellEditEnding == flagDoneCellEditEnding)
                {
                    if (int.TryParse(tbK.Text, out int k))
                    {
                        string result = CalculateSumAndProduct(array, k);
                        tbRez.Text = result;
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
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        /// <summary>
        /// Функция нахождения суммы и произведения
        /// </summary>
        /// <param name="array"></param>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string CalculateSumAndProduct(int[,] array, int columnNumber)
        {
            if (array == null || columnNumber <= 0 || columnNumber > array.GetLength(1))
            {
                throw new ArgumentException("Некорректный номер столбца или массив не инициализирован.");
            }

            int sum = 0;
            int product = 1;

            for (int i = 0; i < array.GetLength(0); i++)
            {
                sum += array[i, columnNumber - 1];
                product *= array[i, columnNumber - 1];
            }

            return $"Сумма: {sum}, Произведение: {product}";
        }
        /// <summary>
        /// Отслеживание размера таблицы и отображение ее размеров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int rowCount = InputDataGrid.Items.Count;
            int columnCount = (array != null) ? array.GetLength(1) : 0;

            StatusTableSize.Text = $"Размер таблицы: {rowCount - 1}x{columnCount}";
        }
        /// <summary>
        /// Метод таймера для обновления времени и даты
        /// </summary>
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }
        /// <summary>
        /// Обработчик события тика таймера
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            tbTime.Text = d.ToString("HH:mm:ss");
            tbData.Text = d.ToString("dd.MM.yyyy");
        }
        /// <summary>
        /// Кнока очистки данных для таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmDataGrid_Click(object sender, RoutedEventArgs e)
        {
            tbCols.Clear();
            tbRows.Clear();
        }
        /// <summary>
        /// Кнопка очистки столбца K
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmKClear_Click(object sender, RoutedEventArgs e)
        {
            tbK.Clear();
        }
    }
}