using System;
namespace Cw7.Services
{
	public interface IDatabaseService
	{
		Task<IEnumerable<TripDTO>> GetTrips();

		Task<int> GetClientTripsCount(int idClient);
		Task RemoveClient(int idClient);

		Task<bool> DoesClientExists(ClientToTripDTO clientToTripDTO);
		Task AddClient(ClientToTripDTO clientToTripDTO);
		Task<bool> DoesClientHasTrip(ClientToTripDTO clientToTripDTO);
		Task<bool> DoesTripExists(ClientToTripDTO clientToTripDTO);
		Task AssignClientToTrip(ClientToTripDTO clientToTripDTO);
	}
}

