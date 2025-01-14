# EF Core - Creating migrations

Add-Migration -o <dest_folder>

Type the name of the migration.

# JWT configuration explanation

The registration of the JWT authentication middleware is done in Program.cs by calling the AddAuthentication method.

Next, we specify the default authentication scheme JwtBearerDefaults.AuthenticationScheme as well as DefaultChallengeScheme.

By calling the AddJwtBearer method, we enable the JWT authenticating using the default scheme, and we pass a parameter, which we use to set up JWT bearer options:

* The issuer is the actual server that created the token (ValidateIssuer=true) --> In appsetting.json would be the "Issuer" value in the "JwtSection" object
* The receiver of the token is a valid recipient (ValidateAudience=true)
* The token has not expired (ValidateLifetime=true)
* The signing key is valid and is trusted by the server (ValidateIssuerSigningKey=true) --> In appsetting.json would be the "Key" value in the "JwtSection" object

Additionally, we are providing values for the issuer, audience, and the secret key that the server uses to generate the signature for JWT.