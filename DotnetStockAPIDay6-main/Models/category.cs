using System;
using System.Collections.Generic;

namespace DotnetStockAPI.Models;

public partial class category
{
    public int categoryid { get; set; }

    public string categoryname { get; set; } = null!;

    public int categorystatus { get; set; }
}
