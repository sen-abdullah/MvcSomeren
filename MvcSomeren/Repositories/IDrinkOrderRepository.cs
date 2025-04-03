using System.Collections.Generic;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IDrinkOrderRepository
{
    DrinkOrderViewModel Get();
}