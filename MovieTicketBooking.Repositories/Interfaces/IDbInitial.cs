using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IDbInitial
    {
        Task Seed();
    }
}
