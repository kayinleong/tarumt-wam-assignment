using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Context;
using Tarumt.WAM.Assignment.Infrastructure.Models;

namespace Tarumt.WAM.Assignment.Infrastructure.Services
{
    public class MovieVenueService(DatabaseContext context)
    {
        public PagedList<MovieVenue> GetAllAsync(int pageNumber, int pageSize, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<MovieVenue>.ToPagedList(
                    context.Set<MovieVenue>()
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<MovieVenue>.ToPagedList(
                    context.Set<MovieVenue>()
                        .Where(m => m.Location.Contains(keyword))
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public async Task<MovieVenue> GetByIdAsync(string id)
        {
            return await context.MovieVenues!
                .FirstAsync(m => m.Id == id) ?? throw new InvalidOperationException("MovieVenue not found");
        }

        public async Task CreateAsync(MovieVenue movieVenue)
        {
            if (context.MovieVenues!.Any(m => m.Location == movieVenue.Location))
            {
                throw new InvalidOperationException("Location already exists");
            }

            try
            {
                await context.MovieVenues!.AddAsync(movieVenue);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to create MovieVenue");
            }
        }

        public async Task UpdateByIdAsync(MovieVenue movieVenue)
        {
            if (context.MovieVenues!.Any(m => m.Location == movieVenue.Location && m.Id != movieVenue.Id))
            {
                throw new InvalidOperationException("Location already exists");
            }

            var existingMovieVenue = await GetByIdAsync(movieVenue.Id);
            existingMovieVenue.Location = movieVenue.Location;
            existingMovieVenue.Coordinate = movieVenue.Coordinate;

            try
            {
                context.MovieVenues!.Update(existingMovieVenue);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to update MovieVenue");
            }
        }

        public async Task DeleteByIdAsync(MovieVenue movieVenue)
        {
            var existingMovieVenue = await GetByIdAsync(movieVenue.Id);

            try
            {
                context.MovieVenues!.Remove(existingMovieVenue);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to delete MovieVenue");
            }
        }
    }
}
