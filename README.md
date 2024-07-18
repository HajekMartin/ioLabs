Create simple .NET 6 API app secured with JWT tokens for storing and reading data from singleton service or DB

# API
- all endpoints secured with oAuth 2.0 (see below)

- POST saveMessage

  - request: any string data, validate for non empty input using fluent validation

  - store message into "storage" - in memory or DB, see below

  - response: the same as input with text: “You send: $REQUEST”

- GET getMessages

  - list of saved calls

  - optionally with pagination (page, pageSize)

  - optional with search support in data model field request, through query param search

    - text only in request, exact match not needed

- optionaly integrate swagger

# Data model
- Data model

    - Api Call

    - int id primary key

    - datetime requestTime

    - string request

    - string accessToken

    - string refreshToken

    - string user - authorized user email

- Singleton service for storing data can be used

  - Data does not have to be persisted

- Optionaly - persist data

  - use ef core, sqlite or ano other

  - use migrations

# Security
- API secured with oAuth 2.0 (Authorization Code)

  - https://keycloak.stage.iolabs.ch/auth/realms/iotest/.well-known/openid-configuration

 - clientID: test-api

- clientSecret: vCDbLdj631sATkfujdg75j9WGzafryKf

- username: be.test

- password: test123