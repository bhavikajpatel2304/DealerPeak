using System;
using System.Collections.Generic;

namespace DealerPeak.Models;

public partial class DealerVehicle
{
    public int DealerVehicleId { get; set; }

    public int DealerId { get; set; }

    public int VehicleId { get; set; }

    public DateTime? DateAdded { get; set; }

    public virtual Dealer Dealer { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
