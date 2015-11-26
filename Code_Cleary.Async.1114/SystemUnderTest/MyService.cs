using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IMyService
{
    Task<int> GetAsync();
}

public sealed class MyService : IMyService
{
    public async Task<int> GetAsync()
    {
        await Task.Delay(1000);
        return 13;
    }
}