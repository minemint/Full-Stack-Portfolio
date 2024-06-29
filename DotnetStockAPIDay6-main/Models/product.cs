using System;
using System.Collections.Generic;

namespace DotnetStockAPI.Models;

public partial class product
{
    public int productid { get; set; }

    public string? productname { get; set; }

    public decimal? unitprice { get; set; }

    public int? unitinstock { get; set; }

    public string? productpicture { get; set; }

    public int categoryid { get; set; }

    public DateTime createddate { get; set; }

    public DateTime? modifieddate { get; set; }
}
