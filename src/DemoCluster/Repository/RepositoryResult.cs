using System.Collections.Generic;
using System.Linq;

namespace DemoCluster.Repository
{
    public class RepositoryResult
    {
        private readonly List<RepositoryError> _errors = new List<RepositoryError>();
        public string Key { get; protected set; }
        public IEnumerable<string> Keys { get; protected set; }
        public bool Succeeded { get; protected set; }
        public IEnumerable<RepositoryError> Errors => _errors;

        public static RepositoryResult Success()
        {
            return new RepositoryResult { Succeeded = true, Key = string.Empty, Keys = new List<string>() };
        }

        public static RepositoryResult Success(string key = null)
        {
            return new RepositoryResult { Succeeded = true, Key = key ?? string.Empty, Keys = new List<string>() };
        }

        public static RepositoryResult Success(IEnumerable<string> keys = null)
        {
            return new RepositoryResult { Succeeded = true, Key = string.Empty, Keys = keys ?? new List<string>() };
        }

        public static RepositoryResult Failed(params RepositoryError[] errors)
        {
            var result = new RepositoryResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }

            return result;
        }

        public override string ToString() =>
            Succeeded
                ? "Succeeded"
                : $"Failed : {string.Join(",", Errors.Select(x => x.Code).ToList())}";
    }
}