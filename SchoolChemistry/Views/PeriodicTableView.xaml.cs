using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using SchoolChemistry.Models;

namespace SchoolChemistry.Views
{
    /// <summary>
    /// Представление (View) для отображения периодической таблицы химических элементов Менделеева.
    /// Создаёт визуальную интерактивную таблицу с цветовой кодировкой по группам элементов.
    /// </summary>
    /// <remarks>
    /// <para>Особенности реализации:</para>
    /// <list type="bullet">
    /// <item><description>Поддержка динамического создания карточек элементов на основе данных из БД</description></item>
    /// <item><description>Цветовая дифференциация по группам и типам элементов</description></item>
    /// <item><description>Отдельное отображение лантаноидов и актиноидов</description></item>
    /// <item><description>Всплывающие подсказки с детальной информацией об элементе</description></item>
    /// <item><description>Адаптивный размер карточек (обычный/уменьшенный для редкоземельных)</description></item>
    /// </list>
    /// </remarks>
    public partial class PeriodicTableView : UserControl
    {
        private ObservableCollection<ChemicalElement> _elements;

        /// <summary>
        /// Инициализирует новый экземпляр представления периодической таблицы.
        /// </summary>
        /// <param name="elements">Коллекция химических элементов для отображения в таблице.</param>
        public PeriodicTableView(ObservableCollection<ChemicalElement> elements)
        {
            InitializeComponent();
            _elements = elements;
            this.Loaded += PeriodicTableView_Loaded;
        }

        /// <summary>
        /// Обработчик события загрузки элемента управления.
        /// Вызывает отображение периодической таблицы после полной инициализации компонента.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PeriodicTableView_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayPeriodicTable();
        }

        /// <summary>
        /// Основной метод построения и отображения периодической таблицы.
        /// Создаёт визуальную структуру, включающую заголовки периодов, карточки элементов,
        /// отдельные секции для лантаноидов и актиноидов, а также итоговую информацию.
        /// </summary>
        private void DisplayPeriodicTable()
        {
            var mainPanel = new StackPanel { Margin = new Thickness(10) };

            // Заголовок
            mainPanel.Children.Add(new TextBlock
            {
                Text = "ПЕРИОДИЧЕСКАЯ ТАБЛИЦА ХИМИЧЕСКИХ ЭЛЕМЕНТОВ",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20),
                Foreground = Brushes.DarkBlue
            });

