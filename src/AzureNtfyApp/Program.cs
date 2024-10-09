using CliWrap;
using System.Text;

Console.WriteLine("NTFY is running...");

var stdOutBuffer = new StringBuilder();
var stdErrBuffer = new StringBuilder();


var result = await Cli.Wrap("ntfy")
        .WithArguments(["serve", "--config", "/app/ntfy-vol/server.yml"])
        .WithValidation(CommandResultValidation.None)
        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
        .ExecuteAsync();

var stdOut = stdOutBuffer.ToString();
var stdErr = stdErrBuffer.ToString();
Console.WriteLine(stdOut);
Console.WriteLine(stdErr);
int i = 0;