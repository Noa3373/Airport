using System;
using System.Collections.Generic;
using Airport.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data;

public partial class AirportDbContext : DbContext
{
    public AirportDbContext()
    {
    }

    public AirportDbContext(DbContextOptions<AirportDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airplane> Airplanes { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FlightsHistory> FlightsHistories { get; set; }

    public virtual DbSet<ScheduledFlight> ScheduledFlights { get; set; }

    public virtual DbSet<Terminal> Terminals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airplane>(entity =>
        {
            entity.Property(e => e.FlightNumber).IsFixedLength();
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasOne(d => d.Airplane).WithMany(p => p.Flights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flights_Airplanes");

            entity.HasOne(d => d.Terminal).WithMany(p => p.Flights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flights_Terminals");
        });

        modelBuilder.Entity<FlightsHistory>(entity =>
        {
            entity.HasOne(d => d.Flight).WithMany(p => p.FlightsHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightsHistory_Flights");
        });

        modelBuilder.Entity<ScheduledFlight>(entity =>
        {
            entity.HasOne(d => d.Flight).WithMany(p => p.ScheduledFlights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduledFlights_Flights");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
