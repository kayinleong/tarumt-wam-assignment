using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Context;
using Tarumt.WAM.Assignment.Infrastructure.Models;

namespace Tarumt.WAM.Assignment.Infrastructure.Services
{
    public class MovieService(DatabaseContext context)
    {
        public PagedList<Movie> GetAllAsync(int pageNumber, int pageSize, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<Movie>.ToPagedList(
                    context.Set<Movie>()
                        .Include(m => m.MovieVenue)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<Movie>.ToPagedList(
                    context.Set<Movie>()
                        .Where(m => m.Name.Contains(keyword))
                        .Include(m => m.MovieVenue)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public async Task<Movie> GetByIdAsync(string id)
        {
            return await context.Movies!
                .FirstAsync(m => m.Id == id) ?? throw new InvalidOperationException("Movie not found");
        }

        public async Task CreateAsync(Movie movie)
        {
            if (context.Movies!.Any(m => m.Name == movie.Name))
            {
                throw new InvalidOperationException("Name already exists");
            }

            try
            {
                await context.Movies!.AddAsync(movie);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to create Movie");
            }
        }

        public async Task UpdateByIdAsync(Movie movie)
        {
            if (context.Movies!.Any(m => m.Name == movie.Name && m.Id != movie.Id))
            {
                throw new InvalidOperationException("Name already exists");
            }

            var existingMovie = await GetByIdAsync(movie.Id);
            existingMovie.Name = movie.Name;
            existingMovie.Description = movie.Description;
            existingMovie.MovieVenue = movie.MovieVenue;

            if (!string.IsNullOrEmpty(movie.ImageUrl))
            {
                existingMovie.ImageUrl = movie.ImageUrl;
            }

            try
            {
                context.Movies!.Update(existingMovie);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to update Movie");
            }
        }

        public async Task DeleteByIdAsync(Movie movie)
        {
            var existingMovie = await GetByIdAsync(movie.Id);

            try
            {
                context.Movies!.Remove(existingMovie);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to delete MovieShowtime");
            }
        }
    }
}
