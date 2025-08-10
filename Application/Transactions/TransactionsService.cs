using FinanceService.Models;
using FinanceService.Enums;
using FinanceService.Domain.Repositories;

namespace FinanceService.Application.Transactions
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionRepository _transactionsRepository;

        public TransactionsService(ITransactionRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }
        public async Task<FinancialTransaction> CreateAsync(FinancialTransaction transaction)
        {
            // Validaciones de tipo
            if (!transaction.Type.HasValue || !Enum.IsDefined(typeof(TransactionType), transaction.Type.Value) || transaction.Type.Value == TransactionType.Unknown)
            {
                throw new ArgumentException("Tipo de transacción inválido.");
            }

            // Regla de negocio: ingresos no pueden ser negativos
            if (transaction.Amount < 0 && transaction.Type.Value == TransactionType.Income)
            {
                throw new InvalidOperationException("Un ingreso no puede tener un monto negativo.");
            }

            // Regla de negocio: no puede existir una transacción con monto en 0
            if (transaction.Amount == 0)
            {
                throw new InvalidOperationException("El monto de la transacción no puede ser 0.");
            }

            await _transactionsRepository.AddTransactionAsync(transaction);
            return transaction;
        }

        public async Task DeleteAllAsync()
        {
            await _transactionsRepository.DeleteAllTransactionsAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _transactionsRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transacción con ID {id} no encontrada.");
            }
            await _transactionsRepository.DeleteTransactionAsync(id);
        }

        public async Task<IEnumerable<FinancialTransaction>> GetAllAsync()
        {
           return await _transactionsRepository.GetAllTransactionsAsync();
        }

        public async Task<FinancialTransaction?> GetByIdAsync(int id)
        {
            try
            {
                return await _transactionsRepository.GetTransactionByIdAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public async Task UpdateAsync(int id, FinancialTransaction transaction)
        {
            // Validaciones de tipo
            if (!transaction.Type.HasValue || !Enum.IsDefined(typeof(TransactionType), transaction.Type.Value) || transaction.Type.Value == TransactionType.Unknown)
            {
                throw new ArgumentException("Tipo de transacción inválido.");
            }

            // Regla de negocio: ingresos no pueden ser negativos
            if (transaction.Amount < 0 && transaction.Type.Value == TransactionType.Income)
            {
                throw new InvalidOperationException("Un ingreso no puede tener un monto negativo.");
            }

            if(transaction.Id != id)
            {
                throw new ArgumentException("El ID de la transacción no coincide con el ID proporcionado.");
            }

            await _transactionsRepository.UpdateTransactionAsync(transaction);
        }
    }
}
