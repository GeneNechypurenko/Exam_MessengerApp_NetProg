using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MessengerApp_TcpServer_Console.Data;
using MessengerApp_TcpServer_Console.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class UserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);

            if (user != null && user.Password == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error authenticating user: {ex.Message}");
            return false;
        }
    }
    public async Task<bool> AddUserAsync(string name, string password)
    {
        try
        {
            var newUser = new User
            {
                Name = name,
                Password = password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
    public async Task<string> GetContactsJson(string username)
    {
        try
        {
            User user = await _context.Users.SingleOrDefaultAsync(u => u.Name == username);
            if (user != null)
            {
                var contacts = await _context.Contacts
                    .Where(c => c.UserId == user.Id)
                    .Include(c => c.ContactUser)
                    .Select(c => new
                    {
                        Contact = c.ContactUser,
                        Messages = _context.Messages
                            .Where(m => (m.SenderId == user.Id && m.RecipientId == c.ContactUserId) || (m.SenderId == c.ContactUserId && m.RecipientId == user.Id))
                            .ToList()
                    })
                    .ToListAsync();

                string contactsJson = JsonConvert.SerializeObject(contacts);
                Console.WriteLine("Contacts retrieved successfully.");
                return contactsJson;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
}