using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Models;

public partial class Airplane
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string FlightNumber { get; set; } = null!;

    [StringLength(50)]
    public string Brand { get; set; } = null!;

    public int? PreviousTerminal { get; set; }

    public int? CurrentTerminal { get; set; }

    public string ImgPath { get; set; } = null!;

    public bool CanTakeOff { get; set; }

    [InverseProperty("Airplane")]
    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public override string ToString() => $"FlightNumber: {FlightNumber}, Brand: {Brand}";
}
