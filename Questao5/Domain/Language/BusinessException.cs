namespace Questao5.Domain.Language
{
    public sealed class BusinessException : Exception
    {
        public string Type { get; }
        public BusinessException(string type, string? msg = null) : base(msg ?? type) => Type = type;
    }
}
