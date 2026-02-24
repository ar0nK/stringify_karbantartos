using StringifyMaintenance.Models;

namespace StringifyMaintenance.Services;

public static class Session
{
    public static User? CurrentUser { get; set; }
}
