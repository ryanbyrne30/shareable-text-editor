namespace DocumentAPI.Repositories;

public class RepositoryFactory(IServiceProvider serviceProvider)
{
    public Repository CreateRepository()
    {
        return serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Repository>();
    }
}