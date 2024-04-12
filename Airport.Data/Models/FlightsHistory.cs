using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Models;

[Table("FlightsHistory")]
public partial class FlightsHistory
{
    [Key]
    public int Id { get; set; }

    public int FlightId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActualArrivalTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActualDepartureTime { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("FlightsHistories")]
    public virtual Flight Flight { get; set; } = null!;
}
