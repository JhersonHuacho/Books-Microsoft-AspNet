﻿namespace Packt.CloudySkiesAir.Chapter5 {
  public abstract class FlightInfoBase : IFlightInfo {
    public Airport ArrivalLocation { get; set; }
    public DateTime ArrivalTime { get; set; }
    public Airport DepartureLocation { get; set; }
    public DateTime DepartureTime { get; set; }
    public TimeSpan Duration => DepartureTime - ArrivalTime;
    public string Id { get; set; }

    public virtual string BuildFlightIdentifier() =>
      $"{Id} {DepartureLocation} - {ArrivalLocation}";
    public override string ToString() => BuildFlightIdentifier();
  }
}