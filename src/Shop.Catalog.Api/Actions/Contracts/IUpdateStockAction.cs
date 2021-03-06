﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Catalog.Api.Actions.Contracts
{
    public interface IUpdateStockAction
    {
        Task<IActionResult> ExecuteAsync(int productId, int amountChanged, CancellationToken cancellationToken);
    }
}