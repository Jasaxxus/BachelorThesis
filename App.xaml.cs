using Microsoft.EntityFrameworkCore.Infrastructure;
using SQLitePCL;
using System.Configuration;
using System.Data;
using System.Windows;

namespace JProject
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DatabaseFacade facade = new DatabaseFacade(new JprojectDbContext());
            facade.EnsureCreated();
        }
    }

}
