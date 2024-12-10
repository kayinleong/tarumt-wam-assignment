using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Constants;
using Tarumt.WAM.Assignment.Infrastructure.Context;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Tarumt.WAM.Assignment.Infrastructure.Services
{
    public class TicketService(DatabaseContext context)
    {
        public PagedList<Ticket> GetAllAsync(int pageNumber, int pageSize, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<Ticket>.ToPagedList(
                    context.Set<Ticket>()
                        .Include(m => m.User)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.MovieVenue)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.MovieVenue)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<Ticket>.ToPagedList(
                    context.Set<Ticket>()
                        .Include(m => m.User)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.Movie)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.MovieVenue)
                        .Where(m => m.User.Username.Contains(keyword))
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public PagedList<Ticket> GetAllByStatusAsync(int pageNumber, int pageSize, string keyword, TicketEnum status = TicketEnum.PAID)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<Ticket>.ToPagedList(
                    context.Set<Ticket>()
                        .Where(m => m.Status == status)
                        .Include(m => m.User)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.Movie)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.MovieVenue)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<Ticket>.ToPagedList(
                    context.Set<Ticket>()
                        .Where(m => m.Status == status)
                        .Include(m => m.User)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.Movie)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.MovieVenue)
                        .Where(m => m.User.Username.Contains(keyword))
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public PagedList<Ticket> GetAllByStatusAndUserAsync(int pageNumber, int pageSize, string keyword, User user, TicketEnum status = TicketEnum.PAID)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<Ticket>.ToPagedList(
                    context.Set<Ticket>()
                        .Where(m => m.Status == status)
                        .Include(m => m.User)
                        .Where(m => m.User == user)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.Movie)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.MovieVenue)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<Ticket>.ToPagedList(
                    context.Set<Ticket>()
                        .Where(m => m.Status == status)
                        .Include(m => m.User)
                        .Where(m => m.User == user)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.Movie)
                        .Include(m => m.MovieShowtime)
                        .ThenInclude(m => m.MovieVenue)
                        .Where(m => m.User.Username.Contains(keyword))
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public async Task<Ticket> GetByIdAsync(string id)
        {
            return await context.Tickets!
                .Include(m => m.User)
                .Include(m => m.MovieShowtime)
                .ThenInclude(m => m.Movie)
                .Include(m => m.MovieShowtime)
                .ThenInclude(m => m.MovieVenue)
                .FirstAsync(m => m.Id == id) ?? throw new InvalidOperationException("Ticket not found");
        }

        public async Task CreateAsync(Ticket ticket)
        {
            try
            {
                await context.Tickets!.AddAsync(ticket);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to create Ticket");
            }
        }

        public async Task UpdateByIdAsync(Ticket ticket)
        {
            var existingTicket = await GetByIdAsync(ticket.Id);
            existingTicket.SeatNumbers = ticket.SeatNumbers;
            existingTicket.MovieShowtime = ticket.MovieShowtime;
            existingTicket.Status = ticket.Status;
            existingTicket.User = ticket.User;

            try
            {
                context.Tickets!.Update(existingTicket);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to update Ticket");
            }
        }

        public async Task DeleteByIdAsync(Ticket ticket)
        {
            var existingTicket = await GetByIdAsync(ticket.Id);

            try
            {
                context.Tickets!.Remove(existingTicket);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to delete Ticket");
            }
        }
    }
}
