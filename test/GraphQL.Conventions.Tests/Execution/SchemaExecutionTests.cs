﻿using GraphQL.Conventions.Tests.Templates;
using GraphQL.Conventions.Tests.Templates.Extensions;
using System.Threading.Tasks;
using GraphQL.NewtonsoftJson;

namespace GraphQL.Conventions.Tests.Execution
{
    public class SchemaExecutionTests : ConstructionTestBase
    {
        [Test]
        public async Task Can_Have_Decimals_In_Schema()
        {
            var schema = Schema<SchemaTypeWithDecimal>();
            schema.ShouldHaveQueries(1);
            schema.ShouldHaveMutations(0);
            schema.Query.ShouldHaveFieldWithName("test");
            var result = await schema.ExecuteAsync((e) => e.Query = "query { test }");
            ResultHelpers.AssertNoErrorsInResult(result);
        }

        class SchemaTypeWithDecimal
        {
            public QueryTypeWithDecimal Query { get; }
        }

        class QueryTypeWithDecimal
        {
            public decimal Test => 10;
        }
    }
}
