# Travis CI
Master: 
[![Build Status](https://travis-ci.org/vaclavm/kentico-onboarding-cs.svg?branch=master)](https://travis-ci.org/vaclavm/kentico-onboarding-cs)

Develop: 
[![Build Status](https://travis-ci.org/vaclavm/kentico-onboarding-cs.svg?branch=develop)](https://travis-ci.org/vaclavm/kentico-onboarding-cs)

#### Summary
Create a Web API that will provide persistence to your JavaScript application (from task 5) using the latest .NET Framework (not .NET Core).

##### Application design
 * Web API will (eventually) provide CRUD operations for items in your JS front-end
 * Database provider might change at any time – has to be isolated in a single assembly
 * Controllers have only and single responsibility – they react to user calls
 * Dependency injection is a must (since you favor TDD, right)
 * It is a simple application, so some simplifications are allowed (some are not):
(/) having a single assembly for all contracts and (single) DTO is perfectly OK
(/) dependency injection framework is referenced in all assemblies
(x) having a public implementation of a contract is NOT OK → their implementation must be internal
(x) having single assembly is NOT OK → (Web) API, (intra-assembly) contracts and database assemblies should emerge by now

#### Technology stack
 * be [as async as possible](https://msdn.microsoft.com/en-us/magazine/dn802603.aspx) in controllers and services and repositories
 * use [Mongo DB as NoSQL](http://docs.mlab.com/languages/) storage provider. [mLab](https://mlab.com/) is at your service (and for free) – `single assembly reference expected`
 * use [Unity as Dependency Injection container](https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/dependency-injection) – `all contract-implementing assemblies and API assembly reference expected`
 * use [NUnit as test automation framework](https://kentico.atlassian.net/wiki/display/TEST/NUnit) and [NSubstitute](http://nsubstitute.github.io/help/getting-started/) as mocking framework – `only test assemblies reference expected`
 * do not use [AutoMapper](https://github.com/AutoMapper/AutoMapper/wiki/Getting-started) as this is not necessary for API of this size, however, get to know it at least theoretically

##### Tooling
 * Use newest (RC is permissible) version of [Visual Studio](https://kentico.atlassian.net/wiki/display/KA/Visual+Studio) – check out the link to ensure you are set-up correctly and leverage your [R#](https://kentico.atlassian.net/wiki/display/KA/ReSharper) license.
 * Use [Postman](https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop) to tune (and debug) API calls

##### Way of work
 * Preferably work in pairs (strong preference)
 * More detailed description of quest at hand is described directly in sub-tasks below
