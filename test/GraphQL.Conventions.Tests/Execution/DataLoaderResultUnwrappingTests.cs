﻿using System.Threading.Tasks;
using GraphQL.Conventions;
using GraphQL.DataLoader;
using GraphQL.NewtonsoftJson;
using Tests.Templates;

namespace Tests.Execution
{
    public class DataLoaderResultUnwrappingTests : ConstructionTestBase
    {
        [Test]
        public async Task Schema_Will_Execute_With_No_Errors_When_A_Type_Is_In_A_IDataLoaderResult()
        {
            const string query = @"{
                dataLoaderResult
                nestedDataLoaderResult
                anEnum
                nestedEnum
                namedResult
                resultProperty
            }";

            var schema = Schema<BugReproSchemaDataLoaderResult>();

            var result = await schema.ExecuteAsync((e) => e.Query = query);
            ResultHelpers.AssertNoErrorsInResult(result);
        }

        private class BugReproSchemaDataLoaderResult
        {
            public BugReproQueryDataLoaderResult Query { get; }
        }

        public enum BugReproQueryDataLoaderResultEnum
        {
            Zero,
            One
        }

        private class BugReproQueryDataLoaderResult
        {
            public IDataLoaderResult<string> DataLoaderResult() => new DataLoaderResult<string>("Test");

            public IDataLoaderResult<IDataLoaderResult<string>> NestedDataLoaderResult() => new DataLoaderResult<IDataLoaderResult<string>>(new DataLoaderResult<string>("Test"));

            public IDataLoaderResult<BugReproQueryDataLoaderResultEnum> AnEnum() => new DataLoaderResult<BugReproQueryDataLoaderResultEnum>(BugReproQueryDataLoaderResultEnum.One);

            public IDataLoaderResult<IDataLoaderResult<BugReproQueryDataLoaderResultEnum>> NestedEnum() => new DataLoaderResult<IDataLoaderResult<BugReproQueryDataLoaderResultEnum>>(new DataLoaderResult<BugReproQueryDataLoaderResultEnum>(BugReproQueryDataLoaderResultEnum.One));

            [Name("namedResult")]
            public IDataLoaderResult<int> ANamedResult() => new DataLoaderResult<int>(100500);

            public IDataLoaderResult<int> ResultProperty => new DataLoaderResult<int>(100500);

            public IDataLoaderResult<int> ResultField = new DataLoaderResult<int>(100500);
        }
    }
}
