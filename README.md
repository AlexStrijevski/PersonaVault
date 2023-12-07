# PersonaVault

Small project with user registration/login, letting registered user possibility to input Personal Details and then Address details, that will be encrypted and stored in SQL database.

Registration/login:
  When user is registering, password is being hashed and password hash and salt are being stored in database. On successfull login, endpoint return JWT token with user ID in it, when accessing locked endpoints, we are retrieving user ID from JWT token, in order to make requested changes, like updating separate field in personal details or address details.

To work with database I used Entity Framework.
