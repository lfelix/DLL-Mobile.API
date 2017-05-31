using System;
using System.Data.Entity;

namespace DLLMobileAPI
{
    internal class Initializer : DropCreateDatabaseIfModelChanges<ApiContext>
    {
        protected override void Seed(ApiContext context)
        {
        }
    }
}