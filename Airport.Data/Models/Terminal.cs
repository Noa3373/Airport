using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Models;

public partial class Terminal
{
    [Key]
    public int Id { get; set; }

    public int Number { get; set; }

    public int DurationInTerminal { get; set; }

    public bool IsOccupied { get; set; }

    public int PositionX { get; set; }

    public int PositionY { get; set; }

    [InverseProperty("Terminal")]
    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
