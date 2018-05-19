
using System.Linq;


namespace KnowledgeManagement.DAL.SpecifyingSkill.Repository
{
   
    public interface IReadOnlyRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(int id);
       
    }
}
