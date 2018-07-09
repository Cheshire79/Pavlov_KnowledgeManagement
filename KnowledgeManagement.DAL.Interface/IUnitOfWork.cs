using System;
using System.Threading.Tasks;

namespace KnowledgeManagement.DAL.Interface
{
    public interface IUnitOfWork <TSu, TSk, TL, TSp> : IDisposable where TSu : class where TSk : class where TL : class where TSp : class
    {
        IRepository<TSk> Skills { get; }
        IRepository<TSu> SubSkills { get; }
        IReadOnlyRepository<TL> Levels { get; }
        IRepository<TSp> SpecifyingSkills { get; }
        Task SaveAsync();
    }
}
