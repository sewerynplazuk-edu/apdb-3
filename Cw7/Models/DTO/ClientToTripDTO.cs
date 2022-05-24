using System;
namespace Cw7
{
	public class ClientToTripDTO
	{
#pragma warning disable CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ zadeklarowanie pola jako dopuszczającego wartość null.
        public string FirstName { get; set; }
        public string LastName { get; set; }
		public string Email { get; set; }
		public string Telephone { get; set; }
		public string Pesel { get; set; }
		public int IdTrip { get; set; }
		public string TripName { get; set; }
		public DateTime PaymentDate { get; set; }
#pragma warning restore CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ zadeklarowanie pola jako dopuszczającego wartość null.
	}
}

