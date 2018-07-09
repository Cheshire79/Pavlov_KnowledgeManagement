
namespace KnowledgeManagement.DAL.Interface
{
    public interface IFactoryRepository<TSu, TSk, TL, TSp>  where TSu : class where TSk : class where TL : class where TSp : class
    {
        IRepository<TSu> CreateSubSkillRepository(IDataContext<TSu, TSk, TL, TSp> dataContext);
        IRepository<TSk> CreateSkillRepository(IDataContext<TSu, TSk, TL, TSp> dataContext);
        IReadOnlyRepository<TL> CreateLevelRepository(IDataContext<TSu, TSk, TL, TSp> dataContext);
        IRepository<TSp> CreateSpecifyingSkillRepository(IDataContext<TSu, TSk, TL, TSp> dataContext);
    }
}
