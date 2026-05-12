namespace SchoolChemistry.Models
{
    /// <summary>
    /// Модель химического элемента для работы с базой данных.
    /// </summary>
    public class ChemicalElement
    {
        /// <summary>Идентификатор в БД</summary>
        public int Id { get; set; }

        /// <summary>Химический символ (H, O, Fe)</summary>
        public string Symbol { get; set; }

        /// <summary>Название элемента (Водород, Кислород)</summary>
        public string Name { get; set; }

        /// <summary>Атомный номер (порядковый номер)</summary>
        public int AtomicNumber { get; set; }

        /// <summary>Относительная атомная масса</summary>
        public decimal AtomicMass { get; set; }

        /// <summary>Номер группы (может быть null)</summary>
        public int? GroupNumber { get; set; }

        /// <summary>Номер периода (может быть null)</summary>
        public int? Period { get; set; }

        /// <summary>Описание элемента</summary>
        public string Description { get; set; }
    }
}