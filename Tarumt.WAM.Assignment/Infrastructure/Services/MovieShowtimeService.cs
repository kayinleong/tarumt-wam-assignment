using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Context;
using Tarumt.WAM.Assignment.Infrastructure.Models;

namespace Tarumt.WAM.Assignment.Infrastructure.Services
{
    public class MovieShowtimeService(DatabaseContext context)
    {
        public PagedList<MovieShowtime> GetAllAsync(int pageNumber, int pageSize, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<MovieShowtime>.ToPagedList(
                    context.Set<MovieShowtime>()
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<MovieShowtime>.ToPagedList(
                    context.Set<MovieShowtime>()
                        .Where(m => m.Name.Contains(keyword))
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public async Task<MovieShowtime> GetByIdAsync(string id)
        {
            return await context.MovieShowtimes!
                .FirstAsync(m => m.Id == id) ?? throw new InvalidOperationException("MovieShowtime not found");
        }

        public async Task CreateAsync(MovieShowtime movieShowtime)
        {
            if (context.MovieShowtimes!.Any(m => m.Name == movieShowtime.Name))
            {
                throw new InvalidOperationException("Name already exists");
            }

            try
            {
                await context.MovieShowtimes!.AddAsync(movieShowtime);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to create MovieShowtime");
            }
        }

        public async Task UpdateByIdAsync(MovieShowtime movieShowtime)
        {
            if (context.MovieShowtimes!.Any(m => m.Name == movieShowtime.Name && m.Id != movieShowtime.Id))
            {
                throw new InvalidOperationException("Name already exists");
            }

            var existingMovieVenue = await GetByIdAsync(movieShowtime.Id);

            try
            {
                context.MovieShowtimes!.Update(existingMovieVenue);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to update MovieShowtime");
            }
        }

        public async Task DeleteByIdAsync(MovieShowtime movieShowtime)
        {
            var existingMovieVenue = await GetByIdAsync(movieShowtime.Id);

            try
            {
                context.MovieShowtimes!.Remove(existingMovieVenue);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to delete MovieShowtime");
            }
        }
    }
}
