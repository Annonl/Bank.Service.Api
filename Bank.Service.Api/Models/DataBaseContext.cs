using Microsoft.EntityFrameworkCore;

namespace Bank.Service.Api.Models;

/// <summary>
/// Контекст текущей базы данных.
/// </summary>
public class DataBaseContext : DbContext
{
    /// <summary>
    /// Установка связи с базой данных.
    /// </summary>
    /// <param name="options">Опции подключения.</param>
    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Таблица пользователи.
    /// </summary>
    public DbSet<User> Users { get; set; }
    
    /// <summary>
    /// Таблица счет.
    /// </summary>
    public DbSet<Account> Accounts { get; set; }
    
    /// <summary>
    /// Таблица с операциями.
    /// </summary>
    public DbSet<HistoryOperation> Operations { get; set; }

    /// <summary>
    /// Определение связи между классами User, Account и HistoryOperation.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasMany(user => user.Accounts)
            .WithOne(account => account.User!)
            .HasForeignKey(account => account.UserId);
        
        modelBuilder
            .Entity<Account>()
            .HasOne(account => account.User)
            .WithMany(user => user.Accounts)
            .HasForeignKey(account => account.UserId);
        
        modelBuilder
            .Entity<HistoryOperation>()
            .HasOne(history => history.Account)
            .WithMany(account => account.HistoryOperations)
            .HasForeignKey(history => history.AccountId);
        
        modelBuilder
            .Entity<Account>()
            .HasMany(account => account.HistoryOperations)
            .WithOne(history => history.Account)
            .HasForeignKey(history => history.AccountId);
    }
}

