using System;
using System.Windows.Input;

namespace SchoolChemistry.Helpers
{
    /// <summary>
    /// Реализация интерфейса <see cref="ICommand"/> для создания делегированных команд в WPF/MVVM.
    /// Позволяет связать методы выполнения и проверки доступности команды без создания отдельных классов.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Инициализирует новый экземпляр команды <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Делегат, содержащий логику выполнения команды. Принимает параметр типа <see cref="object"/>.</param>
        /// <param name="canExecute">Делегат, определяющий, может ли команда быть выполнена. Принимает параметр типа <see cref="object"/> и возвращает <see cref="bool"/>. Если не указан, команда всегда доступна.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если параметр <paramref name="execute"/> равен <c>null</c>.</exception>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, может ли команда быть выполнена в текущем состоянии.
        /// </summary>
        /// <param name="parameter">Параметр команды, передаваемый из источника команды.</param>
        /// <returns>
        /// <c>true</c>, если команда может быть выполнена; иначе <c>false</c>.
        /// Если делегат <see cref="_canExecute"/> не задан, всегда возвращает <c>true</c>.
        /// </returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        /// <summary>
        /// Выполняет логику команды.
        /// </summary>
        /// <param name="parameter">Параметр команды, передаваемый из источника команды.</param>
        public void Execute(object parameter) => _execute(parameter);

        /// <summary>
        /// Событие, возникающее при изменении состояния доступности команды.
        /// Использует <see cref="CommandManager.RequerySuggested"/> для автоматического обновления состояния команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}