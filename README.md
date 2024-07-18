Projekt vytvořený  vrámci výběrového řízení pro https://iolabs.ch/. Jedná se o jejich úlohu. Níže je popsáno zadání co mi bylo poskytnuto, pdo ním moje dojmy.

# Zadání

Create simple .NET 6 API app secured with JWT tokens for storing and reading data from singleton service or DB

### API
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

### Data model
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

### Security
- API secured with oAuth 2.0 (Authorization Code)

  - https://keycloak.stage.iolabs.ch/auth/realms/iotest/.well-known/openid-configuration

 - clientID: test-api

- clientSecret: vCDbLdj631sATkfujdg75j9WGzafryKf

- username: be.test

- password: test123

# Moje řešení
- Zadání teda trochu nejasný místy matoucí ale dle mých odhadů jsem snad vše splnil.
- Mám dva Controllery API a APISql (tzn i dva contexty).
  - API je pouze pro uložení do inmemory db.
  - APISql ukládá do SQLite.
    - Je tam jen jedna Migrace.
    - Zároveň jsem ho dělal jako druhý je tudíž lepší aspoň v nějaké struktuře kódu i když ho tam moc není.

- Struktura je snad dle dobrých navyků v .NET.

- Tokeny:
  - Pro obnovení tokenu slouží endpoint refresh-token ta vrátí novou sadu tokenů.

- Nejasnosti:
  - Při odeslání dotazu nevím kde mám vzít referesh token (dle mých znalostí se neposílá). Musel bych se doptávat extra asi? Běžně se neposílá tzn neukládám.

### Swagger
- Swagger funguje včetně přihlášení.
- nefunguje obnovení tokenu.
