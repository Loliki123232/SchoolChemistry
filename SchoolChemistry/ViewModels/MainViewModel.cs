using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using SchoolChemistry.Models;
using SchoolChemistry.DataAccess;
using SchoolChemistry.Helpers;
using SchoolChemistry.Services;
using System.Windows.Controls;

namespace SchoolChemistry.ViewModels
{
    /// <summary>
    /// Главная модель представления (ViewModel) для основного окна приложения.
    /// Реализует паттерн MVVM, управляет отображением тем, элементов и периодической таблицы.
    /// Использует сервис навигации для централизованного управления переходами между представлениями.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly NavigationService _navigationService;
        private object _currentContent;
        private string _searchQuery;

        /// <summary>
        /// Коллекция всех учебных тем, загруженных из базы данных.
        /// </summary>
        public ObservableCollection<Theme> Themes { get; set; }

        /// <summary>
        /// Коллекция всех химических элементов, загруженных из базы данных.
        /// </summary>
        public ObservableCollection<ChemicalElement> AllElements { get; set; }

        /// <summary>
        /// Текущее содержимое, отображаемое в основной области приложения.
        /// Может быть типа <see cref="TextBlock"/>, <see cref="ElementDetailView"/> или <see cref="PeriodicTableView"/>.
        /// </summary>
        public object CurrentContent
        {
            get => _currentContent;
            set { _currentContent = value; OnPropertyChanged(nameof(CurrentContent)); }
        }

        /// <summary>
        /// Строка поиска для поиска химического элемента по символу или названию.
        /// После успешного поиска автоматически очищается.
        /// </summary>
        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(nameof(SearchQuery)); }
        }

        /// <summary>
        /// Команда для выбора темы и отображения её содержимого через сервис навигации.
        /// </summary>
        public ICommand SelectThemeCommand { get; }

        /// <summary>
        /// Команда для поиска химического элемента по введённому запросу.
        /// </summary>
        public ICommand SearchElementCommand { get; }

        /// <summary>
        /// Команда для отображения периодической таблицы химических элементов через сервис навигации.
        /// </summary>
        public ICommand ShowPeriodicTableCommand { get; }

        /// <summary>
        /// Инициализирует новый экземпляр главной модели представления.
        /// Загружает данные из базы данных, инициализирует команды, подписывается на события навигации
        /// и устанавливает содержимое по умолчанию (первая тема или сообщение об ошибке).
        /// </summary>
        public MainViewModel()
        {
            _dbService = new DatabaseService();
            _navigationService = new NavigationService();

            // Подписываемся на события навигации
            _navigationService.NavigationRequested += (content) => CurrentContent = content;

            LoadThemes();
            LoadAllElements();

            SelectThemeCommand = new RelayCommand(parameter => _navigationService.ShowThemeContent(parameter as Theme));
            SearchElementCommand = new RelayCommand(_ => SearchElement());
            ShowPeriodicTableCommand = new RelayCommand(_ => _navigationService.ShowPeriodicTable(AllElements));

            if (Themes != null && Themes.Count > 0)
                _navigationService.ShowThemeContent(Themes[0]);
            else
                CurrentContent = new TextBlock { Text = "Нет данных. Проверьте подключение к БД.", FontSize = 14, Margin = new Thickness(10) };
        }

        /// <summary>
        /// Загружает все темы из базы данных и инициализирует коллекцию <see cref="Themes"/>.
        /// При возникновении ошибки показывает сообщение пользователю и создаёт пустую коллекцию.
        /// </summary>
        private void LoadThemes()
        {
            try
            {
                var themes = _dbService.GetAllThemes();
                if (themes != null && themes.Any())
                {
                    Themes = new ObservableCollection<Theme>(themes);
                }
                else
                {
                    Themes = new ObservableCollection<Theme>();
                    MessageBox.Show("Темы не найдены в базе данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Themes = new ObservableCollection<Theme>();
                MessageBox.Show($"Ошибка загрузки тем: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загружает все химические элементы из базы данных и инициализирует коллекцию <see cref="AllElements"/>.
        /// </summary>
        private void LoadAllElements()
        {
            AllElements = new ObservableCollection<ChemicalElement>(_dbService.GetAllElements());
        }

        /// <summary>
        /// Выполняет поиск химического элемента по символу или названию.
        /// При успешном поиске отображает детальную информацию об элементе через сервис навигации.
        /// При ошибке показывает соответствующее сообщение.
        /// </summary>
        private void SearchElement()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                MessageBox.Show("Введите символ или название химического элемента.",
                    "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var element = _dbService.GetElementBySymbolOrName(SearchQuery);
            if (element == null)
            {
                MessageBox.Show($"Элемент '{SearchQuery}' не найден.",
                    "Не найден", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _navigationService.ShowElementDetails(element);
            SearchQuery = "";
        }

        /// <summary>
        /// Событие, возникающее при изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие <see cref="PropertyChanged"/> для указанного свойства.
        /// </summary>
        /// <param name="name">Имя свойства, которое изменилось.</param>
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}