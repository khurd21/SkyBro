Feature: A user asks for help
    In order to get assistance from SkyBro
    As a user of SkyBro
    I want to ask for help

Scenario: A user asks for help
    Given the user asks for "help"
    When the request is made
    Then the response should contain
        | ResponseSnippet |
        | You can ask me for sky conditions at an airport. |
        | Alexa, ask Sky Bro for sky conditions at |
        | I can only provide sky conditions for up to three days in advance. |
    And the reprompt should contain
        | ResponseSnippet |
        | Ask me for sky conditions at |