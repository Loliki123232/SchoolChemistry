using System;
using System.Windows;
using System.Windows.Controls;
using SchoolChemistry.Models;
using SchoolChemistry.Views;

namespace SchoolChemistry.Services
{
    /// <summary>
    /// Сервис навигации для перемещения между различными представлениями
    /// </summary>
    public class NavigationService
    {
        // Событие, которое возникает при смене контента
        public event Action<object> NavigationRequested;

        /// <summary>
        /// Показать содержимое темы
        /// </summary>
        /// <param name="theme">Тема для отображения</param>
        public void ShowThemeContent(Theme theme)
        {
            if (theme == null)
                return;

            var textBlock = new TextBlock
            {
                Text = theme.Content,
                TextWrapping = TextWrapping.Wrap,
                Margin = new System.Windows.Thickness(10),
                FontSize = 14,
                LineHeight = 1.5
            };

            NavigationRequested?.Invoke(textBlock);
        }

        /// <summary>
        /// Показать периодическую таблицу
        /// </summary>
        /// <param name="elements">Коллекция элементов для отображения</param>
        public void ShowPeriodicTable(System.Collections.ObjectModel.ObservableCollection<ChemicalElement> elements)
        {
            if (elements == null || elements.Count == 0)
            {
                var errorText = new TextBlock
                {
                    Text = "Нет данных для отображения. Проверьте подключение к базе данных.",
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new System.Windows.Thickness(10),
                    FontSize = 14,
                    Foreground = System.Windows.Media.Brushes.Red
                };
                NavigationRequested?.Invoke(errorText);
                return;
            }

            var periodicTable = new PeriodicTableView(elements);
            NavigationRequested?.Invoke(periodicTable);
        }

        /// <summary>
        /// Показать информацию об элементе
        /// </summary>
        /// <param name="element">Элемент для отображения</param>
        public void ShowElementDetails(ChemicalElement element)
        {
            if (element == null)
            {
                var errorText = new TextBlock
                {
                    Text = "Элемент не найден",
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new System.Windows.Thickness(10),
                    Foreground = System.Windows.Media.Brushes.Red
                };
                NavigationRequested?.Invoke(errorText);
                return;
            }

            var detailView = CreateElementDetailView(element);
            NavigationRequested?.Invoke(detailView);
        }

        /// <summary>
        /// Создать представление детальной информации об элементе
        /// </summary>
        private UserControl CreateElementDetailView(ChemicalElement element)
        {
            var border = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.LightGray,
                BorderThickness = new System.Windows.Thickness(1),
                Margin = new System.Windows.Thickness(10),
                Padding = new System.Windows.Thickness(15),
                CornerRadius = new System.Windows.CornerRadius(5),
                Background = System.Windows.Media.Brushes.White
            };

            var stackPanel = new StackPanel();

            // Заголовок
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"{element.Name} ({element.Symbol})",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkBlue,
                Margin = new System.Windows.Thickness(0, 0, 0, 15)
            });

            // Информационная сетка
            var infoGrid = new Grid();
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.Margin = new System.Windows.Thickness(0, 0, 0, 15);

            // Атомный номер
            infoGrid.Children.Add(new TextBlock
            {
                Text = "Атомный номер:",
                FontWeight = FontWeights.Bold,
                Margin = new System.Windows.Thickness(0, 5, 10, 5)
            });
            Grid.SetRow(infoGrid.Children[infoGrid.Children.Count - 1], 0);
            Grid.SetColumn(infoGrid.Children[infoGrid.Children.Count - 1], 0);

            infoGrid.Children.Add(new TextBlock
            {
                Text = element.AtomicNumber.ToString(),
                Margin = new System.Windows.Thickness(0, 5, 0, 5)
            });
            Grid.SetRow(infoGrid.Children[infoGrid.Children.Count - 1], 0);
            Grid.SetColumn(infoGrid.Children[infoGrid.Children.Count - 1], 1);

            // Атомная масса
            infoGrid.Children.Add(new TextBlock
            {
                Text = "Атомная масса:",
                FontWeight = FontWeights.Bold,
                Margin = new System.Windows.Thickness(0, 5, 10, 5)
            });
            Grid.SetRow(infoGrid.Children[infoGrid.Children.Count - 1], 1);
            Grid.SetColumn(infoGrid.Children[infoGrid.Children.Count - 1], 0);

            infoGrid.Children.Add(new TextBlock
            {
                Text = element.AtomicMass.ToString("F4"),
                Margin = new System.Windows.Thickness(0, 5, 0, 5)
            });
            Grid.SetRow(infoGrid.Children[infoGrid.Children.Count - 1], 1);
            Grid.SetColumn(infoGrid.Children[infoGrid.Children.Count - 1], 1);

            // Группа и период
            infoGrid.Children.Add(new TextBlock
            {
                Text = "Группа / Период:",
                FontWeight = FontWeights.Bold,
                Margin = new System.Windows.Thickness(0, 5, 10, 5)
            });
            Grid.SetRow(infoGrid.Children[infoGrid.Children.Count - 1], 2);
            Grid.SetColumn(infoGrid.Children[infoGrid.Children.Count - 1], 0);

            infoGrid.Children.Add(new TextBlock
            {
                Text = $"{element.GroupNumber} / {element.Period}",
                Margin = new System.Windows.Thickness(0, 5, 0, 5)
            });
            Grid.SetRow(infoGrid.Children[infoGrid.Children.Count - 1], 2);
            Grid.SetColumn(infoGrid.Children[infoGrid.Children.Count - 1], 1);

            stackPanel.Children.Add(infoGrid);

            // Описание
            stackPanel.Children.Add(new TextBlock
            {
                Text = "Описание:",
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Margin = new System.Windows.Thickness(0, 10, 0, 5)
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = element.Description ?? "Нет описания",
                TextWrapping = TextWrapping.Wrap,
                Margin = new System.Windows.Thickness(0, 0, 0, 10)
            });

            border.Child = stackPanel;

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = border
            };

            return new UserControl { Content = scrollViewer };
        }
    }
}