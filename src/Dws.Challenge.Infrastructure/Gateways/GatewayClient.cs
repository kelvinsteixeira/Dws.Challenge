using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dws.Challenge.Infrastructure.Gateways.Interfaces;

namespace Dws.Challenge.Infrastructure.Gateways
{
    public class GatewayClient : IGatewayClient
    {
        public GatewayClient()
        {

        }

        public T GetAsync<T>(string resource)
        {
            throw new NotImplementedException();
        }
    }
}
