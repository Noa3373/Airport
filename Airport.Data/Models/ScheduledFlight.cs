using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Models;

public partial class ScheduledFlight
{
    [Key]
    public int Id { get; set; }

    public int FlightId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ScheduledArrivalTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ScheduledDepartureTime { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("ScheduledFlights")]
    public virtual Flight Flight { get; set; } = null!;
}
