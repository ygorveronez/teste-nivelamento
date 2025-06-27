namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public Guid Id { get; set; }
        public Guid IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public char Tipo { get; set; }   // 'C' | 'D'
        public decimal Valor { get; set; }
    }
}
