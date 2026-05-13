using Xunit;
using SchoolChemistry.Helpers;
using SchoolChemistry.Models;

namespace SchoolChemistry.Tests
{
    public class UnitTests
    {
        /// <summary>
        /// Тест 1: Проверка создания модели Theme
        /// </summary>
        [Fact]
        public void Test1_ThemeModel_ShouldCreateCorrectly()
        {
            // Arrange
            var theme = new Theme
            {
                Id = 1,
                Name = "Тестовая тема",
                Content = "Тестовое содержимое"
            };

            // Assert
            Assert.Equal(1, theme.Id);
            Assert.Equal("Тестовая тема", theme.Name);
            Assert.Equal("Тестовое содержимое", theme.Content);
        }

        /// <summary>
        /// Тест 2: Проверка создания модели ChemicalElement
        /// </summary>
        [Fact]
        public void Test2_ChemicalElementModel_ShouldCreateCorrectly()
        {
            // Arrange
            var element = new ChemicalElement
            {
                Id = 1,
                Symbol = "H",
                Name = "Водород",
                AtomicNumber = 1,
                AtomicMass = 1.008m,
                GroupNumber = 1,
                Period = 1,
                Description = "Самый лёгкий элемент"
            };

            // Assert
            Assert.Equal(1, element.AtomicNumber);
            Assert.Equal("H", element.Symbol);
            Assert.Equal("Водород", element.Name);
            Assert.Equal(1.008m, element.AtomicMass);
        }

        /// <summary>
        /// Тест 3: Проверка выполнения команды RelayCommand
        /// </summary>
        [Fact]
        public void Test3_RelayCommand_ExecuteAndCanExecute_ShouldWorkCorrectly()
        {
            // Arrange
            bool wasExecuted = false;
            object receivedParameter = null;

            var command = new RelayCommand(
                execute: (param) =>
                {
                    wasExecuted = true;
                    receivedParameter = param;
                },
                canExecute: (param) => param != null
            );

            // Assert - проверка CanExecute
            Assert.False(command.CanExecute(null));
            Assert.True(command.CanExecute("test"));

            // Act
            command.Execute("testParameter");

            // Assert
            Assert.True(wasExecuted);
            Assert.Equal("testParameter", receivedParameter);
        }

        /// <summary>
        /// Тест 4: Проверка свойств Theme (дополнительный)
        /// </summary>
        [Fact]
        public void Test4_ThemeProperties_ShouldWorkCorrectly()
        {
            // Arrange
            var theme = new Theme();

            // Act
            theme.Id = 5;
            theme.Name = "Химические элементы";
            theme.Content = "Содержимое темы";

            // Assert
            Assert.Equal(5, theme.Id);
            Assert.Equal("Химические элементы", theme.Name);
            Assert.Equal("Содержимое темы", theme.Content);
            Assert.IsType<int>(theme.Id);
            Assert.IsType<string>(theme.Name);
        }
    }
}
