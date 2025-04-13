﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infra.DataAccess.Repositories;
internal class ExpensesRepository : IExpensesRepository, IExpenseUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense)
    {
       await _dbContext.Expenses.AddAsync(expense);

    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Expenses.FindAsync(id);
        //if(result is null)
        //{
        //    return false;
        //}

        _dbContext.Expenses.Remove(result!);
        //return true;
    }

    public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

       return await _dbContext
            .Expenses
            .AsNoTracking()
            .Where(ex => ex.UserId == user.Id && ex.Date >= startDate && ex.Date <= endDate)
            .OrderBy(ex => ex.Date)
            .ThenBy(ex => ex.Title)
            .ToListAsync();
    }

    public async Task<List<Expense>> GetAll(User user)
    {
       return await _dbContext.Expenses
            .AsNoTracking()
            .Where(ex => ex.UserId == user.Id)
            .ToListAsync();
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }

    async Task<Expense?> IExpensesRepository.GetById(User user, long id)
    {
        return await _dbContext.Expenses
                     .AsNoTracking()
                     .FirstOrDefaultAsync(ex => ex.Id == id && ex.UserId == user.Id);
    }
     async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(ex => ex.Id == id && ex.UserId == user.Id);
    }

}
