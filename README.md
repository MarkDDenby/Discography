C# based solution for API interaction technical test

This solution has been created in .Net Core 3.1 using Visual Studio 2019 Professional

Solution Structure
==================

Discography			    - MVC Website
Discography.Tests       - MSTest based unit tests
ServiceClient           - Client used to communicate with MusicBrainz and lyrics.ovh webservices
ServiceClient.Contracts - Programmatic Interfaces implemented in the ServiceClient project.
ServiceClient.Models    - Contains the model classes the service client deserialized into.

Solution Dependencies
=====================

The solution has dependencies on

AspNetCore 3.1
Microsoft.Extensions.Logging
Microsoft SignalR
Moq
MSTest

all dependencies are controlled via Nuget.