            // Отображаем элементы по периодам
            for (int period = 1; period <= 7; period++)
            {
                var periodElements = _elements.Where(e => e.Period == period).OrderBy(e => e.GroupNumber).ToList();

                if (periodElements.Any())
                {
                    // Заголовок периода
                    var periodTitle = new TextBlock
                    {
                        Text = $"Период {period}",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 10, 0, 5),
                        Foreground = Brushes.DarkBlue
                    };
                    mainPanel.Children.Add(periodTitle);

                    // Контейнер для элементов периода
                    var wrapPanel = new WrapPanel
                    {
                        Margin = new Thickness(0, 0, 0, 15)
                    };

                    foreach (var element in periodElements)
                    {
                        wrapPanel.Children.Add(CreateElementCard(element));
                    }

                    mainPanel.Children.Add(wrapPanel);
                }
            }

            // Лантаноиды (57-71)
            var lanthanides = _elements.Where(e => e.AtomicNumber >= 57 && e.AtomicNumber <= 71).OrderBy(e => e.AtomicNumber).ToList();
            if (lanthanides.Any())
            {
                var lanthanideTitle = new TextBlock
                {
                    Text = "Лантаноиды (57-71) - Редкоземельные элементы",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 15, 0, 5),
                    Foreground = Brushes.DarkGreen
                };
                mainPanel.Children.Add(lanthanideTitle);

                var wrapPanel = new WrapPanel { Margin = new Thickness(0, 0, 0, 15) };
                foreach (var element in lanthanides)
                {
                    wrapPanel.Children.Add(CreateElementCard(element, isSmall: true));
                }
                mainPanel.Children.Add(wrapPanel);
            }

            // Актиноиды (89-103)
            var actinides = _elements.Where(e => e.AtomicNumber >= 89 && e.AtomicNumber <= 103).OrderBy(e => e.AtomicNumber).ToList();
            if (actinides.Any())
            {
                var actinideTitle = new TextBlock
                {
                    Text = "Актиноиды (89-103) - Радиоактивные элементы",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 15, 0, 5),
                    Foreground = Brushes.DarkRed
                };
                mainPanel.Children.Add(actinideTitle);

                var wrapPanel = new WrapPanel { Margin = new Thickness(0, 0, 0, 15) };
                foreach (var element in actinides)
                {
                    wrapPanel.Children.Add(CreateElementCard(element, isSmall: true));
                }
                mainPanel.Children.Add(wrapPanel);
            }

            // Информация о количестве элементов
            var infoText = new TextBlock
            {
                Text = $"Всего элементов: {_elements.Count}",
                FontSize = 12,
                Foreground = Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 10)
            };
            mainPanel.Children.Add(infoText);

            // Скроллер для всей страницы
            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = mainPanel
            };

            this.Content = scrollViewer;
        }

        /// <summary>
        /// Создаёт визуальную карточку для отдельного химического элемента.
        /// </summary>
        /// <param name="element">Химический элемент, для которого создаётся карточка.</param>
        /// <param name="isSmall">Указывает, следует ли использовать уменьшенный размер карточки (для лантаноидов и актиноидов).</param>
        /// <returns>
        /// Объект <see cref="Border"/>, содержащий форматированную информацию об элементе:
        /// атомный номер, символ, название и цветовое оформление в зависимости от группы.
        /// Карточка имеет всплывающую подсказку с детальной информацией.
        /// </returns>
        private Border CreateElementCard(ChemicalElement element, bool isSmall = false)
        {
            int cardWidth = isSmall ? 90 : 110;
            int cardHeight = isSmall ? 80 : 100;

            var border = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1.5),
                Width = cardWidth,
                Height = cardHeight,
                Margin = new Thickness(3),
                Background = GetElementColor(element),
                CornerRadius = new CornerRadius(5),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            var stackPanel = new StackPanel
            {
                Margin = new Thickness(3)
            };

            // Атомный номер
            var atomicNumber = new TextBlock
            {
                Text = element.AtomicNumber.ToString(),
                FontSize = isSmall ? 9 : 11,
                HorizontalAlignment = HorizontalAlignment.Left,
                Foreground = Brushes.DarkGray
            };

            // Символ
            var symbol = new TextBlock
            {
                Text = element.Symbol,
                FontWeight = FontWeights.Bold,
                FontSize = isSmall ? 18 : 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.Black
            };

            // Название
            var name = new TextBlock
            {
                Text = isSmall ? (element.Name.Length > 10 ? element.Name.Substring(0, 8) + "." : element.Name) :
                               (element.Name.Length > 12 ? element.Name.Substring(0, 10) + "." : element.Name),
                FontSize = isSmall ? 8 : 10,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Foreground = Brushes.DarkSlateGray
            };

            stackPanel.Children.Add(atomicNumber);
            stackPanel.Children.Add(symbol);
            stackPanel.Children.Add(name);

            border.Child = stackPanel;

            // Подсказка
            border.ToolTip = new ToolTip
            {
                Content = $"{element.Name} ({element.Symbol})\nАтомный номер: {element.AtomicNumber}\nАтомная масса: {element.AtomicMass:F4}\n{element.Description}",
                Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse
            };

            return border;
        }

        /// <summary>
        /// Определяет цвет фона карточки элемента на основе его группы или типа.
        /// </summary>
        /// <param name="element">Химический элемент, для которого определяется цвет.</param>
        /// <returns>
        /// Кисть (<see cref="Brush"/>) соответствующего цвета:
        /// <list type="bullet">
        /// <item><description>Лантаноиды - светло-оранжевый (#FFF3E0)</description></item>
        /// <item><description>Актиноиды - светло-розовый (#FCE4EC)</description></item>
        /// <item><description>Группа 1 (щелочные) - светло-красный</description></item>
        /// <item><description>Группа 17 (галогены) - светло-оранжевый</description></item>
        /// <item><description>Группа 18 (инертные) - светло-голубой</description></item>
        /// <item><description>Остальные группы - соответствующие цвета палитры</description></item>
        /// </list>
        /// </returns>
        private Brush GetElementColor(ChemicalElement element)
        {
            if (element.AtomicNumber >= 57 && element.AtomicNumber <= 71)
                return (Brush)new BrushConverter().ConvertFrom("#FFF3E0");

            if (element.AtomicNumber >= 89 && element.AtomicNumber <= 103)
                return (Brush)new BrushConverter().ConvertFrom("#FCE4EC");

            return element.GroupNumber switch
            {
                1 => (Brush)new BrushConverter().ConvertFrom("#FFCDD2"), // Щелочные
                2 => (Brush)new BrushConverter().ConvertFrom("#FFF9C4"), // Щёлочноземельные
                3 => (Brush)new BrushConverter().ConvertFrom("#C8E6C9"),
                4 => (Brush)new BrushConverter().ConvertFrom("#BBDEFB"),
                5 => (Brush)new BrushConverter().ConvertFrom("#B3E5FC"),
                6 => (Brush)new BrushConverter().ConvertFrom("#B2EBF2"),
                7 => (Brush)new BrushConverter().ConvertFrom("#B2DFDB"),
                8 => (Brush)new BrushConverter().ConvertFrom("#D1C4E9"),
                9 => (Brush)new BrushConverter().ConvertFrom("#E1BEE7"),
                10 => (Brush)new BrushConverter().ConvertFrom("#F8BBD0"),
                11 => (Brush)new BrushConverter().ConvertFrom("#FFCCBC"),
                12 => (Brush)new BrushConverter().ConvertFrom("#D7CCC8"),
                13 => (Brush)new BrushConverter().ConvertFrom("#FFF3E0"),
                14 => (Brush)new BrushConverter().ConvertFrom("#EFEBE9"),
                15 => (Brush)new BrushConverter().ConvertFrom("#E8EAF6"),
                16 => (Brush)new BrushConverter().ConvertFrom("#C8E6C9"),
                17 => (Brush)new BrushConverter().ConvertFrom("#FFAB91"), // Галогены
                18 => (Brush)new BrushConverter().ConvertFrom("#B3E5FC"), // Инертные
                _ => (Brush)new BrushConverter().ConvertFrom("#F5F5F5")
            };
        }
    }
}