using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace KnowledgeManagement.DAL.Interface
{
    public interface IDataContext<TSu, TSk, TL, TSp> : IDisposable where TSu : class where TSk : class where TL : class where TSp : class
    {
        DbSet<TSu> SubSkills { get; set; }
        DbSet<TSk> Skills { get; set; }
        DbSet<TL> Levels { get; set; }
        DbSet<TSp> SpecifyingSkills { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
