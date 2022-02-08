using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.Test;

internal class MyConf : IConfiguration
{
    public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        throw new NotImplementedException();
    }

    public IChangeToken GetReloadToken()
    {
        throw new NotImplementedException();
    }

    public IConfigurationSection GetSection(string key)
    {
        throw new NotImplementedException();
    }
}

internal class MyConfSection : IConfigurationSection
{
    public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Key => throw new NotImplementedException();

    public string Path => throw new NotImplementedException();

    public string Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        throw new NotImplementedException();
    }

    public IChangeToken GetReloadToken()
    {
        throw new NotImplementedException();
    }

    public IConfigurationSection GetSection(string key)
    {
        throw new NotImplementedException();
    }
}
