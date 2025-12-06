var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL server with persistent data volume
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

// Add the bestblogsdb database
var bestblogsdb = postgres.AddDatabase("bestblogsdb");

// Add the API project
var api = builder.AddProject<Projects.BestBlogs_API>("api")
    .WithReference(bestblogsdb);

// Add frontend (optional - for Aspire-managed frontend)
// Uncomment when frontend is ready to be orchestrated by Aspire
// var frontend = builder.AddNpmApp("frontend", "../../../frontend")
//     .WithReference(api)
//     .WithHttpEndpoint(env: "PORT");

builder.Build().Run();
