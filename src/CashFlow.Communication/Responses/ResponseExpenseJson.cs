using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Communication.Responses;
public class ResponseExpenseJson
{
    public List<ResponseShortExpenseJson> Expenses { get; set; } = [];
}
