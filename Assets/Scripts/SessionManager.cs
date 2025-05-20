using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class SessionManager
{
    public static string UserID { get; set; }
    public static string FirstName { get; set; }
    public static string LastName { get; set; }
    public static string Email { get; set; }
    public static string ChatID { get; set; }
    public static bool isLogged { get; set; }
    public static string baseURL { get; set; }
    public static int LastScannedProductID { get; set; } = 0;
    public static bool isAvatarSpawned { get; set; }
    public static string welcomeMsg { get; set; } = "Hi How can I help you?";
}