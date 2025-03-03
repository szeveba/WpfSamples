using Microsoft.EntityFrameworkCore;

namespace MVVMApp.Model;

internal class Context : DbContext
{
    public Context()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "Todo.db");
    }

    public string DbPath { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}