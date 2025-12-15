using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HotelProgramma.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
