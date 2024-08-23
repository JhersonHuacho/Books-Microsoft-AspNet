namespace Packt.CloudySkiesAir.Chapter4;

public class FlightTracker {
    private readonly List<Flight> _flights = new();

    public Flight ScheduleNewFlight(string id, string dest, DateTime depart) {
        Flight flight = new(id, dest, depart) {
            Status = FlightStatus.Inbound
        };
        return ScheduleNewFlight(flight);
    }

    public Flight ScheduleNewFlight(Flight flight) 
    {
        _flights.Add(flight);
        return flight;
    }

    public void DisplayFlights() {
        foreach (Flight f in _flights) {
            Console.WriteLine($"{f.Id,-9} {f.Destination, -5} {DateHelpers.Format(f.DepartureTime), -21} {f.Gate, -5} {f.Status}");
        }
    }

    public Flight? MarkFlightDelayed(string id, DateTime newDepartureTime) {
        Action<Flight> updateAction = flight => {
            flight.DepartureTime = newDepartureTime;
            flight.Status = FlightStatus.Delayed;
            Console.WriteLine($"{id} delayed until {DateHelpers.Format(newDepartureTime)}");
        };
        return UpdateFlight(id, updateAction);
    }

    public Flight? MarkFlightArrived(string id, DateTime arrivalTime, string gate) {
        Flight? flight = FindFlightById(id);
        if (flight != null) {
            flight.ArrivalTime = arrivalTime;
            flight.Gate = gate;
            flight.Status = FlightStatus.OnTime;
            Console.WriteLine($"{id} arrived at {arrivalTime.Format()}.");
        } else {
            Console.WriteLine($"{id} could not be found");
        }
        return flight;
    }

    public Flight? MarkFlightDeparted(string id, DateTime departureTime) {
        Flight? flight = FindFlightById(id);
        if (flight != null) {
            flight.DepartureTime = departureTime;
            flight.Status = FlightStatus.Departed;
            Console.WriteLine($"{id} departed at {DateHelpers.Format(departureTime)}.");
        } else {
            Console.WriteLine($"{id} could not be found");
        }
        return flight;
    }

    private Flight? UpdateFlight(string id, Action<Flight> updateAction) {
        Flight? flight = FindFlightById(id);
        if (flight != null) {
            updateAction(flight);
        } else {
            Console.WriteLine($"{id} could not be found");
        }
        return flight;
    }

    public Flight? FindFlightById(string id) => _flights.FirstOrDefault(f => f.Id == id);

    public void DisaplayMatchingFlights(List<Flight> flights, Func<Flight, bool> shouldDisplay) {
        foreach (Flight flight in flights) {
            if (shouldDisplay(flight)) {
                Console.WriteLine($"{flight.Id,-9} {flight.Destination,-5} {DateHelpers.Format(flight.DepartureTime),-21} {flight.Gate,-5} {flight.Status}");
            }
        }
    }
}