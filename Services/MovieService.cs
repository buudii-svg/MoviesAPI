namespace MoviesAPI.Services
{
    public class MovieService : IMovieService
    {

        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> Create(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
            return await _context.Movies
                .Where(m => genreId == 0 || m.GenreId == genreId)
                .Include(g => g.Genre)
                .OrderByDescending(m => m.Rating)
                .ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _context.Movies.Include(g => g.Genre).FirstOrDefaultAsync(m => m.Id == id);
        }

        //update and delete function doesnot need async because they are not returning anything
        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }
    }
}
