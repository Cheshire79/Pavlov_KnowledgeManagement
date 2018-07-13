namespace Identity.DAL.Interface
{
    public interface IFactoryEntitiesManager<TApplicationUserManager, TApplicationRoleManager, TApplicationContext> where TApplicationUserManager : class where TApplicationRoleManager : class where TApplicationContext : class
    {
        TApplicationUserManager CreateUserStore(TApplicationContext applicationContext);
        TApplicationRoleManager CreateRoleStore(TApplicationContext applicationContext);
    }
}
