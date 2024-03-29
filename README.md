# TektonProductCatalog

This is a Tekton challenge to validate .Net Core knowledge. They want to validate some matters such as Rest, Validation, Cache, external services consumption, Logging, and good development practices using [TDD](https://en.wikipedia.org/wiki/Test-driven_development) as a software development technique.

The [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) is the base of the solution.

The data abstraction was implemented using the [Repository Pattern](https://developer.android.com/codelabs/basic-android-kotlin-training-repository-pattern#0).

[Mediator pattern](https://en.wikipedia.org/wiki/Mediator_pattern) was used to control the flow of the API supported in the ASP .Net Core package `MediatR`

## Installation

1. Download [.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) from the official page and install it.
3. Install [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
4. Clone the repo using **Visual Studio** or from the command line
```sh
git clone https://github.com/ecortezr/TektonProductCatalog.git
```
2. Open the solution `TektonProductCatalog.sln` in **Visual Studio** and run the default project `Product.Api`

## Usage

When you run the solution a web browser will be open. This API was documented with Swagger and you'll see all the available endpoints. You need to add a Product before editing it or getting its associated details.

You can Run Tests in **Visual Studio** by clicking on `Test` -> `Run All Tests` (or `Ctrl + R + A`)

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)
