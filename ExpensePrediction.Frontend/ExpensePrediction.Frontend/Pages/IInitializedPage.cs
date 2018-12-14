using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.Frontend.Pages
{
    interface IInitializedPage
    {
        Task Initialize();
    }
}
