using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public sealed class SystemUnderTest
{
    private readonly IMyService _service;

    public SystemUnderTest(IMyService service)
    {
        _service = service;
    }

    public async Task<int> RetrieveValueAsync()
    {
        return 42 + await _service.GetAsync();
    }

    public static async Task FailAsync()
    {
        await Task.Delay(100);
        throw new Exception("Should fail.");
    }

    public static void Fail()
    {
        throw new Exception("Should fail.");
    }
}
