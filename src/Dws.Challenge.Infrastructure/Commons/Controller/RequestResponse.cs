using System.Collections.Generic;
using System.Linq;
using Dws.Challenge.Infrastructure.Extensions;

namespace Dws.Challenge.Infrastructure.Commons.Controller
{
    public class RequestResponse
    {
        private Dictionary<string, string> errors = new Dictionary<string, string>();

        public string ErrorMessage { get; protected set; }

        public IReadOnlyDictionary<string, string> Errors => this.errors;

        public bool HasError => !string.IsNullOrWhiteSpace(this.ErrorMessage) || this.Errors.Any();

        public void AddError(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            this.errors.Upsert(key, value);
        }

        public void AddError(KeyValuePair<string, string> error)
        {
            this.AddError(error.Key, error.Value);
        }
    }

    public class RequestResponse<T> : RequestResponse
    {
        public RequestResponse(T content)
        {
            this.Content = content;
        }

        public T Content { get; }
    }
}