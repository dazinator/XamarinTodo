using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Todo
{
    // Changes below by @cwrea for adaptation to EF Core.
    public class TodoItemDatabase : DbContext
    {
        private readonly IHostingEnvironment _env;

        public TodoItemDatabase(IHostingEnvironment env) : base()
        {
            _env = env;
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        //public static TodoItemDatabase Create(string databasePath)
        //{
        //    var dbContext = new TodoItemDatabase(databasePath);
        //    dbContext.Database.EnsureCreated();
        //    dbContext.Database.Migrate();
        //    return dbContext;
        //}

        public async Task<List<TodoItem>> GetItemsAsync()
        {
            return await TodoItems.ToListAsync();
        }

        public async Task<List<TodoItem>> GetItemsNotDoneAsync()
        {
            return await TodoItems.Where(item => item.Done).ToListAsync();
        }

        public async Task<TodoItem> GetItemAsync(int id)
        {
            return await TodoItems.SingleAsync(item => item.ID == id);
        }

        public async Task<int> SaveItemAsync(TodoItem item)
        {
            if (item.ID == 0)
            {
                await TodoItems.AddAsync(item);
            }
            return await SaveChangesAsync();
        }

        public async Task<int> DeleteItemAsync(TodoItem item)
        {
            TodoItems.Remove(item);
            return await SaveChangesAsync();
        }

        #region Private implementation            

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbName = "TodoSQLite.db";
            var path = System.IO.Path.Combine(_env.ContentRootPath, dbName);          
            optionsBuilder.UseSqlite($"Filename={path};");
            //optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }

        #endregion
    }
}
