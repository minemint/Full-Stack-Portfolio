using System.ComponentModel.DataAnnotations;
namespace DotnetStockAPI.Models;

public partial class Event
{
    public int eventid { get; set; }
    public required string title { get; set; }
    public DateTime start_date { get; set; }
    public DateTime? end_date { get; set; }
}