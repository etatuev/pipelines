# Pipelines

Simple pipeline implementation

### Run tests

`dotnet test`

### Code coverage

#### Install coverlet tool

`dotnet tool install --global coverlet.console`

#### Generate code-coverage report

_Note the --no-build arg, it's required._

`coverlet --target="dotnet" --targetargs="test --no-build" .\Pipelines.Tests\bin\Debug\netcoreapp3.1\Pipelines.Tests.dll`

```
+-----------+-------+--------+--------+
| Module    | Line  | Branch | Method |
+-----------+-------+--------+--------+
| Pipelines | 90.9% | 70%    | 88.88% |
+-----------+-------+--------+--------+
```

## Example

```c#
T LogStep<T>(T prev)
{
    _console.WriteLine(prev.ToString());
    return prev;
}

Pipeline<string,int> stringLengthPipeline = PipelineBuilder.Create<string>()
    .AddStep(new ConcatStringPipelineStep(", let's count "))
    .AddStep(new ConcatStringPipelineStep("this string length!"))
    .AddStep(LogStep)
    .AddStep(new StringLengthPipelineStep())
    .Build();


Pipeline<string,string> pipeline = PipelineBuilder
    .Create<string>()
    .AddStep(LogStep)
    .AddStep(stringLengthPipeline)
    .AddStep(length => $"So, result is {length}")
    .Build();

string result = await pipeline.ExecuteAsync("Hello");

_console.WriteLine(result);
```
Output will be:

```
Hello
Hello, let's count this string length!
So, result is 38
```
