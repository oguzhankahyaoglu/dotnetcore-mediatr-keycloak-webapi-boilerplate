# dotnetcore-mediatr-keycloak-webapi-boilerplate
A .netcore webapi template using keycloak role-based authentication and mediatr for business layer communications, and also Serilog for logging

As we all do in webapi development, we generate some endpoints (controller/actions) which takes a requestDto object and returns a responseDto object. And we also develop some intermediate classes to convert requests, process them, and return responses of them, which Mediatr library eases it a lot. It just behaves a pipeline for the whole application, in which you can also develop middlewares like logging, caching vs. For example, you want to return a cached response for a variety of requests; It is enough to write an ICacheable interface, create a middleware for it, and execute/return response from the cache, that's it. For detailed explanation of the libray check https://github.com/jbogard/MediatR/wiki

In this template, I created these layers
- WebAPI: takes requests and pushes them to mediatr which is going to find a suitable handler for the request
- BusinessLayer: contains handler implementations
- Contracts: contains DTOs like Request and Response objects, other dto model types
- DataLayer: contains a simple data access logic using EF Core

For authentication with keycloak, you have warmup a Keycloak server and read Keycloak documentation in detail. I just used "password" flow of a client.
