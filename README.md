# Pipelines

Simple pipeline implementation

### Run tests

`dotnet test`

### Code coverage

#### Install coverlet tool

`dotnet tool install --global coverlet.console`

#### Generate code-coverage report

_Note the --no-build arg, it's required._

`coverlet --target="dotnet" --targetargs="test --no-build"  .\Pipelines.Tests\bin\Debug\netcoreapp3.1\Pipelines.Tests.dll`

```
+-----------+-------+--------+--------+
| Module    | Line  | Branch | Method |
+-----------+-------+--------+--------+
| Pipelines | 90.9% | 70%    | 88.88% |
+-----------+-------+--------+--------+
```
