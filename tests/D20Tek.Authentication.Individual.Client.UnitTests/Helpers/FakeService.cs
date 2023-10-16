//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Helpers;

internal class FakeService : ServiceBase
{
    public FakeService()
        : base(new Mock<ILogger>().Object)
    {
    }

    public async Task<Result<string>> DoServiceOperation()
    {
        var response = await InvokeServiceOperation<string>(() =>
        {
            throw new NotImplementedException();
        });

        return response;
    }

    public async Task<Result> DoOperation()
    {
        var response = await InvokeOperation(() =>
        {
            throw new NotImplementedException();
        });

        return response;
    }
}
