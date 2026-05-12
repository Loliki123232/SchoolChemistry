namespace SchoolChemistry.Models
{
    /// <summary>
    /// Модель учебной темы по химии.
    /// </summary>
    public class Theme
    {
        /// <summary>Идентификатор темы в БД</summary>
        public int Id { get; set; }

        /// <summary>Название темы</summary>
        public string Name { get; set; }

        /// <summary>Теоретическое содержание темы</summary>
        public string Content { get; set; }
    }
}