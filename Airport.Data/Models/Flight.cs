using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Models;

public partial class Flight
{
    [Key]
    public int Id { get; set; }

    public int AirplaneId { get; set; }

    public int TerminalId { get; set; }

    [ForeignKey("AirplaneId")]
    [InverseProperty("Flights")]
    public virtual Airplane Airplane { get; set; } = null!;

    [InverseProperty("Flight")]
    public virtual ICollection<FlightsHistory> FlightsHistories { get; set; } = new List<FlightsHistory>();

    [InverseProperty("Flight")]
    public virtual ICollection<ScheduledFlight> ScheduledFlights { get; set; } = new List<ScheduledFlight>();

    [ForeignKey("TerminalId")]
    [InverseProperty("Flights")]
    public virtual Terminal Terminal { get; set; } = null!;
}
