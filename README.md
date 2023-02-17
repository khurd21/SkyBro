# Sky Bro: Weather Observations

<img
    src="https://img.shields.io/badge/dotnet-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"
    alt="Website Badge" />
<img
    src="https://img.shields.io/badge/CSharp-239120?style=for-the-badge&logo=csharp&logoColor=white"
    alt="Website Badge" />
<img
    src="https://img.shields.io/badge/Lambda-FF9900?style=for-the-badge&logo=aws-lambda&logoColor=white"
    alt="Website Badge" />
<img
    src="https://img.shields.io/badge/Alexa-00CAFF?style=for-the-badge&logo=amazon-alexa&logoColor=white"
    alt="Website Badge" />

Sky Bro is a weather observations Skill for Alexa. It is targeted for skydivers that want to know what the sky conditions are at their favorite dropzone before traveling for a gnarly day of sky shredding!

Ask weather questions to Alexa:
- Alexa, ask Sky Bro for weather at Sanderson Field
- Alexa, ask Sky Bro for sky conditions at Skydive Chelan

---

## Here are some steps to follow from Visual Studio:

To deploy your function to AWS Lambda, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed function open its Function View window by double-clicking the function name shown beneath the AWS Lambda node in the AWS Explorer tree.

To perform testing against your deployed function use the Test Invoke tab in the opened Function View window.

To configure event sources for your deployed function, for example to have your function invoked when an object is created in an Amazon S3 bucket, use the Event Sources tab in the opened Function View window.

To update the runtime configuration of your deployed function use the Configuration tab in the opened Function View window.

To view execution logs of invocations of your function use the Logs tab in the opened Function View window.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Execute unit tests
```
    cd "WeatherObservations/test/WeatherObservations.Tests"
    dotnet test
```

Deploy function to AWS Lambda
```
    cd "WeatherObservations/src/WeatherObservations"
    dotnet lambda deploy-function
```
