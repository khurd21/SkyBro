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

![SkyBro Header](./docs/images/SkyBro-Header.png)

![Deployment to Lambda](https://github.com/khurd21/SkyBro/actions/workflows/deploy-to-lambda.yml/badge.svg)

Sky Bro is a weather observations Skill for Alexa. It is targeted for skydivers that want to know what the sky conditions are at their favorite dropzone before traveling for a gnarly day of sky shredding! 

# How to Use SkyBro

When the skill is enabled on your Alexa account, ask weather questions such as:
- *__Alexa, ask Sky Bro for weather at Sanderson Field__*
- *__Alexa, ask Sky Bro for sky conditions at Skydive Chelan on Saturday__*
- *__Alexa, open Sky Bro__*
    - *__Sky conditions for Skydive Kapowsin tomorrow__*
    - *__What is the weather at Bowerman Field for tomorrow?__*

SkyBro will give an overview of the weather conditions including:

- Wind
- Cloud Coverage
- Flight Rules
- Temperature
- Dew Point
- Adverse Conditions (snow, lightning)

# Want to Use SkyBro?

SkyBro is currently in the Beta stage of development. It is a working Alexa Skill but still needs to be thoroughly tested before
deployment! If you want to be on the Beta list to use the Skill, feel free to contact me. Contact information can be found on my
[GitHub profile](https://github.com/khurd21).

# Tools Used

SkyBro relies on the following technologies:

- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Dotnet 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (or use a package manager)
- [AWS Lambda](https://aws.amazon.com/lambda/)
- [Alexa Skill Kit](https://developer.amazon.com/en-US/alexa/alexa-skills-kit)
- [DynamoDB](https://aws.amazon.com/dynamodb/) (future)

My programming environment uses the following:

- [VSCode](https://code.visualstudio.com)
- [Dotnet CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/)


# Setup Environment

## Here are some steps to follow to get started from the command line:

1. __If not already installed, install the Dotnet 6.0 CLI.__

2. __Clone the repository and `cd` into it.__

3. __Install `Amazon.Lambda.Tools` Global Tools if not already installed.__

```bash
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```bash
    dotnet tool update -g Amazon.Lambda.Tools
```

4. __Run the following commands to build the project.__

```bash
dotnet build
```

This repository consists of two `.csproj` files, one for the testing project and the other for
the Lambda. This is wrapped in a `.sln` file which exists in the root directory of the project.
This simply helps rebuild, test, and debug the project faster.


5. __Execute unit tests__

```bash
    dotnet test
```

# How to Deploy to AWS Lambda

Currently, deployment to AWS Lambda occurs in the command line. It would be cool to have
deployment occur from this repository. For now, this is how to deploy changes:

```bash
    cd "src/WeatherObservations"
    dotnet lambda deploy-function WeatherObservations
```
