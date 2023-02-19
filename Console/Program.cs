using DDPS.Api;
using Microsoft.EntityFrameworkCore;

using (var context = new HotelContext())
{
    var clients = context.Clients.ToListAsync();

    foreach(var client in clients.Result)
    {
        if (clients is null)
            continue;
        else
            Console.WriteLine($"{client.Id} - {client.LastName}");
    }
        

}
