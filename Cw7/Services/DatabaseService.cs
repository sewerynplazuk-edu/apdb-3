using System;
using Cw7.Models;
using Microsoft.EntityFrameworkCore;

namespace Cw7.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly MasterContext _masterContext;

        public DatabaseService(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }

        public async Task<IEnumerable<TripDTO>> GetTrips()
        {
            return await _masterContext.Trips
                .Select(trip => new TripDTO
                {
                    Name = trip.Name,
                    Description = trip.Description,
                    DateFrom = trip.DateFrom,
                    DateTo = trip.DateTo,
                    MaxPeople = trip.MaxPeople,
                    Countries = trip.IdCountries.Select(country => new CountryDTO { Name = country.Name }).ToList(),
                    Clients = trip.ClientTrips.Select(clientTrip => new ClientDTO { FirstName = clientTrip.IdClientNavigation.FirstName, LastName = clientTrip.IdClientNavigation.LastName }).ToList()
                }).ToListAsync();
        }

        public async Task<int> GetClientTripsCount(int idClient)
        {
            var client = await _masterContext.Clients.Where(client => client.IdClient == idClient).FirstAsync();
            if (client == null)
            {
                return -1;
            }
            return client.ClientTrips.Count();
        }

        public async Task RemoveClient(int idClient)
        {
            var client = new Client() { IdClient = idClient };

            _masterContext.Attach(client);
            _masterContext.Remove(client);

            await _masterContext.SaveChangesAsync();
        }

        public async Task<bool> DoesClientExists(ClientToTripDTO clientToTripDTO)
        {
            var clientsCount = await _masterContext.Clients.CountAsync(client => client.Pesel == clientToTripDTO.Pesel);
            return clientsCount > 0;
        }

        public async Task AddClient(ClientToTripDTO clientToTripDTO)
        {
            var lastClientOrderedById = await _masterContext.Clients.OrderBy(client => client.IdClient).LastAsync();
            var nextId = lastClientOrderedById.IdClient + 1;

            var client = new Client()
            {
                IdClient = nextId,
                FirstName = clientToTripDTO.FirstName,
                LastName = clientToTripDTO.LastName,
                Email = clientToTripDTO.Email,
                Telephone = clientToTripDTO.Telephone,
                Pesel = clientToTripDTO.Pesel
            };

            _masterContext.Add(client);
            await _masterContext.SaveChangesAsync();
        }

        public async Task<bool> DoesClientHasTrip(ClientToTripDTO clientToTripDTO)
        {

            var clientTripsCount = await _masterContext.Clients.CountAsync(client => client.ClientTrips.Count(clientTrip => clientTrip.IdTrip == clientToTripDTO.IdTrip) > 0);
            return clientTripsCount > 0;
        }

        public async Task<bool> DoesTripExists(ClientToTripDTO clientToTripDTO)
        {
            var tripsCount = await _masterContext.Trips.CountAsync(trip => trip.IdTrip == clientToTripDTO.IdTrip);
            return tripsCount > 0;
        }

        public async Task AssignClientToTrip(ClientToTripDTO clientToTripDTO)
        {
            var client = await _masterContext.Clients.Where(client => client.Pesel == clientToTripDTO.Pesel).FirstOrDefaultAsync();
            if (client == null)
            {
                throw new Exception("This method assumes that client was added to the database");
            }

            var clientTrip = new ClientTrip()
            {
                IdClient = client.IdClient,
                IdTrip = clientToTripDTO.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientToTripDTO.PaymentDate > DateTime.Now ? null : clientToTripDTO.PaymentDate
            };

            _masterContext.Add(clientTrip);
            await _masterContext.SaveChangesAsync();
        }
    }
}