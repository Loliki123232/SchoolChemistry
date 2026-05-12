using System.Windows.Controls;
using System.Windows;
using SchoolChemistry.Models;

namespace SchoolChemistry.Views
{
    public partial class ElementDetailView : UserControl
    {
        public ElementDetailView(ChemicalElement element)
        {
            InitializeComponent();

            if (element != null)
            {

                CreateDisplayManually(element);
            }
        }

        private void CreateDisplayManually(ChemicalElement element)
        {
            var stackPanel = new StackPanel { Margin = new Thickness(15) };

            // Заголовок (символ)
            stackPanel.Children.Add(new TextBlock
            {
                Text = element.Symbol,
                FontSize = 48,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            });

            // Название
            stackPanel.Children.Add(CreateInfoRow("Название:", element.Name));

            // Атомный номер
            stackPanel.Children.Add(CreateInfoRow("Атомный номер:", element.AtomicNumber.ToString()));

            // Атомная масса
            stackPanel.Children.Add(CreateInfoRow("Атомная масса:", element.AtomicMass.ToString("F4")));

            // Группа
            stackPanel.Children.Add(CreateInfoRow("Группа:", element.GroupNumber?.ToString() ?? "—"));

            // Период
            stackPanel.Children.Add(CreateInfoRow("Период:", element.Period?.ToString() ?? "—"));

            // Описание
            stackPanel.Children.Add(new TextBlock
            {
                Text = "Описание:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 10, 0, 5)
            });
            stackPanel.Children.Add(new TextBlock
            {
                Text = string.IsNullOrEmpty(element.Description) ? "Нет описания" : element.Description,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 10)
            });

            this.Content = stackPanel;
        }

        private StackPanel CreateInfoRow(string label, string value)
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
            panel.Children.Add(new TextBlock { Text = label + " ", FontWeight = FontWeights.Bold, Width = 100 });
            panel.Children.Add(new TextBlock { Text = value });
            return panel;
        }
    }
}