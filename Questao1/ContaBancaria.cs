using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria {

        private const double TaxaSaque = 3.50;

        public int Numero { get; }                     
        public string Titular { get; private set; }    
        private double _saldo;                         

        public double Saldo => _saldo;                 

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            _saldo = 0.0;
        }

        public ContaBancaria(int numero, string titular, double depositoInicial)
            : this(numero, titular)                   
        {
            Deposito(depositoInicial);
        }

        public void AlterarTitular(string novoNome)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
                throw new ArgumentException("O nome do titular não pode ficar vazio.");

            Titular = novoNome;
        }

        public void Deposito(double valor)
        {
            if (valor < 0.0)
                throw new ArgumentException("Depósito deve ser um valor positivo.");

            _saldo += valor;
        }

        public void Saque(double valor)
        {
            if (valor < 0.0)
                throw new ArgumentException("Saque deve ser um valor positivo.");

            _saldo -= valor + TaxaSaque;              
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: {_saldo.ToString("C", CultureInfo.GetCultureInfo("en-US"))}";
        }
    }
}
