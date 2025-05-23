﻿using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpenseUpdateOnlyRepository
{
    void Update(Expense expense);
    Task<Expense?> GetById(Domain.Entities.User user, long id);
}
