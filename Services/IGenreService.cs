namespace MoviesAPI.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetById(byte id);
        Task<Genre> Create(Genre genre);
        Genre Update(Genre genre);
        Genre Delete(Genre genre);
    }
}
